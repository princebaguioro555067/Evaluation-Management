namespace Evaluation_Management
{
    partial class Login_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login_Form));
            panel1 = new Panel();
            panel7 = new Panel();
            panel6 = new Panel();
            lblLoginClose = new Label();
            lblGoToSignUp = new LinkLabel();
            label5 = new Label();
            label4 = new Label();
            CheckBoxRegisterShowPass = new CheckBox();
            panel3 = new Panel();
            label2 = new Label();
            txtUsernameLogin = new TextBox();
            panel4 = new Panel();
            label1 = new Label();
            txtPasswordLogin = new TextBox();
            btnLogin = new Button();
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            panel7.SuspendLayout();
            panel6.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(panel7);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(715, 435);
            panel1.TabIndex = 0;
            // 
            // panel7
            // 
            panel7.BackColor = Color.Crimson;
            panel7.Controls.Add(panel6);
            panel7.Controls.Add(pictureBox2);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 0);
            panel7.Name = "panel7";
            panel7.Size = new Size(715, 435);
            panel7.TabIndex = 2;
            // 
            // panel6
            // 
            panel6.BackColor = Color.LavenderBlush;
            panel6.Controls.Add(lblLoginClose);
            panel6.Controls.Add(lblGoToSignUp);
            panel6.Controls.Add(label5);
            panel6.Controls.Add(label4);
            panel6.Controls.Add(CheckBoxRegisterShowPass);
            panel6.Controls.Add(panel3);
            panel6.Controls.Add(panel4);
            panel6.Controls.Add(btnLogin);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(310, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(405, 435);
            panel6.TabIndex = 13;
            // 
            // lblLoginClose
            // 
            lblLoginClose.AutoSize = true;
            lblLoginClose.BackColor = Color.Transparent;
            lblLoginClose.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLoginClose.Location = new Point(373, 9);
            lblLoginClose.Name = "lblLoginClose";
            lblLoginClose.Size = new Size(20, 20);
            lblLoginClose.TabIndex = 12;
            lblLoginClose.Text = "X";
            lblLoginClose.Click += lblLoginClose_Click;
            // 
            // lblGoToSignUp
            // 
            lblGoToSignUp.AutoSize = true;
            lblGoToSignUp.LinkColor = SystemColors.WindowFrame;
            lblGoToSignUp.Location = new Point(53, 263);
            lblGoToSignUp.Name = "lblGoToSignUp";
            lblGoToSignUp.Size = new Size(75, 15);
            lblGoToSignUp.TabIndex = 17;
            lblGoToSignUp.TabStop = true;
            lblGoToSignUp.Text = "Sign up Here";
            lblGoToSignUp.LinkClicked += lblGoToSignUp_LinkClicked;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(53, 77);
            label5.Name = "label5";
            label5.Size = new Size(98, 23);
            label5.TabIndex = 16;
            label5.Text = "TITLE HERE";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(53, 109);
            label4.Name = "label4";
            label4.Size = new Size(171, 20);
            label4.TabIndex = 15;
            label4.Text = "Login to your Account";
            // 
            // CheckBoxRegisterShowPass
            // 
            CheckBoxRegisterShowPass.AutoSize = true;
            CheckBoxRegisterShowPass.BackColor = Color.Transparent;
            CheckBoxRegisterShowPass.Font = new Font("Microsoft JhengHei UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CheckBoxRegisterShowPass.Location = new Point(228, 261);
            CheckBoxRegisterShowPass.Name = "CheckBoxRegisterShowPass";
            CheckBoxRegisterShowPass.Size = new Size(121, 21);
            CheckBoxRegisterShowPass.TabIndex = 7;
            CheckBoxRegisterShowPass.Text = "Show password";
            CheckBoxRegisterShowPass.UseVisualStyleBackColor = false;
            CheckBoxRegisterShowPass.CheckedChanged += CheckBoxRegisterShowPass_CheckedChanged;
            // 
            // panel3
            // 
            panel3.BackColor = Color.LavenderBlush;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(label2);
            panel3.Controls.Add(txtUsernameLogin);
            panel3.Location = new Point(53, 155);
            panel3.Name = "panel3";
            panel3.Size = new Size(297, 42);
            panel3.TabIndex = 13;
            // 
            // label2
            // 
            label2.BackColor = Color.LavenderBlush;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.WindowFrame;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(295, 17);
            label2.TabIndex = 2;
            label2.Text = "Username:";
            // 
            // txtUsernameLogin
            // 
            txtUsernameLogin.BackColor = Color.LavenderBlush;
            txtUsernameLogin.BorderStyle = BorderStyle.None;
            txtUsernameLogin.ForeColor = SystemColors.ControlText;
            txtUsernameLogin.Location = new Point(8, 20);
            txtUsernameLogin.Name = "txtUsernameLogin";
            txtUsernameLogin.Size = new Size(280, 16);
            txtUsernameLogin.TabIndex = 12;
            // 
            // panel4
            // 
            panel4.BackColor = Color.LavenderBlush;
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(label1);
            panel4.Controls.Add(txtPasswordLogin);
            panel4.Location = new Point(53, 213);
            panel4.Name = "panel4";
            panel4.Size = new Size(297, 42);
            panel4.TabIndex = 14;
            // 
            // label1
            // 
            label1.BackColor = Color.LavenderBlush;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.WindowFrame;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(295, 17);
            label1.TabIndex = 2;
            label1.Text = "Password:";
            // 
            // txtPasswordLogin
            // 
            txtPasswordLogin.BackColor = Color.LavenderBlush;
            txtPasswordLogin.BorderStyle = BorderStyle.None;
            txtPasswordLogin.ForeColor = SystemColors.ControlText;
            txtPasswordLogin.Location = new Point(8, 20);
            txtPasswordLogin.Name = "txtPasswordLogin";
            txtPasswordLogin.Size = new Size(280, 16);
            txtPasswordLogin.TabIndex = 12;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.Crimson;
            btnLogin.FlatAppearance.BorderColor = Color.White;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = SystemColors.ButtonHighlight;
            btnLogin.Location = new Point(54, 306);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(297, 34);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "LOGIN";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Left;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(310, 435);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // Login_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(715, 435);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Login_Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Login_Form_Load;
            panel1.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel7;
        private Label lblLoginClose;
        private Panel panel6;
        private LinkLabel lblGoToSignUp;
        private Label label5;
        private Label label4;
        private CheckBox CheckBoxRegisterShowPass;
        private Panel panel3;
        private Label label2;
        private TextBox txtUsernameLogin;
        private Panel panel4;
        private Label label1;
        private TextBox txtPasswordLogin;
        private Button btnLogin;
        private PictureBox pictureBox2;
    }
}
