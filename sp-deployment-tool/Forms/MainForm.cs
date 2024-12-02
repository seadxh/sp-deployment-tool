using Microsoft.Data.SqlClient;
using sp_deployment_tool.Helpers;

namespace sp_deployment_tool {
    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();

            databases_checkbox_list.CheckOnClick = true;

            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.FormClosing += MainForm_FormClosing;

            this.KeyDown += MainForm_KeyDown;
            this.KeyPreview = true;

            LoadDatabases();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.S) {
                e.SuppressKeyPress = true;

                SqlKeywords.HighlightSQLSyntax(ref sp_content_input);
            }
        }
        private void beautify_button_Click(object sender, EventArgs e) {
            SqlKeywords.HighlightSQLSyntax(ref sp_content_input);
        }

        private void save_sp_button_Click(object sender, EventArgs e) {
            string newSpContent = sp_content_input.Text.Trim();
            if (string.IsNullOrWhiteSpace(newSpContent)) {
                MessageBox.Show("Stored procedure content cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedSchema = schema_combo.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedSchema)) {
                MessageBox.Show("Please select a schema.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string storedProcedureName = sp_name_input.Text.Trim();
            if (string.IsNullOrWhiteSpace(storedProcedureName)) {
                MessageBox.Show("Please enter a stored procedure name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedDatabases = databases_checkbox_list.CheckedItems.Cast<string>().ToList();
            if (!selectedDatabases.Any()) {
                MessageBox.Show("Please select at least one database.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            main_progress_bar.Visible = true;
            main_progress_bar.Minimum = 0;
            main_progress_bar.Maximum = selectedDatabases.Count;
            main_progress_bar.Value = 0;

            string serverName = Session.ServerName;
            string portNumber = Session.PortNumber;

            foreach (var databaseName in selectedDatabases) {
                try {
                    string connectionString = $"Server={serverName},{portNumber};Database={databaseName};Integrated Security=True;";

                    using (SqlConnection conn = new SqlConnection(connectionString)) {
                        conn.Open();

                        string checkSpExistsQuery = $@"
                    SELECT COUNT(*) 
                    FROM sys.objects 
                    WHERE type = 'P' AND name = '{storedProcedureName}'";
                        using (SqlCommand checkCmd = new SqlCommand(checkSpExistsQuery, conn)) {
                            int spExists = (int)checkCmd.ExecuteScalar();

                            if (spExists > 0) {
                                var result = MessageBox.Show($"Stored procedure '{storedProcedureName}' exists in database '{databaseName}'. Do you want to overwrite it?",
                                    "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.No) {
                                    continue;
                                }

                                string dropSpQuery = $"DROP PROCEDURE {selectedSchema}.{storedProcedureName};";
                                using (SqlCommand dropCmd = new SqlCommand(dropSpQuery, conn)) {
                                    dropCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        string createSpQuery = $"CREATE PROCEDURE {selectedSchema}.{storedProcedureName} AS BEGIN {newSpContent} END";
                        using (SqlCommand createCmd = new SqlCommand(createSpQuery, conn)) {
                            createCmd.ExecuteNonQuery();
                        }

                        main_progress_bar.Value++;
                        MessageBox.Show($"Stored procedure '{storedProcedureName}' has been deployed successfully to database '{databaseName}'.",
                            "Deployment Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                } catch (Exception ex) {
                    MessageBox.Show($"Deployment failed for database '{databaseName}': {ex.Message}", "Deployment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    main_progress_bar.Visible = false;
                    return;
                }
            }

            main_progress_bar.Visible = false;
            MessageBox.Show("Deployment process completed.", "Deployment Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            HashSet<string> commonSchemas = null;

            foreach (int index in databases_checkbox_list.CheckedIndices) {
                string databaseName = databases_checkbox_list.Items[index].ToString();
                HashSet<string> currentSchemas = new HashSet<string>();

                using (SqlConnection conn = new SqlConnection($"{Session.ConnectionString};Database={databaseName}")) {
                    try {
                        await conn.OpenAsync();

                        SqlCommand cmd = new SqlCommand("SELECT schema_name FROM information_schema.schemata;", conn);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                            while (reader.Read()) {
                                string schemaName = reader["schema_name"].ToString();
                                if (!schemaName.StartsWith("TF\\")) {
                                    currentSchemas.Add(schemaName);
                                }
                            }
                        }

                        if (commonSchemas == null) {
                            commonSchemas = currentSchemas;
                        } else {
                            commonSchemas.IntersectWith(currentSchemas);
                        }
                    } catch (Exception ex) {
                        MessageBox.Show($"Failed to load schemas for {databaseName}: {ex.Message}");
                        main_progress_bar.Style = ProgressBarStyle.Blocks;
                        schema_combo.Enabled = true;
                        return;
                    }
                }
            }

            if (commonSchemas != null) {
                schema_combo.Items.AddRange(commonSchemas.ToArray());
            }

            main_progress_bar.Style = ProgressBarStyle.Blocks;
            schema_combo.Enabled = true;
        }


        private void Databases_checkbox_list_MouseDown(object sender, MouseEventArgs e) {
            CheckedListBox listBox = sender as CheckedListBox;

            int index = listBox.IndexFromPoint(e.Location);

            if (index == ListBox.NoMatches) {
                listBox.ClearSelected();
            }
        }

        private void exit_button_Click(object sender, EventArgs e) {
            this.FindForm().Close();
        }
    }
}