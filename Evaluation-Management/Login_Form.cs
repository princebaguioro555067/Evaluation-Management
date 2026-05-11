namespace Evaluation_Management
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }


        private void lblLoginClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            StaffPage Form = new StaffPage();
            Form.Show();
            this.Hide();
        }

        private void lblGoToSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
            RegisterForm Form = new RegisterForm();
            Form.Show();
            this.Hide();
            
        }
    }
}
