namespace sp_deployment_tool {
    partial class LoginForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            connect_button = new Button();
            port_number_input = new TextBox();
            label2 = new Label();
            label1 = new Label();
            server_name_input = new TextBox();
            auth_types_combo = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            username_input = new TextBox();
            password_input = new TextBox();
            label5 = new Label();
            cancel_button = new Button();
            test_conn_button = new Button();
            progress_bar = new ProgressBar();
            SuspendLayout();
            // 
            // connect_button
            // 
            connect_button.Location = new Point(342, 182);
            connect_button.Name = "connect_button";
            connect_button.Size = new Size(75, 23);
            connect_button.TabIndex = 10;
            connect_button.Text = "Connect";
            connect_button.UseVisualStyleBackColor = true;
            connect_button.Click += connect_button_Click;
            // 
            // port_number_input
            // 
            port_number_input.Location = new Point(435, 12);
            port_number_input.Name = "port_number_input";
            port_number_input.Size = new Size(63, 23);
            port_number_input.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(397, 16);
            label2.Name = "label2";
            label2.Size = new Size(32, 15);
            label2.TabIndex = 8;
            label2.Text = "Port:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(69, 20);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 7;
            label1.Text = "Host:";
            // 
            // server_name_input
            // 
            server_name_input.Location = new Point(115, 12);
            server_name_input.Name = "server_name_input";
            server_name_input.Size = new Size(266, 23);
            server_name_input.TabIndex = 6;
            // 
            // auth_types_combo
            // 
            auth_types_combo.FormattingEnabled = true;
            auth_types_combo.Location = new Point(115, 58);
            auth_types_combo.Name = "auth_types_combo";
            auth_types_combo.Size = new Size(189, 23);
            auth_types_combo.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 61);
            label3.Name = "label3";
            label3.Size = new Size(89, 15);
            label3.TabIndex = 12;
            label3.Text = "Authentication:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 101);
            label4.Name = "label4";
            label4.Size = new Size(63, 15);
            label4.TabIndex = 13;
            label4.Text = "Username:";
            // 
            // username_input
            // 
            username_input.Enabled = false;
            username_input.Location = new Point(115, 98);
            username_input.Name = "username_input";
            username_input.Size = new Size(189, 23);
            username_input.TabIndex = 14;
            // 
            // password_input
            // 
            password_input.Enabled = false;
            password_input.Location = new Point(115, 127);
            password_input.Name = "password_input";
            password_input.Size = new Size(189, 23);
            password_input.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(44, 130);
            label5.Name = "label5";
            label5.Size = new Size(60, 15);
            label5.TabIndex = 16;
            label5.Text = "Password:";
            // 
            // cancel_button
            // 
            cancel_button.Location = new Point(423, 182);
            cancel_button.Name = "cancel_button";
            cancel_button.Size = new Size(75, 23);
            cancel_button.TabIndex = 17;
            cancel_button.Text = "Cancel";
            cancel_button.UseVisualStyleBackColor = true;
            cancel_button.Click += cancel_button_Click;
            // 
            // test_conn_button
            // 
            test_conn_button.Location = new Point(15, 182);
            test_conn_button.Name = "test_conn_button";
            test_conn_button.Size = new Size(125, 23);
            test_conn_button.TabIndex = 18;
            test_conn_button.Text = "Test Connection ...";
            test_conn_button.UseVisualStyleBackColor = true;
            test_conn_button.Click += test_conn_button_Click;
            // 
            // progress_bar
            // 
            progress_bar.Location = new Point(16, 162);
            progress_bar.Name = "progress_bar";
            progress_bar.Size = new Size(483, 14);
            progress_bar.TabIndex = 19;
            progress_bar.Visible = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(511, 213);
            Controls.Add(progress_bar);
            Controls.Add(test_conn_button);
            Controls.Add(cancel_button);
            Controls.Add(label5);
            Controls.Add(password_input);
            Controls.Add(username_input);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(auth_types_combo);
            Controls.Add(connect_button);
            Controls.Add(port_number_input);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(server_name_input);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LoginForm";
            Text = "Connect to a Database";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button connect_button;
        private TextBox port_number_input;
        private Label label2;
        private Label label1;
        private TextBox server_name_input;
        private ComboBox auth_types_combo;
        private Label label3;
        private Label label4;
        private TextBox username_input;
        private TextBox password_input;
        private Label label5;
        private Button cancel_button;
        private Button test_conn_button;
        private ProgressBar progress_bar;
    }
}