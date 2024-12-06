namespace Employees
{
    partial class ChangePasswordForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TopPanel = new Panel();
            FormNameLabel = new Label();
            MainLabel = new Label();
            CancelBtn = new Button();
            CreateBtn = new Button();
            PasswordCheckTextBox = new TextBox();
            PasswordCheckLabel = new Label();
            PasswordTextBox = new TextBox();
            PasswordLabel = new Label();
            TopPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TopPanel
            // 
            TopPanel.BackColor = Color.ForestGreen;
            TopPanel.Controls.Add(FormNameLabel);
            TopPanel.Controls.Add(MainLabel);
            TopPanel.Dock = DockStyle.Top;
            TopPanel.Location = new Point(0, 0);
            TopPanel.Name = "TopPanel";
            TopPanel.Size = new Size(604, 80);
            TopPanel.TabIndex = 0;
            // 
            // FormNameLabel
            // 
            FormNameLabel.AutoSize = true;
            FormNameLabel.Font = new Font("Bahnschrift", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            FormNameLabel.ForeColor = SystemColors.ControlLightLight;
            FormNameLabel.Location = new Point(12, 40);
            FormNameLabel.Name = "FormNameLabel";
            FormNameLabel.Size = new Size(145, 25);
            FormNameLabel.TabIndex = 4;
            FormNameLabel.Text = "Смена пароля";
            // 
            // MainLabel
            // 
            MainLabel.AutoSize = true;
            MainLabel.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            MainLabel.ForeColor = SystemColors.ControlLightLight;
            MainLabel.Location = new Point(12, 7);
            MainLabel.Name = "MainLabel";
            MainLabel.Size = new Size(418, 33);
            MainLabel.TabIndex = 3;
            MainLabel.Text = "Торгово-посредническая фирма";
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = AnchorStyles.Bottom;
            CancelBtn.BackColor = Color.ForestGreen;
            CancelBtn.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            CancelBtn.ForeColor = SystemColors.ControlLightLight;
            CancelBtn.Location = new Point(462, 217);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(130, 50);
            CancelBtn.TabIndex = 5;
            CancelBtn.Text = "Отмена";
            CancelBtn.UseVisualStyleBackColor = false;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // CreateBtn
            // 
            CreateBtn.Anchor = AnchorStyles.Bottom;
            CreateBtn.BackColor = Color.ForestGreen;
            CreateBtn.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            CreateBtn.ForeColor = SystemColors.ControlLightLight;
            CreateBtn.Location = new Point(12, 217);
            CreateBtn.Name = "CreateBtn";
            CreateBtn.Size = new Size(130, 50);
            CreateBtn.TabIndex = 4;
            CreateBtn.Text = "Сменить";
            CreateBtn.UseVisualStyleBackColor = false;
            CreateBtn.Click += CreateBtn_Click;
            // 
            // PasswordCheckTextBox
            // 
            PasswordCheckTextBox.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PasswordCheckTextBox.Location = new Point(312, 156);
            PasswordCheckTextBox.Name = "PasswordCheckTextBox";
            PasswordCheckTextBox.Size = new Size(270, 40);
            PasswordCheckTextBox.TabIndex = 12;
            // 
            // PasswordCheckLabel
            // 
            PasswordCheckLabel.AutoSize = true;
            PasswordCheckLabel.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PasswordCheckLabel.ForeColor = SystemColors.ActiveCaptionText;
            PasswordCheckLabel.Location = new Point(28, 159);
            PasswordCheckLabel.Name = "PasswordCheckLabel";
            PasswordCheckLabel.Size = new Size(278, 33);
            PasswordCheckLabel.TabIndex = 11;
            PasswordCheckLabel.Text = "Подтвердите пароль:";
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PasswordTextBox.Location = new Point(231, 95);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(351, 40);
            PasswordTextBox.TabIndex = 10;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PasswordLabel.ForeColor = SystemColors.ActiveCaptionText;
            PasswordLabel.Location = new Point(28, 98);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(197, 33);
            PasswordLabel.TabIndex = 9;
            PasswordLabel.Text = "Новый пароль:";
            // 
            // ChangePasswordForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(604, 279);
            Controls.Add(PasswordCheckTextBox);
            Controls.Add(PasswordCheckLabel);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(CancelBtn);
            Controls.Add(CreateBtn);
            Controls.Add(TopPanel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ChangePasswordForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Смена пароля";
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel TopPanel;
        private Label FormNameLabel;
        private Label MainLabel;
        private Button CancelBtn;
        private Button CreateBtn;
        private TextBox PasswordCheckTextBox;
        private Label PasswordCheckLabel;
        private Label PasswordLabel;
        public TextBox PasswordTextBox;
    }
}