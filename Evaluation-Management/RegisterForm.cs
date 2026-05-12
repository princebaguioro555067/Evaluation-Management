using EvaluationDomain.Models;
using EvaluationInfrastructure.Repositories;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Evaluation_Management
{
    public partial class RegisterForm : Form
    {
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();

        public RegisterForm()
        {
            InitializeComponent();
        }


        private void lblRegisterClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtUsernameRegister.Text.Trim();
            string password = txtPasswordRegister.Text;
            string confirmPassword = txtConfirmPasswordRegister.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username is required.", "Sign Up",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password is required.", "Sign Up",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Sign Up",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPasswordRegister.Clear();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Sign Up",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_employeeRepo.UsernameExists(username))
            {
                MessageBox.Show($"Username '{username}' is already taken.", "Sign Up",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // New accounts are Staff by default.
                // A Supervisor or AAM can update the role later.
                var newEmployee = new Employee
                {
                    Username = username,
                    Password = password,
                    Name = username,
                    Designation = "Staff",
                    Role = EmployeeRole.Staff,
                    GroupNumber = 0
                };

                _employeeRepo.Add(newEmployee);

                MessageBox.Show("Account created successfully! You can now log in.", "Sign Up",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Login_Form loginForm = new Login_Form();
                loginForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating account: {ex.Message}", "Sign Up",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckBoxRegisterShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPasswordRegister.PasswordChar = CheckBoxRegisterShowPass.Checked ? '\0' : '*';
            txtConfirmPasswordRegister.PasswordChar = CheckBoxRegisterShowPass.Checked ? '\0' : '*';
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login_Form loginForm = new Login_Form();
            loginForm.Show();
            this.Hide();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            txtPasswordRegister.PasswordChar = '*';
            txtConfirmPasswordRegister.PasswordChar = '*';
        }
    }
}
