using EvaluationDomain.Helpers;
using EvaluationDomain.Models;
using EvaluationInfrastructure.Repositories;
using MaterialSkin;
using MaterialSkin.Controls;

namespace Evaluation_Management
{
    public partial class StaffPage : MaterialForm
    {
        private readonly Employee _loggedInEmployee;
        private readonly EvaluationSubmissionRepository _submissionRepo = new();
        private readonly EmployeeRepository _employeeRepo = new();

        public StaffPage(Employee employee)
        {
            InitializeComponent();
            _loggedInEmployee = employee;
            Theme();
            BackColorAdjustments();
            ForeColorAdjustments();
        }

        private void StaffPage_Load(object sender, EventArgs e)
        {
            LoadHeaderInfo();
            LoadDeadlineInfo();
            LoadSubmissionsTab();
            SetupHistoryGrid();
            LoadSubmissionHistory();
            materialTabControl1.SelectedIndexChanged += materialTabControl1_SelectedIndexChanged;
        }

        // ---------------------------------------------------------------
        // HEADER
        // lblEpt = "Employee Performance Tracker" title — never touched
        // ---------------------------------------------------------------
        private void LoadHeaderInfo()
        {
            lblSp.Text = "Staff Portal";
            lblTeam.Text = _loggedInEmployee.GroupLabel;
            lblDos.Text = $"As of {DateTime.Now:MMMM yyyy}";
            lblNameofManager.Text = _loggedInEmployee.Supervisor?.Name ?? "No Supervisor Assigned";
        }

        // ---------------------------------------------------------------
        // DEADLINE INFO — shows days remaining until the 28th
        // ---------------------------------------------------------------
        private void LoadDeadlineInfo()
        {
            int daysLeft = ScoreCalculator.DaysUntilDeadline();
            var now = DateTime.Now;

            if (daysLeft > 0)
                lblKis.Text = $"{daysLeft} day{(daysLeft == 1 ? "" : "s")} until deadline";
            else if (daysLeft == 0)
                lblKis.Text = "Deadline is TODAY!";
            else
                lblKis.Text = $"Deadline passed ({Math.Abs(daysLeft)} day{(Math.Abs(daysLeft) == 1 ? "" : "s")} ago)";

            lblKisDes.Text = $"Submit by the 28th of {now:MMMM yyyy}";

            // Check if already submitted this month
            var thisMonthSubmission = _submissionRepo
                .GetForEmployeeAndPeriod(_loggedInEmployee.Id, now.Month, now.Year);

            if (thisMonthSubmission != null)
            {
                lblSa.Text = thisMonthSubmission.StatusDisplay;
                lblSaDes.Text = $"Score: {thisMonthSubmission.Score:F0} — {thisMonthSubmission.EvaluationLevel}";
            }
            else
            {
                lblSa.Text = "Not Yet Submitted";
                lblSaDes.Text = $"Submit before the 28th for full score";
            }
        }

        // ---------------------------------------------------------------
        // SUBMISSIONS TAB — total count and score stats
        // ---------------------------------------------------------------
        private void LoadSubmissionsTab()
        {
            var allSubmissions = _submissionRepo.GetForEmployee(_loggedInEmployee.Id);

            lblTotalSubmissionNum.Text = allSubmissions.Count.ToString();

            int approved = allSubmissions.Count(s => s.Status == SubmissionStatus.Approved);
            decimal approvalRate = allSubmissions.Count > 0
                ? Math.Round((decimal)approved / allSubmissions.Count * 100, 1) : 0;
            lblApprovalRateNum.Text = $"{approvalRate}%";

            // Set date picker default to today
            dtpDate.Value = DateTime.Now;

            // Load month options into combo box

        }

        // ---------------------------------------------------------------
        // SEND TO MANAGER button — materialButton4
        // ---------------------------------------------------------------
        private async void btnSendToManager_Click_1(object sender, EventArgs e)
        {

            if (_loggedInEmployee.Supervisor == null)
            {
                MessageBox.Show("You have no supervisor assigned. Contact your AAM.",
                    "Submit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string comment = lblComment.Text.Trim();

            if (string.IsNullOrWhiteSpace(comment))
            {
                MessageBox.Show("Please write your performance update before submitting.",
                    "Submit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            DateTime submissionDate = dtpDate.Value;

            // Compute score preview - use submissionDate's month/year instead of undefined `period`
            decimal score = ScoreCalculator.Compute(submissionDate, submissionDate.Month, submissionDate.Year);
            string level = ScoreCalculator.GetLevel(score);

            // Timeliness: compare submission date to the 28th of the same month/year
            string timeliness = submissionDate.Day <= 28
                ? "On time — full score"
                : $"{(submissionDate.Date - new DateTime(submissionDate.Year, submissionDate.Month, 28)).Days} day(s) late";

            var confirm = MessageBox.Show(
                //$"Submit evaluation for {selectedPeriod}?\n\n" +
                $"Submission Date: {submissionDate:MMMM dd, yyyy}\n" +
                $"Timeliness: {timeliness}\n" +
                $"Computed Score: {score:F0} — {level}\n" +
                $"Submitted to: {_loggedInEmployee.Supervisor.Name}",
                "Confirm Submission",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                // ← this is the only real change, .Wait() → await
                await _submissionRepo.SubmitAsync(
                    _loggedInEmployee.Id,
                    submissionDate.Month,
                    submissionDate.Year,
                    submissionDate,
                    comment);

                MessageBox.Show(
                    $"Evaluation submitted successfully!\n\nScore: {score:F0} — {level}",
                    "Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearSubmissionForm();
                LoadDeadlineInfo();
                LoadSubmissionsTab();
                LoadSubmissionHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Submit",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------------------------------------------------------
        // CLEAR button — materialButton3
        // ---------------------------------------------------------------
        private void materialButton3_Click(object sender, EventArgs e)
        {
            ClearSubmissionForm();
        }

        private void ClearSubmissionForm()
        {
            lblComment.Text = string.Empty;
            dtpDate.Value = DateTime.Now;

        }

        // ---------------------------------------------------------------
        // HISTORY TAB — DataGridView setup
        // ---------------------------------------------------------------
        private void SetupHistoryGrid()
        {
            dataGridView1.Columns["Column1"].HeaderText = "Period";
            dataGridView1.Columns["Column2"].HeaderText = "Submitted On";
            dataGridView1.Columns["Column3"].HeaderText = "Timeliness";
            dataGridView1.Columns["Column4"].HeaderText = "Score";
            dataGridView1.Columns["Column5"].HeaderText = "Status";

            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        // ---------------------------------------------------------------
        // HISTORY TAB — load all submissions
        // ---------------------------------------------------------------
        private void LoadSubmissionHistory()
        {
            dataGridView1.Rows.Clear();

            var submissions = _submissionRepo.GetForEmployee(_loggedInEmployee.Id);

            foreach (var sub in submissions)
            {
                dataGridView1.Rows.Add(
                    sub.PeriodDisplay,
                    sub.SubmissionDate.ToString("MMM dd, yyyy"),
                    sub.TimelinessDisplay,
                    $"{sub.Score:F0} — {sub.EvaluationLevel}",
                    sub.StatusDisplay
                );

                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = sub;

                // Color code the status
                var row = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
                row.DefaultCellStyle.ForeColor = sub.Status switch
                {
                    SubmissionStatus.Approved => Color.FromArgb(0, 150, 80),
                    SubmissionStatus.Rejected => Color.FromArgb(200, 30, 30),
                    _ => Color.FromArgb(64, 64, 64)
                };
            }

            if (dataGridView1.Rows.Count == 0)
                ClearSubmissionDetails();
        }

        // ---------------------------------------------------------------
        // HISTORY TAB — row selected, show details on right
        // ---------------------------------------------------------------
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var row = dataGridView1.SelectedRows[0];
            if (row.Tag is not EvaluationSubmission sub) return;

            materialTextBox1.Text = _loggedInEmployee.Name;
            materialTextBox2.Text = sub.SubmissionDate.ToString("MMMM dd, yyyy");
            materialTextBox3.Text = _loggedInEmployee.Supervisor?.Name ?? "No Supervisor";

            // Status badge
            materialLabel7.Text = sub.StatusDisplay;
            Color levelColor = ScoreCalculator.GetLevelColor(sub.Score);
            panel12.BackColor = sub.Status switch
            {
                SubmissionStatus.Approved => Color.FromArgb(0, 150, 80),
                SubmissionStatus.Pending => Color.FromArgb(246, 190, 0),
                SubmissionStatus.Rejected => Color.FromArgb(200, 30, 30),
                _ => levelColor
            };
            materialLabel7.BackColor = panel12.BackColor;

            // Comment
            if (materialCard1.Controls.Count > 0 &&
                materialCard1.Controls[0] is MaterialMultiLineTextBox commentBox)
            {
                commentBox.Text = string.IsNullOrWhiteSpace(sub.ManagerComment)
                    ? sub.Comment
                    : $"Your comment:\n{sub.Comment}\n\nManager's feedback:\n{sub.ManagerComment}";
            }
        }

        private void ClearSubmissionDetails()
        {
            materialTextBox1.Text = string.Empty;
            materialTextBox2.Text = string.Empty;
            materialTextBox3.Text = string.Empty;
            materialLabel7.Text = "No Data";
            panel12.BackColor = Color.LightCoral;
            materialLabel7.BackColor = Color.LightCoral;
        }

        // ---------------------------------------------------------------
        // Tab switching
        // ---------------------------------------------------------------
        private void materialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialTabControl1.SelectedIndex == 1)
                LoadSubmissionHistory();
        }

        private void Theme()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            Color myCrimson = Color.FromArgb(220, 20, 60);
            materialSkinManager.ColorScheme = new ColorScheme(
                myCrimson, myCrimson, myCrimson, myCrimson, TextShade.WHITE);
            materialCard2.Padding = new Padding(0);
        }

        private void BackColorAdjustments()
        {
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.White;
            panel5.BackColor = Color.White;
            panel6.BackColor = Color.White;
            lblEpt.BackColor = Color.White;
            lblNpuDes.BackColor = Color.White;
            lblNpu.BackColor = Color.White;
            labelM.BackColor = Color.White;
            lblDos.BackColor = Color.White;
            lblCom.BackColor = Color.White;
            lblSp.BackColor = Color.White;
            lblFdSg.BackColor = Color.White;
            lblST.BackColor = Color.White;
            lblKis.BackColor = Color.White;
            lblKisDes.BackColor = Color.White;
            lblSa.BackColor = Color.White;
            lblSaDes.BackColor = Color.White;
            lblTs.BackColor = Color.White;
            lblTotalSubmissionNum.BackColor = Color.White;
            lblAr.BackColor = Color.White;
            lblApprovalRateNum.BackColor = Color.White;
            materialLabel1.BackColor = Color.White;
            lblNameofManager.BackColor = Color.White;
            materialLabel11.BackColor = Color.White;
            materialLabel3.BackColor = Color.White;
            materialLabel6.BackColor = Color.White;
            materialLabel8.BackColor = Color.White;
            materialLabel4.BackColor = Color.White;
            panel12.BackColor = Color.LightCoral;
            materialLabel7.BackColor = Color.LightCoral;
            materialLabel5.BackColor = Color.White;
            lblTeam.BackColor = Color.White;
            lblEpt2.BackColor = Color.White;
        }

        private void ForeColorAdjustments()
        {
            Color myCrimson = Color.FromArgb(220, 20, 60);
            Color myDarkGray = Color.FromArgb(64, 64, 64);
            lblEpt.ForeColor = myCrimson;
            lblMsDes.ForeColor = Color.Gray;
            lblSp.ForeColor = Color.Gray;
            lblFdSg.ForeColor = Color.Gray;
            lblNpuDes.ForeColor = Color.Gray;
            lblKis.ForeColor = myDarkGray;
            lblSa.ForeColor = myDarkGray;
            lblKisDes.ForeColor = Color.Gray;
            lblSaDes.ForeColor = Color.Gray;
            lblTotalSubmissionNum.ForeColor = myCrimson;
            lblApprovalRateNum.ForeColor = myCrimson;
            lblAr.ForeColor = myDarkGray;
            lblTs.ForeColor = myDarkGray;
            lblNameofManager.ForeColor = myCrimson;
            materialLabel1.ForeColor = Color.Gray;
            materialLabel3.ForeColor = myDarkGray;
            materialLabel6.ForeColor = myDarkGray;
            materialLabel8.ForeColor = myDarkGray;
            materialLabel7.ForeColor = Color.White;
            lblEpt2.ForeColor = myCrimson;
            materialLabel12.ForeColor = Color.Gray;
        }

        private void btnLogout1_Click(object sender, EventArgs e)
        {
            Login_Form form = new Login_Form();
            form.Show();
            this.Hide();
        }

        private void btnLogout2_Click(object sender, EventArgs e)
        {
            Login_Form form = new Login_Form();
            form.Show();
            this.Hide();
        }
    }
}