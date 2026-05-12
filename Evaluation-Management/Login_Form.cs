using EvaluationInfrastructure.Repositories;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Evaluation_Management
{
    public partial class Login_Form : Form
    {
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();
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
            string username = txtUsernameLogin.Text.Trim();
            string password = txtPasswordLogin.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your username and password.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var employee = _employeeRepo.Login(username, password);

            if (employee == null)
            {
                MessageBox.Show("Invalid username or password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPasswordLogin.Clear();
                return;
            }

            // Route to the correct dashboard based on role
            if (employee.IsManager)
            {
                ManagerPage managerPage = new ManagerPage(employee);
                managerPage.Show();
            }
            else
            {
                StaffPage staffPage = new StaffPage(employee);
                staffPage.Show();
            }

            this.Hide();
        }
        private void CheckBoxRegisterShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPasswordLogin.PasswordChar = CheckBoxRegisterShowPass.Checked ? '\0' : '•';
        }

        private void lblGoToSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
            this.Hide();

        }

        private void Login_Form_Load(object sender, EventArgs e)
        {
            txtPasswordLogin.PasswordChar = '*';
        }
    }
}
