using EvaluationDomain.Models;
using EvaluationInfrastructure.Repositories;

namespace Evaluation_Management
{
    public partial class RegisterForm : Form
    {
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();
        private List<Employee> _supervisors = new List<Employee>();

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            txtPasswordRegister.PasswordChar = '*';
            LoadTeams();
        }

        private void LoadTeams()
        {
            _supervisors = _employeeRepo.GetByRole(EmployeeRole.Supervisor);

            cmbTeamNumber.Items.Clear();
            cmbTeamNumber.Items.Add("-- Select your Team --");

            for (int i = 1; i <= 5; i++)
            {
                var supervisor = _supervisors.FirstOrDefault(s => s.GroupNumber == i);
                string display = supervisor != null
                    ? $"Team {i}  —  Supervisor: {supervisor.Name}"
                    : $"Team {i}  —  No Supervisor Yet";

                cmbTeamNumber.Items.Add(display);
            }

            cmbTeamNumber.SelectedIndex = 0;
        }

        private void lblRegisterClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtUsernameRegister.Text.Trim();
            string password = txtPasswordRegister.Text;

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

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Sign Up",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbTeamNumber.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select your team.", "Sign Up",
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
                int groupNumber = cmbTeamNumber.SelectedIndex; // index 1 = Team 1, etc.

                var supervisor = _supervisors.FirstOrDefault(s => s.GroupNumber == groupNumber);

                var newEmployee = new Employee
                {
                    Username = username,
                    Password = password,       // hashed inside repo.Add()
                    Name = username,
                    Designation = "Staff",
                    Role = EmployeeRole.Staff,
                    GroupNumber = groupNumber,
                    SupervisorId = supervisor?.Id
                };

                _employeeRepo.Add(newEmployee);

                string teamInfo = supervisor != null
                    ? $"You have been assigned to Team {groupNumber} under {supervisor.Name}."
                    : $"You have been assigned to Team {groupNumber}. A supervisor will be assigned later.";

                MessageBox.Show(
                    $"Account created successfully!\n\n{teamInfo}\n\nYou can now log in.",
                    "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login_Form loginForm = new Login_Form();
            loginForm.Show();
            this.Hide();
        }
    }
}