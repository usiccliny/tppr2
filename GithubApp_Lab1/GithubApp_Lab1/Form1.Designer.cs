namespace GithubApp_Lab1
{
    partial class Form1
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
            authButton = new Button();
            listRepositories = new ListBox();
            btnGetRepos = new Button();
            txtRepoName = new TextBox();
            label1 = new Label();
            btnCreateRepos = new Button();
            lblStatus = new Label();
            btnDeleteRepo = new Button();
            lblStatusUpd = new Label();
            btnUpdateRepo = new Button();
            label3 = new Label();
            txtUpdate = new TextBox();
            SuspendLayout();
            // 
            // authButton
            // 
            authButton.Location = new Point(12, 382);
            authButton.Name = "authButton";
            authButton.Size = new Size(91, 56);
            authButton.TabIndex = 1;
            authButton.Text = "Авторизация";
            authButton.UseVisualStyleBackColor = true;
            // 
            // listRepositories
            // 
            listRepositories.FormattingEnabled = true;
            listRepositories.ItemHeight = 15;
            listRepositories.Location = new Point(0, 0);
            listRepositories.Name = "listRepositories";
            listRepositories.Size = new Size(209, 349);
            listRepositories.TabIndex = 2;
            // 
            // btnGetRepos
            // 
            btnGetRepos.Location = new Point(120, 382);
            btnGetRepos.Name = "btnGetRepos";
            btnGetRepos.Size = new Size(89, 56);
            btnGetRepos.TabIndex = 3;
            btnGetRepos.Text = "Загрузить репозитории";
            btnGetRepos.UseVisualStyleBackColor = true;
            btnGetRepos.Click += btnGetRepos_Click;
            // 
            // txtRepoName
            // 
            txtRepoName.Location = new Point(470, 12);
            txtRepoName.Name = "txtRepoName";
            txtRepoName.Size = new Size(100, 23);
            txtRepoName.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(315, 15);
            label1.Name = "label1";
            label1.Size = new Size(149, 15);
            label1.TabIndex = 5;
            label1.Text = "Введите имя репозитория";
            // 
            // btnCreateRepos
            // 
            btnCreateRepos.Location = new Point(445, 41);
            btnCreateRepos.Name = "btnCreateRepos";
            btnCreateRepos.Size = new Size(149, 31);
            btnCreateRepos.TabIndex = 6;
            btnCreateRepos.Text = "Создать репозиторий";
            btnCreateRepos.UseVisualStyleBackColor = true;
            btnCreateRepos.Click += btnCreateRepos_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(445, 84);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(38, 15);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "label2";
            lblStatus.Visible = false;
            // 
            // btnDeleteRepo
            // 
            btnDeleteRepo.Location = new Point(225, 382);
            btnDeleteRepo.Name = "btnDeleteRepo";
            btnDeleteRepo.Size = new Size(91, 56);
            btnDeleteRepo.TabIndex = 8;
            btnDeleteRepo.Text = "Удалить выбранный репозиторий";
            btnDeleteRepo.UseVisualStyleBackColor = true;
            btnDeleteRepo.Click += btnDeleteRepo_Click;
            // 
            // lblStatusUpd
            // 
            lblStatusUpd.AutoSize = true;
            lblStatusUpd.Location = new Point(445, 226);
            lblStatusUpd.Name = "lblStatusUpd";
            lblStatusUpd.Size = new Size(38, 15);
            lblStatusUpd.TabIndex = 12;
            lblStatusUpd.Text = "label2";
            lblStatusUpd.Visible = false;
            // 
            // btnUpdateRepo
            // 
            btnUpdateRepo.Location = new Point(445, 183);
            btnUpdateRepo.Name = "btnUpdateRepo";
            btnUpdateRepo.Size = new Size(149, 31);
            btnUpdateRepo.TabIndex = 11;
            btnUpdateRepo.Text = "Обновить репозиторий";
            btnUpdateRepo.UseVisualStyleBackColor = true;
            btnUpdateRepo.Click += btnUpdateRepo_Click;
            // 
            // label3
            // 
            label3.Location = new Point(315, 143);
            label3.Name = "label3";
            label3.Size = new Size(149, 37);
            label3.TabIndex = 10;
            label3.Text = "Введите новое имя репозитория";
            // 
            // txtUpdate
            // 
            txtUpdate.Location = new Point(470, 154);
            txtUpdate.Name = "txtUpdate";
            txtUpdate.Size = new Size(100, 23);
            txtUpdate.TabIndex = 9;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblStatusUpd);
            Controls.Add(btnUpdateRepo);
            Controls.Add(label3);
            Controls.Add(txtUpdate);
            Controls.Add(btnDeleteRepo);
            Controls.Add(lblStatus);
            Controls.Add(btnCreateRepos);
            Controls.Add(label1);
            Controls.Add(txtRepoName);
            Controls.Add(btnGetRepos);
            Controls.Add(listRepositories);
            Controls.Add(authButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button authButton;
        private ListBox listRepositories;
        private Button btnGetRepos;
        private TextBox txtRepoName;
        private Label label1;
        private Button btnCreateRepos;
        private Label lblStatus;
        private Button btnDeleteRepo;
        private Label lblStatusUpd;
        private Button btnUpdateRepo;
        private Label label3;
        private TextBox txtUpdate;
    }
}
