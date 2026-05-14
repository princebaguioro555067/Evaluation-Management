using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using EvaluationInfrastructure.Repositories;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Evaluation_Management
{
    public partial class ManagerPage : MaterialForm
    {
        private readonly Employee _loggedInEmployee;
        private readonly KpmScoreRepository _kpmRepo = new KpmScoreRepository();
        private readonly EvaluationPeriodRepository _periodRepo = new EvaluationPeriodRepository();
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();
        private readonly SubmissionRepository _repo = new SubmissionRepository();
        private readonly AppDbContext _context = new AppDbContext();

        public ManagerPage(Employee employee)
        {
            InitializeComponent();
            _loggedInEmployee = employee;
            Theme();
            BackColorAdjustments();
            ForeColorAdjustments();
            LoadTeamRequests();
        }

        private void LoadTeamRequests()
        {
            // Only shows submissions belonging to this Manager's GroupNumber
            dgvApprovals.DataSource = _repo.GetByGroup(_loggedInEmployee.GroupNumber)
                                           .Where(x => x.Status == "Pending")
                                           .ToList();
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

            // Adding teams 1 through 5 to the ComboBox
            cmbTeamCNM.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                cmbTeamCNM.Items.Add($"Team {i}");
            }
            if (cmbTeamCNM.Items.Count > 0) cmbTeamCNM.SelectedIndex = 0;
        }

        private void LoadManagerStats()
        {
            var now = DateTime.Now;
            var period = _periodRepo.GetByMonthYear(now.Month, now.Year);

            var manager = _loggedInEmployee; // The logged-in manager passed from Login_Form

            // We join Submissions with Employees to get the Staff Name
            var displayList = _context.Submissions
                .Where(s => s.GroupNumber == manager.GroupNumber && s.Status == "Pending")
                .Join(_context.Employees,
                      sub => sub.EmployeeId,
                      emp => emp.Id,
                      (sub, emp) => new
                      {
                          sub.Id,
                          StaffName = emp.Name, // This replaces EmployeeId
                          Date = sub.SubmissionDate,
                          sub.Comment,
                          sub.Status
                      })
                .ToList();

            dgvApprovals.DataSource = displayList;

            // Hide the ID and Comment columns in the DGV to keep it clean
            if (dgvApprovals.Columns["Id"] != null) dgvApprovals.Columns["Id"].Visible = false;
            if (dgvApprovals.Columns["Comment"] != null) dgvApprovals.Columns["Comment"].Visible = false;
            if (displayList.Count == 0)
            {
                dgvApprovals.DataSource = null;
                // Optional: Add a message if no data exists
                // MessageBox.Show("No pending submissions for your team.");
            }
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
            Color myDrakGray = Color.FromArgb(64, 64, 64);

            lblEpt2.ForeColor = myCrimson;
            materialLabel1.ForeColor = Color.Gray;

            materialLabel25.ForeColor = Color.Gray;
            lblManagerName2.ForeColor = Color.Gray;
            materialLabel12.ForeColor = Color.Gray;
            materialLabel23.ForeColor = Color.Gray;

            lblNameEmployee.ForeColor = myDrakGray;
            lblSubmitDate.ForeColor = myDrakGray;
            lblDateOfSubmission.ForeColor = myDrakGray;
            lblEmployeeName.ForeColor = myDrakGray;

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

            if (cmbTeamCNM.SelectedItem == null)
            {
                MessageBox.Show("Please select a Team Number.");
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
            lblUsernameCNM.Text = "Enter a Username";
            lblFirstNameCNM.Text = "Enter a First Name";
            lblLastNameCNM.Text = "Enter a Last Name";
            lblPasswordCNM.Text = "Create a Password";
            lblTeamCNM.Text = "Choose a Team";
        }

        private void btnLogout3_Click(object sender, EventArgs e)
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

        private void btnLogout1_Click(object sender, EventArgs e)
        {
            Login_Form form = new Login_Form();
            form.Show();
            this.Hide();
        }

        private void btnApprovebtn_Click(object sender, EventArgs e)
        {
            if (dgvApprovals.SelectedRows.Count > 0)
            {
                dynamic selected = dgvApprovals.SelectedRows[0].DataBoundItem;
                int submissionId = selected.Id;

                var subRecord = _context.Submissions.Find(submissionId);
                if (subRecord != null)
                {
                    subRecord.Status = "Approved";
                    _context.SaveChanges();

                    MessageBox.Show("Evaluation Approved!");
                    LoadTeamRequests(); // Refresh the grid
                    ClearDetails(); // Clear the right-side labels
                }
            }
        }

        private void ClearDetails()
        {
            // Resets the text to your original placeholders
            lblEmployeeName.Text = "Name goes Here";
            lblDateOfSubmission.Text = "Date goes Here";
            lblEmployeeComment.Text = string.Empty;

            // Removes the blue highlight from the grid
            dgvApprovals.ClearSelection();
        }

        private void cmbTeamCNM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTeamCNM.SelectedItem != null)
            {
                // Updates the "Team: Choose a Team" label instantly
                lblTeamCNM.Text = cmbTeamCNM.SelectedItem.ToString();
            }
        }

        private void dgvApprovals_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvApprovals.SelectedRows.Count > 0)
            {
                // Get the selected anonymous object
                dynamic selected = dgvApprovals.SelectedRows[0].DataBoundItem;

                // Map the data to your labels/textboxes on the right side
                lblEmployeeName.Text = selected.StaffName;
                lblDateOfSubmission.Text = selected.Date.ToShortDateString();
                lblEmployeeComment.Text = selected.Comment; // This shows the large text
            }
        }

        private void dgvApprovals_SelectionChanged(object sender, EventArgs e)
        {
            // 1. Safety Guard: If no rows are selected (common during refresh), stop immediately.
            // This prevents the "Index was out of range" error.
            if (dgvApprovals.SelectedRows.Count == 0)
            {
                ClearDetails();
                return;
            }

            // 2. Get the data object from the selected row.
            var item = dgvApprovals.SelectedRows[0].DataBoundItem;

            // 3. If the item is null, we can't display anything.
            if (item == null)
            {
                ClearDetails();
                return;
            }

            try
            {
                // 4. Use 'dynamic' to access the custom properties (StaffName, Date, Comment) 
                // created in your LINQ Join query.
                dynamic selected = item;

                // 5. Update the "Submission Details" panel labels/textboxes.
                lblEmployeeName.Text = selected.StaffName ?? "Unknown";
                lblDateOfSubmission.Text = selected.Date.ToShortDateString();

                // Ensure this control name matches the large box in your UI.
                lblEmployeeComment.Text = selected.Comment ?? "No comment provided.";
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                // This catches cases where the selected object doesn't have the expected properties.
                ClearDetails();
            }
        }
    }
}
