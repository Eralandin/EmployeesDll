﻿namespace Employees
{
    partial class AddForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddForm));
            TopPanel = new Panel();
            FormNameLabel = new Label();
            MainLabel = new Label();
            BottomPanel = new Panel();
            CreateBtn = new Button();
            CancelBtn = new Button();
            UsernameLabel = new Label();
            UsernameTextBox = new TextBox();
            PasswordLabel = new Label();
            PasswordTextBox = new TextBox();
            PasswordCheckLabel = new Label();
            PasswordCheckTextBox = new TextBox();
            label1 = new Label();
            TreeView = new TreeView();
            RoleLabel = new Label();
            RoleTextBox = new TextBox();
            AdminCheck = new CheckBox();
            ChangePasswordBtn = new Button();
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
            TopPanel.Size = new Size(1076, 80);
            TopPanel.TabIndex = 0;
            // 
            // FormNameLabel
            // 
            FormNameLabel.AutoSize = true;
            FormNameLabel.Font = new Font("Bahnschrift", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            FormNameLabel.ForeColor = SystemColors.ControlLightLight;
            FormNameLabel.Location = new Point(12, 40);
            FormNameLabel.Name = "FormNameLabel";
            FormNameLabel.Size = new Size(237, 25);
            FormNameLabel.TabIndex = 2;
            FormNameLabel.Text = "Работа с сотрудниками";
            // 
            // MainLabel
            // 
            MainLabel.AutoSize = true;
            MainLabel.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            MainLabel.ForeColor = SystemColors.ControlLightLight;
            MainLabel.Location = new Point(12, 7);
            MainLabel.Name = "MainLabel";
            MainLabel.Size = new Size(418, 33);
            MainLabel.TabIndex = 1;
            MainLabel.Text = "Торгово-посредническая фирма";
            // 
            // BottomPanel
            // 
            BottomPanel.BackColor = Color.ForestGreen;
            BottomPanel.Dock = DockStyle.Bottom;
            BottomPanel.Location = new Point(0, 550);
            BottomPanel.Name = "BottomPanel";
            BottomPanel.Size = new Size(1076, 46);
            BottomPanel.TabIndex = 1;
            // 
            // CreateBtn
            // 
            CreateBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            CreateBtn.BackColor = Color.ForestGreen;
            CreateBtn.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            CreateBtn.ForeColor = SystemColors.ControlLightLight;
            CreateBtn.Location = new Point(10, 494);
            CreateBtn.Name = "CreateBtn";
            CreateBtn.Size = new Size(130, 50);
            CreateBtn.TabIndex = 2;
            CreateBtn.Text = "Создать";
            CreateBtn.UseVisualStyleBackColor = false;
            CreateBtn.Click += CreateBtn_Click_1;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CancelBtn.BackColor = Color.ForestGreen;
            CancelBtn.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            CancelBtn.ForeColor = SystemColors.ControlLightLight;
            CancelBtn.Location = new Point(934, 494);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(130, 50);
            CancelBtn.TabIndex = 3;
            CancelBtn.Text = "Отмена";
            CancelBtn.UseVisualStyleBackColor = false;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // UsernameLabel
            // 
            UsernameLabel.AutoSize = true;
            UsernameLabel.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            UsernameLabel.ForeColor = SystemColors.ActiveCaptionText;
            UsernameLabel.Location = new Point(10, 92);
            UsernameLabel.Name = "UsernameLabel";
            UsernameLabel.Size = new Size(250, 33);
            UsernameLabel.TabIndex = 3;
            UsernameLabel.Text = "Имя пользователя:";
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            UsernameTextBox.Location = new Point(266, 89);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.Size = new Size(300, 40);
            UsernameTextBox.TabIndex = 4;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PasswordLabel.ForeColor = SystemColors.ActiveCaptionText;
            PasswordLabel.Location = new Point(12, 152);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(110, 33);
            PasswordLabel.TabIndex = 5;
            PasswordLabel.Text = "Пароль:";
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PasswordTextBox.Location = new Point(128, 149);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(438, 40);
            PasswordTextBox.TabIndex = 6;
            // 
            // PasswordCheckLabel
            // 
            PasswordCheckLabel.AutoSize = true;
            PasswordCheckLabel.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PasswordCheckLabel.ForeColor = SystemColors.ActiveCaptionText;
            PasswordCheckLabel.Location = new Point(12, 213);
            PasswordCheckLabel.Name = "PasswordCheckLabel";
            PasswordCheckLabel.Size = new Size(278, 33);
            PasswordCheckLabel.TabIndex = 7;
            PasswordCheckLabel.Text = "Подтвердите пароль:";
            // 
            // PasswordCheckTextBox
            // 
            PasswordCheckTextBox.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PasswordCheckTextBox.Location = new Point(296, 210);
            PasswordCheckTextBox.Name = "PasswordCheckTextBox";
            PasswordCheckTextBox.Size = new Size(270, 40);
            PasswordCheckTextBox.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(572, 92);
            label1.Name = "label1";
            label1.Size = new Size(221, 33);
            label1.TabIndex = 9;
            label1.Text = "Уровни доступа:";
            // 
            // TreeView
            // 
            TreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TreeView.CheckBoxes = true;
            TreeView.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            TreeView.Location = new Point(572, 128);
            TreeView.Name = "TreeView";
            TreeView.Size = new Size(492, 309);
            TreeView.TabIndex = 10;
            // 
            // RoleLabel
            // 
            RoleLabel.AutoSize = true;
            RoleLabel.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            RoleLabel.ForeColor = SystemColors.ActiveCaptionText;
            RoleLabel.Location = new Point(12, 271);
            RoleLabel.Name = "RoleLabel";
            RoleLabel.Size = new Size(188, 33);
            RoleLabel.TabIndex = 11;
            RoleLabel.Text = "Введите роль:";
            // 
            // RoleTextBox
            // 
            RoleTextBox.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            RoleTextBox.Location = new Point(199, 268);
            RoleTextBox.Name = "RoleTextBox";
            RoleTextBox.Size = new Size(367, 40);
            RoleTextBox.TabIndex = 12;
            // 
            // AdminCheck
            // 
            AdminCheck.AutoSize = true;
            AdminCheck.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            AdminCheck.Location = new Point(12, 324);
            AdminCheck.Name = "AdminCheck";
            AdminCheck.RightToLeft = RightToLeft.Yes;
            AdminCheck.Size = new Size(322, 37);
            AdminCheck.TabIndex = 13;
            AdminCheck.Text = "Права администратора";
            AdminCheck.UseVisualStyleBackColor = true;
            AdminCheck.Visible = false;
            // 
            // ChangePasswordBtn
            // 
            ChangePasswordBtn.Font = new Font("Bahnschrift", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            ChangePasswordBtn.Location = new Point(12, 367);
            ChangePasswordBtn.Name = "ChangePasswordBtn";
            ChangePasswordBtn.Size = new Size(260, 50);
            ChangePasswordBtn.TabIndex = 14;
            ChangePasswordBtn.Text = "Сменить пароль";
            ChangePasswordBtn.UseVisualStyleBackColor = true;
            ChangePasswordBtn.Visible = false;
            ChangePasswordBtn.Click += ChangePasswordBtn_Click;
            // 
            // AddForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1076, 596);
            Controls.Add(ChangePasswordBtn);
            Controls.Add(AdminCheck);
            Controls.Add(RoleTextBox);
            Controls.Add(RoleLabel);
            Controls.Add(TreeView);
            Controls.Add(label1);
            Controls.Add(PasswordCheckTextBox);
            Controls.Add(PasswordCheckLabel);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(UsernameTextBox);
            Controls.Add(UsernameLabel);
            Controls.Add(CancelBtn);
            Controls.Add(CreateBtn);
            Controls.Add(BottomPanel);
            Controls.Add(TopPanel);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1092, 635);
            Name = "AddForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Добавление сотрудника";
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel TopPanel;
        private Label MainLabel;
        private Label FormNameLabel;
        private Panel BottomPanel;
        private Label UsernameLabel;
        private Label RoleLabel;
        private Button ChangePasswordBtn;
        public Button CreateBtn;
        public Button CancelBtn;
        public TextBox UsernameTextBox;
        public TextBox PasswordTextBox;
        public TextBox PasswordCheckTextBox;
        public Label label1;
        public TreeView TreeView;
        public TextBox RoleTextBox;
        public CheckBox AdminCheck;
        public Label PasswordLabel;
        public Label PasswordCheckLabel;
    }
}
