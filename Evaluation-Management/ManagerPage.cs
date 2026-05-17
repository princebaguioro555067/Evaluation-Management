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
        private List<PerformanceListItem> _allPerformanceItems = new();
        private List<EvaluationSubmission> _allPendingSubmissions = new();

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
            btnReject.Click += btnRejectbtn_Click;
            LoadManagerInfo();
            LoadManagerStats();
            LoadTeamRequests();
            LoadPerformanceList();
        }

        // ---------------------------------------------------------------
        // PERFORMANCE LIST TAB
        // ---------------------------------------------------------------

        private class PerformanceListItem
        {
            public string EmployeeName { get; set; } = string.Empty;
            public string Team { get; set; } = string.Empty;
            public string SubmittedOn { get; set; } = string.Empty;
            public string Score { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
        }

        private void LoadPerformanceList()
        {
            if (cmbMonthTPL.SelectedIndex < 0 || cmbYearTPL.SelectedItem == null) return;

            int month = cmbMonthTPL.SelectedIndex + 1;
            int year = int.Parse(cmbYearTPL.SelectedItem.ToString()!);

            dgvTPL.Columns["dataGridViewTextBoxColumn1"].HeaderText = "Employee Name";
            dgvTPL.Columns["dataGridViewTextBoxColumn2"].HeaderText = "Team";
            dgvTPL.Columns["dataGridViewTextBoxColumn3"].HeaderText = "Submitted On";
            dgvTPL.Columns["dataGridViewTextBoxColumn4"].HeaderText = "Score";
            dgvTPL.Columns["dataGridViewTextBoxColumn5"].HeaderText = "Status";
            dgvTPL.ReadOnly = true;
            dgvTPL.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTPL.MultiSelect = false;
            dgvTPL.AllowUserToAddRows = false;

            List<Employee> staffList;

            if (_loggedInEmployee.Role == EmployeeRole.AAM)
                staffList = _employeeRepo.GetByRole(EmployeeRole.Staff);
            else
                staffList = _employeeRepo.GetStaffUnderSupervisor(_loggedInEmployee.Id);

            _allPerformanceItems.Clear();

            foreach (var staff in staffList)
            {
                var submission = _repo.GetForEmployeeAndPeriod(staff.Id, month, year);

                _allPerformanceItems.Add(new PerformanceListItem
                {
                    EmployeeName = staff.Name,
                    Team = $"Group {staff.GroupNumber}",
                    SubmittedOn = submission != null
                        ? submission.SubmissionDate.ToString("MMM dd, yyyy")
                        : "Not Submitted",
                    Score = submission != null
                        ? $"{submission.Score:F0} — {submission.EvaluationLevel}"
                        : "—",
                    Status = submission != null
                        ? submission.StatusDisplay
                        : "Not Submitted"
                });
            }

            PopulatePerformanceGrid(_allPerformanceItems);
        }

        private void PopulatePerformanceGrid(List<PerformanceListItem> items)
        {
            dgvTPL.Rows.Clear();

            foreach (var item in items)
            {
                int rowIndex = dgvTPL.Rows.Add(
                    item.EmployeeName,
                    item.Team,
                    item.SubmittedOn,
                    item.Score,
                    item.Status
                );

                var row = dgvTPL.Rows[rowIndex];
                row.DefaultCellStyle.ForeColor = item.Status switch
                {
                    "Approved" => Color.FromArgb(0, 150, 80),
                    "Rejected" => Color.FromArgb(200, 30, 30),
                    "Pending" => Color.FromArgb(30, 130, 200),
                    "Not Submitted" => Color.FromArgb(150, 150, 150),
                    _ => Color.FromArgb(64, 64, 64)
                };
            }
        }

        private void UpdatePerformanceSummary(List<PerformanceListItem> items)
        {
            int total = items.Count;
            int submitted = items.Count(i => i.Status != "Not Submitted");
            int notSubmitted = total - submitted;
        }

        private void txtSearchTPL_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchTPL.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                PopulatePerformanceGrid(_allPerformanceItems);
                return;
            }

            var filtered = _allPerformanceItems
                .Where(i =>
                    i.EmployeeName.ToLower().Contains(searchText) ||
                    i.Team.ToLower().Contains(searchText) ||
                    i.Status.ToLower().Contains(searchText))
                .ToList();

            PopulatePerformanceGrid(filtered);
        }

        // ---------------------------------------------------------------
        // TEAM EVALUATION TAB
        // ---------------------------------------------------------------

        private void LoadTeamRequests()
        {
            if (cmbMonthTPE.SelectedIndex < 0 || cmbYearTPE.SelectedItem == null) return;

            int month = cmbMonthTPE.SelectedIndex + 1;
            int year = int.Parse(cmbYearTPE.SelectedItem.ToString()!);

            List<EvaluationSubmission> submissions;

            if (_loggedInEmployee.Role == EmployeeRole.AAM)
                submissions = _repo.GetAllForPeriod(month, year);
            else
                submissions = _repo.GetForGroup(_loggedInEmployee.GroupNumber, month, year);

            _allPendingSubmissions = submissions
                .Where(s => s.Status == SubmissionStatus.Pending)
                .ToList();

            PopulateApprovalsGrid(_allPendingSubmissions);
        }

        private void PopulateApprovalsGrid(List<EvaluationSubmission> items)
        {
            var displayList = items.Select(s => new
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
            }).ToList();

            dgvApprovals.DataSource = displayList;

            if (dgvApprovals.Columns["Id"] != null)
                dgvApprovals.Columns["Id"].Visible = false;
            if (dgvApprovals.Columns["Comment"] != null)
                dgvApprovals.Columns["Comment"].Visible = false;
            if (dgvApprovals.Columns["Status"] != null)
                dgvApprovals.Columns["Status"].Visible = false;
            if (dgvApprovals.Columns["StaffName"] != null)
                dgvApprovals.Columns["StaffName"].HeaderText = "Employee Name";

            if (displayList.Count == 0)
                ClearDetails();
        }

        private void LoadManagerInfo()
        {
            lblManagerName.Text = _loggedInEmployee.Name;
            lblTeamTPL.Text = _loggedInEmployee.Role == EmployeeRole.AAM
                ? "All Teams"
                : $"Group {_loggedInEmployee.GroupNumber}";

            lblEpt.Text = "Employee Performance Tracker";
            lblEpt2.Text = "Employee Performance Tracker";
            lblEpt3.Text = "Employee Performance Tracker";
            lblManagerName2.Text = _loggedInEmployee.Name;

            materialLabel23.Text = _loggedInEmployee.Role == EmployeeRole.AAM
                ? "All Teams"
                : $"Group {_loggedInEmployee.GroupNumber}";

            // TPL comboboxes
            cmbMonthTPL.Items.Clear();
            for (int m = 1; m <= 12; m++)
                cmbMonthTPL.Items.Add(new DateTime(2000, m, 1).ToString("MMMM"));
            cmbMonthTPL.SelectedIndex = DateTime.Now.Month - 1;

            cmbYearTPL.Items.Clear();
            int currentYear = DateTime.Now.Year;
            for (int y = currentYear - 2; y <= currentYear + 1; y++)
                cmbYearTPL.Items.Add(y.ToString());
            cmbYearTPL.SelectedItem = currentYear.ToString();

            // TPE comboboxes
            cmbMonthTPE.Items.Clear();
            for (int m = 1; m <= 12; m++)
                cmbMonthTPE.Items.Add(new DateTime(2000, m, 1).ToString("MMMM"));
            cmbMonthTPE.SelectedIndex = DateTime.Now.Month - 1;

            cmbYearTPE.Items.Clear();
            for (int y = currentYear - 2; y <= currentYear + 1; y++)
                cmbYearTPE.Items.Add(y.ToString());
            cmbYearTPE.SelectedItem = currentYear.ToString();

            // Wire TPL
            cmbMonthTPL.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
            cmbYearTPL.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
            cmbMonthTPL.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            cmbYearTPL.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            // Wire TPE
            cmbMonthTPE.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
            cmbYearTPE.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
            cmbMonthTPE.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            cmbYearTPE.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            cmbTeamCNM.Items.Clear();
            for (int i = 1; i <= 5; i++)
                cmbTeamCNM.Items.Add($"Team {i}");
            if (cmbTeamCNM.Items.Count > 0)
                cmbTeamCNM.SelectedIndex = 0;

            txtPasswordCNM.Password = true;
            txtConfirmPasswordCNM.Password = true;
        }

        private void cbShowPasswordEPT_CheckedChanged(object sender, EventArgs e)
        {
            txtPasswordCNM.Password = !cbShowPasswordEPT.Checked;
            txtConfirmPasswordCNM.Password = !cbShowPasswordEPT.Checked;
        }

        private void ComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadTeamRequests();
            LoadPerformanceList();
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

            string? managerComment = null; // manager view-only, no feedback input

            try
            {
                await _repo.ApproveAsync(submissionId, _loggedInEmployee.Id, managerComment);

                MessageBox.Show("Evaluation Approved!", "Approved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTeamRequests();
                LoadPerformanceList();
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

            string? managerComment = null; // manager view-only, no feedback input

            try
            {
                await _repo.RejectAsync(submissionId, _loggedInEmployee.Id, managerComment);

                MessageBox.Show("Evaluation Rejected.", "Rejected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTeamRequests();
                LoadPerformanceList();
                ClearDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ---------------------------------------------------------------
        // SELECTION CHANGED
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

                lblEPercentage.Text = $"{selected.Score}%";
                lblEvaluationlvl.Text = $"Evaluation Level: {selected.Timeliness}  |  {selected.Level}";
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
            lblEmployeeName.Text = "Name ";
            lblDateOfSubmission.Text = "Date ";
            lblEmployeeComment.Text = string.Empty;
            lblEPercentage.Text = "0%";
            lblEvaluationlvl.Text = "Evaluation Level:";
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
                    Password = txtPasswordCNM.Text,
                    Name = $"{txtFirstNameMD.Text.Trim()} {txtLastNameMD.Text.Trim()}",
                    Designation = "Payable Supervisor",
                    Role = EmployeeRole.Supervisor,
                    GroupNumber = groupNumber
                };

                _employeeRepo.Add(newManager);

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

        // ---------------------------------------------------------------
        // THEME / STYLING
        // ---------------------------------------------------------------

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
            lblTeamTPL.ForeColor = Color.Gray;
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