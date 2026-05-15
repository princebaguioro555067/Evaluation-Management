using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using EvaluationInfrastructure.Repositories;
using MaterialSkin;
using MaterialSkin.Controls;

namespace Evaluation_Management
{
    public partial class ManagerPage : MaterialForm
    {
        private readonly Employee _loggedInEmployee;
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();
        private readonly EvaluationSubmissionRepository _repo = new EvaluationSubmissionRepository();

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
            LoadTeamRequests();
        }

        // ---------------------------------------------------------------
        // Loads pending submissions for this manager's group
        // ---------------------------------------------------------------
        private void LoadTeamRequests()
        {
            var now = DateTime.Now;

            List<EvaluationSubmission> submissions;

            if (_loggedInEmployee.Role == EmployeeRole.AAM)
                submissions = _repo.GetAllForPeriod(now.Month, now.Year);
            else
                submissions = _repo.GetForGroup(_loggedInEmployee.GroupNumber, now.Month, now.Year);

            // Build a display list for the DataGridView
            var displayList = submissions
                .Where(s => s.Status == SubmissionStatus.Pending)
                .Select(s => new
                {
                    s.Id,
                    StaffName = s.Employee?.Name ?? "Unknown",
                    Date = s.SubmissionDate,
                    Period = s.PeriodDisplay,
                    Score = $"{s.Score:F0}",
                    Level = s.EvaluationLevel,
                    Timeliness = s.TimelinessDisplay,
                    s.Comment,
                    s.Status
                })
                .ToList();

            dgvApprovals.DataSource = displayList;

            // Hide columns not needed in the grid
            if (dgvApprovals.Columns["Id"] != null)
                dgvApprovals.Columns["Id"].Visible = false;
            if (dgvApprovals.Columns["Comment"] != null)
                dgvApprovals.Columns["Comment"].Visible = false;
            if (dgvApprovals.Columns["Status"] != null)
                dgvApprovals.Columns["Status"].Visible = false;

            if (displayList.Count == 0)
            {
                dgvApprovals.DataSource = null;
                ClearDetails();
            }
        }

        private void LoadManagerInfo()
        {
            lblEpt.Text = "Employee Performance Tracker";
            lblEpt2.Text = "Employee Performance Tracker";
            lblEpt3.Text = "Employee Performance Tracker";
            lblManagerName.Text = _loggedInEmployee.Name;
            lblManagerName2.Text = _loggedInEmployee.Name;

            cmbTeamCNM.Items.Clear();
            for (int i = 1; i <= 5; i++)
                cmbTeamCNM.Items.Add($"Team {i}");

            if (cmbTeamCNM.Items.Count > 0)
                cmbTeamCNM.SelectedIndex = 0;
        }

        private void LoadManagerStats()
        {
            var now = DateTime.Now;

            List<EvaluationSubmission> allSubmissions;

            if (_loggedInEmployee.Role == EmployeeRole.AAM)
                allSubmissions = _repo.GetAllForPeriod(now.Month, now.Year);
            else
                allSubmissions = _repo.GetForGroup(_loggedInEmployee.GroupNumber, now.Month, now.Year);

            int total = allSubmissions.Count;
            int approved = allSubmissions.Count(s => s.Status == SubmissionStatus.Approved);
            int pending = allSubmissions.Count(s => s.Status == SubmissionStatus.Pending);

            decimal avgScore = total > 0
                ? Math.Round(allSubmissions.Average(s => s.Score), 1) : 0;
        }

        // ---------------------------------------------------------------
        // APPROVE button
        // ---------------------------------------------------------------
        private async void btnApprovebtn_Click(object sender, EventArgs e)
        {
            if (dgvApprovals.SelectedRows.Count == 0) return;

            dynamic selected = dgvApprovals.SelectedRows[0].DataBoundItem;
            int submissionId = selected.Id;

            string? managerComment = lblEmployeeComment.Text.Trim();

            try
            {
                await _repo.ApproveAsync(submissionId, _loggedInEmployee.Id, managerComment);

                MessageBox.Show("Evaluation Approved!", "Approved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTeamRequests();
                ClearDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------------------------------------------------------
        // REJECT button
        // ---------------------------------------------------------------
        private async void btnRejectbtn_Click(object sender, EventArgs e)
        {
            if (dgvApprovals.SelectedRows.Count == 0) return;

            dynamic selected = dgvApprovals.SelectedRows[0].DataBoundItem;
            int submissionId = selected.Id;

            string? managerComment = lblEmployeeComment.Text.Trim();

            try
            {
                await _repo.RejectAsync(submissionId, _loggedInEmployee.Id, managerComment);

                MessageBox.Show("Evaluation Rejected.", "Rejected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTeamRequests();
                ClearDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------------------------------------------------------
        // DataGridView row selected — show submission details on right
        // ---------------------------------------------------------------
        private void dgvApprovals_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvApprovals.SelectedRows.Count == 0)
            {
                ClearDetails();
                return;
            }

            var item = dgvApprovals.SelectedRows[0].DataBoundItem;
            if (item == null)
            {
                ClearDetails();
                return;
            }

            try
            {
                dynamic selected = item;
                lblEmployeeName.Text = selected.StaffName ?? "Unknown";
                lblDateOfSubmission.Text = selected.Date.ToShortDateString();
                lblEmployeeComment.Text = selected.Comment ?? "No comment provided.";

                // Show score and evaluation level
                lblEPercentage.Text = $"{selected.Score}%";
                lblEvaluationResult.Text = selected.Level;
                lblEvaluationlvl.Text = selected.Timeliness;
            }
            catch
            {
                ClearDetails();
            }
        }

        private void dgvApprovals_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvApprovals_SelectionChanged(sender, e);
        }

        private void ClearDetails()
        {
            lblEmployeeName.Text = "Name goes Here";
            lblDateOfSubmission.Text = "Date goes Here";
            lblEmployeeComment.Text = string.Empty;
            lblEPercentage.Text = "0%";
            lblEvaluationResult.Text = "—";
            lblEvaluationlvl.Text = "—";
            dgvApprovals.ClearSelection();
        }

        // ---------------------------------------------------------------
        // CREATE NEW MANAGER tab
        // ---------------------------------------------------------------
        private void btnCreateAccountCNM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsernameMD.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstNameMD.Text) ||
                string.IsNullOrWhiteSpace(txtLastNameMD.Text))
            {
                MessageBox.Show("First and Last name are required.", "Validation Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPasswordCNM.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error");
                return;
            }

            if (txtPasswordCNM.Text != txtConfirmPasswordCNM.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error");
                return;
            }

            if (cmbTeamCNM.SelectedItem == null)
            {
                MessageBox.Show("Please select a Team Number.", "Validation Error");
                return;
            }

            if (txtPasswordCNM.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation Error");
                return;
            }

            if (_employeeRepo.UsernameExists(txtUsernameMD.Text.Trim()))
            {
                MessageBox.Show($"Username '{txtUsernameMD.Text.Trim()}' is already taken.",
                    "Validation Error");
                return;
            }

            try
            {
                int groupNumber = cmbTeamCNM.SelectedIndex + 1;

                var newManager = new Employee
                {
                    Username = txtUsernameMD.Text.Trim(),
                    Password = txtPasswordCNM.Text,   // hashed inside repo.Add()
                    Name = $"{txtFirstNameMD.Text.Trim()} {txtLastNameMD.Text.Trim()}",
                    Designation = "Payable Supervisor",
                    Role = EmployeeRole.Supervisor,
                    GroupNumber = groupNumber
                };

                _employeeRepo.Add(newManager);

                // Update the Final Details summary labels
                lblUsernameCNM.Text = newManager.Username;
                lblFirstNameCNM.Text = txtFirstNameMD.Text.Trim();
                lblLastNameCNM.Text = txtLastNameMD.Text.Trim();
                lblPasswordCNM.Text = "********";
                lblTeamCNM.Text = $"Team {groupNumber}";

                MessageBox.Show(
                    $"Manager account created successfully!\n\n" +
                    $"Name: {newManager.Name}\n" +
                    $"Team: {groupNumber}\n" +
                    $"They can now log in with username: {newManager.Username}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            lblUsernameCNM.Text = "Enter a Username";
            lblFirstNameCNM.Text = "Enter a First Name";
            lblLastNameCNM.Text = "Enter a Last Name";
            lblPasswordCNM.Text = "Create a Password";
            lblTeamCNM.Text = "Choose a Team";
        }

        private void cmbTeamCNM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTeamCNM.SelectedItem != null)
                lblTeamCNM.Text = cmbTeamCNM.SelectedItem.ToString();
        }

        // ---------------------------------------------------------------
        // LOGOUT buttons
        // ---------------------------------------------------------------
        private void btnLogout1_Click(object sender, EventArgs e)
        {
            new Login_Form().Show();
            this.Hide();
        }

        private void btnLogout2_Click(object sender, EventArgs e)
        {
            new Login_Form().Show();
            this.Hide();
        }

        private void btnLogout3_Click(object sender, EventArgs e)
        {
            new Login_Form().Show();
            this.Hide();
        }

        private void Theme()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            Color myCrimson = Color.FromArgb(220, 20, 60);
            materialSkinManager.ColorScheme = new ColorScheme(
                myCrimson, myCrimson, myCrimson, myCrimson, TextShade.WHITE);
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
            lblDateOfSubmission.BackColor = Color.White;
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
            lblNameEmployee.BackColor = Color.White;
            lblSubmitDate.BackColor = Color.White;
        }

        private void ForeColorAdjustments()
        {
            Color myCrimson = Color.FromArgb(220, 20, 60);
            Color myDarkGray = Color.FromArgb(64, 64, 64);
            lblEpt2.ForeColor = myCrimson;
            materialLabel1.ForeColor = Color.Gray;
            materialLabel25.ForeColor = Color.Gray;
            lblManagerName2.ForeColor = Color.Gray;
            materialLabel12.ForeColor = Color.Gray;
            materialLabel23.ForeColor = Color.Gray;
            lblNameEmployee.ForeColor = myDarkGray;
            lblSubmitDate.ForeColor = myDarkGray;
            lblDateOfSubmission.ForeColor = myDarkGray;
            lblEmployeeName.ForeColor = myDarkGray;
            lblEPercentage.ForeColor = myCrimson;
            lblEvaluationlvl.ForeColor = myDarkGray;
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
            materialLabel34.ForeColor = myDarkGray;
            materialLabel29.ForeColor = myDarkGray;
            materialLabel33.ForeColor = myDarkGray;
            materialLabel30.ForeColor = myDarkGray;
            materialLabel37.ForeColor = myDarkGray;
            materialLabel38.ForeColor = myDarkGray;
            materialLabel41.ForeColor = myDarkGray;
            materialLabel32.ForeColor = myDarkGray;
            materialLabel40.ForeColor = myDarkGray;
            materialLabel49.ForeColor = myDarkGray;
            materialLabel50.ForeColor = myDarkGray;
        }
    }
}