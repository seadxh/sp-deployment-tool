using Microsoft.Data.SqlClient;
using sp_deployment_tool.Helpers;

namespace sp_deployment_tool {
    public partial class MainForm : Form {
        private bool isUpdatingSchemas = false;
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
            if (e.Control) {
                switch (e.KeyCode) {
                    case Keys.S:
                        e.SuppressKeyPress = true;
                        SqlKeywords.HighlightSQLSyntax(ref sp_content_input);
                        break;
                    case Keys.D:
                        e.SuppressKeyPress = true;
                        save_sp_button_Click(sender, e);
                        break;
                    default:
                        break;
                }
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

            var existingDatabases = new List<string>();
            var deployedDatabases = new List<string>();
            var failedDatabases = new List<string>();

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
                                existingDatabases.Add(databaseName);
                                continue;
                            }
                        }

                        using (SqlCommand createCmd = new SqlCommand(newSpContent, conn)) {
                            createCmd.ExecuteNonQuery();
                        }

                        deployedDatabases.Add(databaseName);
                        main_progress_bar.Value++;
                    }
                } catch (Exception) {
                    failedDatabases.Add(databaseName);
                }
            }

            main_progress_bar.Value = 0;

            if (existingDatabases.Any()) {
                var result = MessageBox.Show(
                    $"Stored procedure '{storedProcedureName}' exists in these databases:\n- {string.Join("\n- ", existingDatabases)}\n\nDo you want to overwrite them?",
                    "Confirm Overwrite",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes) {
                    foreach (var databaseName in existingDatabases) {
                        try {
                            string connectionString = $"Server={serverName},{portNumber};Database={databaseName};Integrated Security=True;";
                            using (SqlConnection conn = new SqlConnection(connectionString)) {
                                conn.Open();
                                string dropSpQuery = $"DROP PROCEDURE {selectedSchema}.{storedProcedureName};";
                                using (SqlCommand dropCmd = new SqlCommand(dropSpQuery, conn)) {
                                    dropCmd.ExecuteNonQuery();
                                }

                                using (SqlCommand createCmd = new SqlCommand(newSpContent, conn)) {
                                    createCmd.ExecuteNonQuery();
                                }

                                deployedDatabases.Add(databaseName);
                            }
                        } catch (Exception) {
                            failedDatabases.Add(databaseName);
                        }
                    }
                }
            }

            var summary = new List<string>();
            if (deployedDatabases.Any()) {
                summary.Add($"Stored procedure '{storedProcedureName}' has been deployed successfully to these databases:\n- {string.Join("\n- ", deployedDatabases)}");
            }
            if (failedDatabases.Any()) {
                summary.Add($"Failed to deploy stored procedure to these databases:\n- {string.Join("\n- ", failedDatabases)}");
            }

            if (summary.Any()) {
                MessageBox.Show(string.Join("\n\n", summary), "Deployment Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("No operations were performed.", "Deployment Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!isUpdatingSchemas) {
                BeginInvoke((MethodInvoker)UpdateSchemaCombo);
            }
        }

        private async void UpdateSchemaCombo() {
            if (isUpdatingSchemas) return;
            isUpdatingSchemas = true;

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
                        isUpdatingSchemas = false;
                        return;
                    }
                }
            }

            if (commonSchemas != null) {
                HashSet<string> addedSchemas = new HashSet<string>();
                foreach (var schema in commonSchemas) {
                    if (!addedSchemas.Contains(schema)) {
                        schema_combo.Items.Add(schema);
                        addedSchemas.Add(schema);
                    }
                }
            }

            main_progress_bar.Style = ProgressBarStyle.Blocks;
            schema_combo.Enabled = true;
            isUpdatingSchemas = false;
        }

        private void remove_sp_button_Click(object sender, EventArgs e) {
            string storedProcedureName = sp_name_input.Text.Trim();
            if (string.IsNullOrWhiteSpace(storedProcedureName)) {
                MessageBox.Show("Please enter a stored procedure name to remove.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedSchema = schema_combo.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedSchema)) {
                MessageBox.Show("Please select a schema.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            var confirmationResult = MessageBox.Show(
                $"Are you sure you want to remove the stored procedure '{storedProcedureName}' from all selected databases?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmationResult == DialogResult.No) {
                return;
            }

            try {
                foreach (var databaseName in selectedDatabases) {
                    string connectionString = $"Server={serverName},{portNumber};Database={databaseName};Integrated Security=True;";

                    using (SqlConnection conn = new SqlConnection(connectionString)) {
                        conn.Open();

                        string checkSpExistsQuery = $@"
                        SELECT COUNT(*) 
                        FROM sys.objects 
                        WHERE type = 'P' AND name = '{storedProcedureName}'";
                        using (SqlCommand checkCmd = new SqlCommand(checkSpExistsQuery, conn)) {
                            int spExists = (int)checkCmd.ExecuteScalar();

                            if (spExists == 0) {
                                throw new InvalidOperationException($"Stored procedure '{storedProcedureName}' not found in database '{databaseName}'.");
                            }

                            string dropSpQuery = $"DROP PROCEDURE {selectedSchema}.{storedProcedureName};";
                            using (SqlCommand dropCmd = new SqlCommand(dropSpQuery, conn)) {
                                dropCmd.ExecuteNonQuery();
                            }
                        }

                        main_progress_bar.Value++;
                    }
                }

                main_progress_bar.Value = 0;
                MessageBox.Show(
                    $"Stored procedure '{storedProcedureName}' has been successfully removed from all selected databases.",
                    "Removal Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            } catch (Exception ex) {
                main_progress_bar.Value = 0;
                MessageBox.Show(
                    $"Removal failed: {ex.Message}\nNo changes were made to any databases.",
                    "Removal Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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