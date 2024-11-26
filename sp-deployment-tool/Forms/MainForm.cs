using Microsoft.Data.SqlClient;
using sp_deployment_tool.Helpers;

namespace sp_deployment_tool {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();

            this.Size = new Size(1366, 768);

            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.FormClosing += MainForm_FormClosing;

            LoadDatabases();
        }


        private void save_sp_button_Click(object sender, EventArgs e) {
            string newSpContent = sp_content_input.Text;

            string databaseName = "ReportV3_Testing2";
            string storedProcedureName = "New_ActivityLog_UpdateById";

            string serverName = Session.ServerName;
            string portNumber = Session.PortNumber;

            string connectionString = $"Server={serverName},{portNumber};Database={databaseName};Integrated Security=True;";

            try {
                using (SqlConnection conn = new SqlConnection(connectionString)) {
                    conn.Open();

                    string checkSpExistsQuery = $@"
                        IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = '{storedProcedureName}')
                        BEGIN
                            -- Drop the existing stored procedure
                            DROP PROCEDURE {storedProcedureName};
                        END";

                    using (SqlCommand checkCmd = new SqlCommand(checkSpExistsQuery, conn)) {
                        checkCmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand(newSpContent, conn)) {
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Stored procedure '{storedProcedureName}' has been updated successfully in database '{databaseName}'.");
                }
            } catch (Exception ex) {
                MessageBox.Show($"Failed to update stored procedure: {ex.Message}");
            }
        }
        private void LoadDatabases() {
            try {
                if (Session.SqlConnection.State != System.Data.ConnectionState.Open) {
                    Session.SqlConnection.Open();
                }

                using (SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases;", Session.SqlConnection)) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {

                            databases_checkbox_list.Items.Add(reader["name"].ToString());
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Failed to load databases: {ex.Message}");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (Application.OpenForms["LoginForm"] is LoginForm otherForm) {
                otherForm.Close();
            }
        }

        private void Databases_checkbox_list_ItemCheck(object sender, ItemCheckEventArgs e) {
            BeginInvoke((MethodInvoker)UpdateSchemaCombo);
        }

        private async void UpdateSchemaCombo() {
            main_progress_bar.Visible = true;
            main_progress_bar.Style = ProgressBarStyle.Marquee;
            schema_combo.Enabled = false;

            schema_combo.Items.Clear();
            HashSet<string> uniqueSchemas = new HashSet<string>();

            foreach (int index in databases_checkbox_list.CheckedIndices) {
                string databaseName = databases_checkbox_list.Items[index].ToString();

                using (SqlConnection conn = new SqlConnection($"{Session.ConnectionString};Database={databaseName}")) {
                    try {
                        await conn.OpenAsync();

                        SqlCommand cmd = new SqlCommand("SELECT schema_name FROM information_schema.schemata;", conn);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                            while (reader.Read()) {
                                string schemaName = reader["schema_name"].ToString();
                                if (!schemaName.StartsWith("TF\\")) {
                                    uniqueSchemas.Add(schemaName);
                                }
                            }
                        }
                    } catch (Exception ex) {
                        MessageBox.Show($"Failed to load schemas for {databaseName}: {ex.Message}");
                    }
                }
            }

            schema_combo.Items.AddRange(uniqueSchemas.ToArray());

            main_progress_bar.Style = ProgressBarStyle.Blocks;
            schema_combo.Enabled = true;
        }
    }
}