namespace sp_deployment_tool {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            databases_label = new Label();
            sp_content_input = new RichTextBox();
            save_sp_button = new Button();
            databases_checkbox_list = new CheckedListBox();
            sp_name_input = new TextBox();
            label2 = new Label();
            exit_button = new Button();
            label1 = new Label();
            schema_combo = new ComboBox();
            main_progress_bar = new ProgressBar();
            remove_sp_button = new Button();
            beautify_button = new Button();
            SuspendLayout();
            // 
            // databases_label
            // 
            databases_label.AutoSize = true;
            databases_label.Location = new Point(12, 9);
            databases_label.Name = "databases_label";
            databases_label.Size = new Size(60, 15);
            databases_label.TabIndex = 4;
            databases_label.Text = "Databases";
            // 
            // sp_content_input
            // 
            sp_content_input.Location = new Point(227, 56);
            sp_content_input.Name = "sp_content_input";
            sp_content_input.ScrollBars = RichTextBoxScrollBars.Vertical;
            sp_content_input.Size = new Size(1111, 603);
            sp_content_input.TabIndex = 6;
            sp_content_input.Text = "";
            // 
            // save_sp_button
            // 
            save_sp_button.Location = new Point(1101, 694);
            save_sp_button.Name = "save_sp_button";
            save_sp_button.Size = new Size(75, 23);
            save_sp_button.TabIndex = 7;
            save_sp_button.Text = "Deploy";
            save_sp_button.UseVisualStyleBackColor = true;
            save_sp_button.Click += save_sp_button_Click;
            // 
            // databases_checkbox_list
            // 
            databases_checkbox_list.FormattingEnabled = true;
            databases_checkbox_list.Location = new Point(12, 27);
            databases_checkbox_list.Name = "databases_checkbox_list";
            databases_checkbox_list.ScrollAlwaysVisible = true;
            databases_checkbox_list.Size = new Size(209, 634);
            databases_checkbox_list.TabIndex = 8;
            databases_checkbox_list.ItemCheck += Databases_checkbox_list_ItemCheck;
            databases_checkbox_list.MouseDown += Databases_checkbox_list_MouseDown;
            // 
            // sp_name_input
            // 
            sp_name_input.Location = new Point(536, 27);
            sp_name_input.Name = "sp_name_input";
            sp_name_input.Size = new Size(802, 23);
            sp_name_input.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(458, 30);
            label2.Name = "label2";
            label2.Size = new Size(72, 15);
            label2.TabIndex = 11;
            label2.Text = "Proc. Name:";
            // 
            // exit_button
            // 
            exit_button.Location = new Point(1263, 694);
            exit_button.Name = "exit_button";
            exit_button.Size = new Size(75, 23);
            exit_button.TabIndex = 12;
            exit_button.Text = "Exit";
            exit_button.UseVisualStyleBackColor = true;
            exit_button.Click += exit_button_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(227, 30);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 13;
            label1.Text = "Schema:";
            // 
            // schema_combo
            // 
            schema_combo.FormattingEnabled = true;
            schema_combo.Location = new Point(285, 27);
            schema_combo.Name = "schema_combo";
            schema_combo.Size = new Size(144, 23);
            schema_combo.TabIndex = 14;
            // 
            // main_progress_bar
            // 
            main_progress_bar.Location = new Point(12, 665);
            main_progress_bar.Name = "main_progress_bar";
            main_progress_bar.Size = new Size(1326, 23);
            main_progress_bar.TabIndex = 15;
            // 
            // remove_sp_button
            // 
            remove_sp_button.Location = new Point(1182, 694);
            remove_sp_button.Name = "remove_sp_button";
            remove_sp_button.Size = new Size(75, 23);
            remove_sp_button.TabIndex = 16;
            remove_sp_button.Text = "Remove";
            remove_sp_button.UseVisualStyleBackColor = true;
            // 
            // beautify_button
            // 
            beautify_button.Location = new Point(227, 694);
            beautify_button.Name = "beautify_button";
            beautify_button.Size = new Size(121, 23);
            beautify_button.TabIndex = 17;
            beautify_button.Text = "Beautify (Ctrl + S)";
            beautify_button.UseVisualStyleBackColor = true;
            beautify_button.Click += beautify_button_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1350, 729);
            Controls.Add(beautify_button);
            Controls.Add(remove_sp_button);
            Controls.Add(main_progress_bar);
            Controls.Add(schema_combo);
            Controls.Add(label1);
            Controls.Add(exit_button);
            Controls.Add(label2);
            Controls.Add(sp_name_input);
            Controls.Add(databases_checkbox_list);
            Controls.Add(save_sp_button);
            Controls.Add(sp_content_input);
            Controls.Add(databases_label);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "GuardTek SP Manager";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label databases_label;
        private RichTextBox sp_content_input;
        private Button save_sp_button;
        private CheckedListBox databases_checkbox_list;
        private TextBox sp_name_input;
        private Label label2;
        private Button exit_button;
        private Label label1;
        private ComboBox schema_combo;
        private ProgressBar main_progress_bar;
        private Button remove_sp_button;
        private Button beautify_button;
    }
}