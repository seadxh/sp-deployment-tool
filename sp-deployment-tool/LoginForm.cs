using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using sp_deployment_tool.Helpers;

namespace sp_deployment_tool {
    public partial class LoginForm : Form {
        public LoginForm() {
            InitializeComponent();
        }

        private void cancel_button_Click(object sender, EventArgs e) {
            this.Close();
        }

        private async void connect_button_Click(object sender, EventArgs e) {

            progress_bar.Visible = true;
            progress_bar.Style = ProgressBarStyle.Marquee;

            string serverName = server_name_input.Text;
            string portNumber = port_number_input.Text;
            string connectionString = $"Server={serverName},{portNumber};Integrated Security=True;";

            try {
                await Task.Run(() =>
                {
                    using (SqlConnection conn = new SqlConnection(connectionString)) {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases;", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        Session.InitializeConnection(serverName, portNumber);
                    }
                });

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

            //sql-1.use.aws.testing.corp.trackforce.com
            //1433
            progress_bar.Visible = true;
            progress_bar.Style = ProgressBarStyle.Marquee;

            string serverName = server_name_input.Text;
            string portNumber = port_number_input.Text;
            string connectionString = $"Server={serverName},{portNumber};Integrated Security=True;";

            try {
                await Task.Run(() =>
                {
                    using (SqlConnection conn = new SqlConnection(connectionString)) {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases;", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                    }
                });

                progress_bar.Visible = false;
                MessageBox.Show("Test successful.");
            } catch (Exception ex) {
                progress_bar.Visible = false;
                MessageBox.Show($"Failed to connect: {ex.Message}");
            } finally {
                progress_bar.Visible = false;
            }
        }

    }
}
