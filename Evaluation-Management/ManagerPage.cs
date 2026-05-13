using EvaluationDomain.Models;
using EvaluationInfrastructure.Repositories;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Data;

namespace Evaluation_Management
{
    public partial class ManagerPage : MaterialForm
    {
        private readonly Employee _loggedInEmployee;
        private readonly KpmScoreRepository _kpmRepo = new KpmScoreRepository();
        private readonly EvaluationPeriodRepository _periodRepo = new EvaluationPeriodRepository();
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();

        public ManagerPage(Employee employee)
        {
            InitializeComponent();
            _loggedInEmployee = employee;
            Theme();
            BackColorAdjustments();
            ForeColorAdjustments();
        }

        private void ManagerPage_Load(object sender, EventArgs e)
        {

            LoadManagerInfo();
            LoadManagerStats();
        }

        private void LoadManagerInfo()
        {
            lblEpt.Text = "Employee Performance Tracker";
            lblEpt2.Text = "Employee Performance Tracker";
            lblEpt3.Text = "Employee Performance Tracker";
            lblManagerName.Text = _loggedInEmployee.Name;
            lblManagerName2.Text = _loggedInEmployee.Name;
        }

        private void LoadManagerStats()
        {
            var now = DateTime.Now;
            var period = _periodRepo.GetByMonthYear(now.Month, now.Year);
        }


        private void Theme()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Define your exact Crimson
            Color myCrimson = Color.FromArgb(220, 20, 60);

            materialSkinManager.ColorScheme = new ColorScheme(
                myCrimson,         // Primary (Title bar)
                myCrimson,         // Primary Dark (Status bar)
                myCrimson,         // Primary Light
                myCrimson,         // Accent (This makes the buttons and checkboxes Red)
                TextShade.WHITE    // Text color on the red bars
            );
        }
        private void BackColorAdjustments()
        {
            panel1.BackColor = Color.White;
            panel8.BackColor = Color.White;
            panel5.BackColor = Color.White;
            panel6.BackColor = Color.White;
            panel3.BackColor = Color.White;
            panel9.BackColor = Color.White;


            materialLabel1.BackColor = Color.White;
            lblEpt2.BackColor = Color.White;

            materialLabel11.BackColor = Color.White;

            lblEvaluationResult.BackColor = Color.White;

            lblEmployeeName.BackColor = Color.White;

            lblEPercentage.BackColor = Color.White;
            lblEvaluationlvl.BackColor = Color.White;

            lblEpt.BackColor = Color.White;
            materialLabel9.BackColor = Color.White;

            materialLabel27.BackColor = Color.White;
            materialLabel28.BackColor = Color.White;
            lblEpt3.BackColor = Color.White;
            materialLabel19.BackColor = Color.White;

            materialLabel35.BackColor = Color.White;
            materialLabel34.BackColor = Color.White;
            materialLabel29.BackColor = Color.White;
            materialLabel33.BackColor = Color.White;
            materialLabel30.BackColor = Color.White;

            materialLabel31.BackColor = Color.White;
            materialLabel39.BackColor = Color.White;

            materialLabel37.BackColor = Color.White;
            materialLabel38.BackColor = Color.White;

            materialLabel41.BackColor = Color.White;
            materialLabel32.BackColor = Color.White;
            materialLabel40.BackColor = Color.White;

            lblLastNameCNM.BackColor = Color.White;
            lblFirstNameCNM.BackColor = Color.White;
            lblUsernameCNM.BackColor = Color.White;

            materialLabel49.BackColor = Color.White;
            materialLabel50.BackColor = Color.White;

            lblPasswordCNM.BackColor = Color.White;
            lblTeamCNM.BackColor = Color.White;




        }
        private void ForeColorAdjustments()
        {
            Color myCrimson = Color.FromArgb(220, 20, 60);
            Color myDrakGray = Color.FromArgb(64, 64, 64);

            lblEpt2.ForeColor = myCrimson;
            materialLabel1.ForeColor = Color.Gray;

            materialLabel25.ForeColor = Color.Gray;
            lblManagerName2.ForeColor = Color.Gray;
            materialLabel12.ForeColor = Color.Gray;
            materialLabel23.ForeColor = Color.Gray;
            materialLabel28.ForeColor = myDrakGray;

            lblEPercentage.ForeColor = myCrimson;

            lblEvaluationlvl.ForeColor = myDrakGray;
            lblEvaluationResult.ForeColor = myCrimson;

            lblEpt.ForeColor = myCrimson;

            materialLabel9.ForeColor = Color.Gray;
            materialLabel8.ForeColor = Color.Gray;

            lblManagerName.ForeColor = Color.Gray;
            materialLabel15.ForeColor = Color.Gray;
            materialLabel16.ForeColor = Color.Gray;
            materialLabel17.ForeColor = Color.Gray;

            lblEpt3.ForeColor = myCrimson;
            materialLabel19.ForeColor = Color.Gray;

            materialLabel34.ForeColor = myDrakGray;
            materialLabel29.ForeColor = myDrakGray;
            materialLabel33.ForeColor = myDrakGray;
            materialLabel30.ForeColor = myDrakGray;

            materialLabel37.ForeColor = myDrakGray;
            materialLabel38.ForeColor = myDrakGray;

            materialLabel41.ForeColor = myDrakGray;
            materialLabel32.ForeColor = myDrakGray;
            materialLabel40.ForeColor = myDrakGray;

            materialLabel49.ForeColor = myDrakGray;
            materialLabel50.ForeColor = myDrakGray;
        }

        private void btnCreateAccountCNM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsernameMD.Text) ||
                 txtPasswordCNM.Text != txtConfirmPasswordCNM.Text)
            {
                MessageBox.Show("Please ensure all fields are filled and passwords match.", "Validation Error");
                return;
            }

            try
            {
                // 2. Create the Employee object
                var newManager = new Employee
                {
                    Username = txtUsernameMD.Text.Trim(),
                    Password = txtPasswordCNM.Text, // Repository usually handles hashing
                    Name = $"{txtFirstNameMD.Text.Trim()} {txtLastNameMD.Text.Trim()}",
                    Designation = "Manager",
                    Role = EmployeeRole.Supervisor,
                    GroupNumber = cmbTeamCNM.SelectedIndex + 1 // Index 0 = Team 1
                };

                // 3. Save to Database
                _employeeRepo.Add(newManager);

                // 4. Update the "Tuhod" Summary Labels
                lblUsernameCNM.Text = newManager.Username;
                lblFirstNameCNM.Text = txtFirstNameMD.Text.Trim();
                lblLastNameCNM.Text = txtLastNameMD.Text.Trim();
                lblPasswordCNM.Text = "********"; // Security: show masked or use txtPasswordCNM.Text
                lblTeamCNM.Text = $"Team {newManager.GroupNumber}";

                MessageBox.Show("Manager account created and assigned successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating account: {ex.Message}");
            }
        }

        private void btnClearCNM_Click(object sender, EventArgs e)
        {
            txtUsernameMD.Clear();
            txtFirstNameMD.Clear();
            txtLastNameMD.Clear();
            txtPasswordCNM.Clear();
            txtConfirmPasswordCNM.Clear();
            cmbTeamCNM.SelectedIndex = -1;

            // Reset Summary Labels to placeholder state
            lblUsernameCNM.Text = "Username";
            lblFirstNameCNM.Text = "First Name";
            lblLastNameCNM.Text = "Last Name";
            lblPasswordCNM.Text = "Password";
            lblTeamCNM.Text = "Team";
        }
    }
}
