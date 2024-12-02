using System.Data;
using Microsoft.Data.SqlClient;
using sp_deployment_tool.Helpers;

namespace sp_deployment_tool {
    public partial class LoginForm : Form {
        public LoginForm() {
            InitializeComponent();

            server_name_input.Text = Properties.Settings.Default.SavedServerName;
            port_number_input.Text = Properties.Settings.Default.SavedPortNumber;
        }

        private void cancel_button_Click(object sender, EventArgs e) {
            this.Close();
        }

        private async void connect_button_Click(object sender, EventArgs e) {

            string serverName = server_name_input.Text;
            string portNumber = port_number_input.Text;


            if (string.IsNullOrEmpty(serverName) || string.IsNullOrWhiteSpace(serverName)) {
                MessageBox.Show($"Failed to connect: Server input can't be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(portNumber) || string.IsNullOrWhiteSpace(portNumber)) {
                portNumber = "1433";
            }

            string connectionString = $"Server={serverName},{portNumber};Integrated Security=True;";

            progress_bar.Visible = true;
            progress_bar.Style = ProgressBarStyle.Marquee;

            try {
                await Task.Run(() => {
                    using (SqlConnection conn = new SqlConnection(connectionString)) {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases;", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        Session.InitializeConnection(serverName, portNumber);
                    }
                });

                Properties.Settings.Default.SavedServerName = serverName;
                Properties.Settings.Default.SavedPortNumber = portNumber;
                Properties.Settings.Default.Save();

                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            } catch (Exception ex) {
                MessageBox.Show($"Failed to connect: {ex.Message}");
            } finally {
                progress_bar.Visible = false;
            }
        }


        private async void test_conn_button_Click(object sender, EventArgs e) {

            string serverName = server_name_input.Text;
            string portNumber = port_number_input.Text;


            if (string.IsNullOrEmpty(serverName) || string.IsNullOrWhiteSpace(serverName)) {
                MessageBox.Show($"Failed to connect: Server input can't be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(portNumber) || string.IsNullOrWhiteSpace(portNumber)) {
                portNumber = "1433";
            }

            string connectionString = $"Server={serverName},{portNumber};Integrated Security=True;";

            progress_bar.Visible = true;
            progress_bar.Style = ProgressBarStyle.Marquee;

            try {
                await Task.Run(() => {
                    using (SqlConnection conn = new SqlConnection(connectionString)) {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases;", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                    }
                });

                progress_bar.Visible = false;
                MessageBox.Show("Test successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                progress_bar.Visible = false;
                MessageBox.Show($"Failed to connect: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                progress_bar.Visible = false;
            }
        }

    }
}
