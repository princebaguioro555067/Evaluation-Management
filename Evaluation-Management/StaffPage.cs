using EvaluationDomain.Models;
using EvaluationInfrastructure.Repositories;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Evaluation_Management
{
    public partial class StaffPage : MaterialForm
    {

        private readonly Employee _loggedInEmployee;
        private readonly KpmScoreRepository _kpmRepo = new KpmScoreRepository();
        private readonly EvaluationPeriodRepository _periodRepo = new EvaluationPeriodRepository();
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();

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
            lblEpt.Text = "Employee Performance Tracker";
            lblEpt2.Text = "Employee Performance Tracker";
            LoadEmployeeInfo();
            LoadDashboardStats();
        }

        private void LoadEmployeeInfo()
        {
            lblEpt.Text = _loggedInEmployee.Name;
            lblSp.Text = "Staff Portal";
            lblNpu.Text = _loggedInEmployee.RoleDisplay;
            lblNpuDes.Text = _loggedInEmployee.Designation;
            labelM.Text = _loggedInEmployee.GroupLabel;
            lblDos.Text = $"As of {DateTime.Now:MMMM yyyy}";
            lblCom.Text = "Payable Section";

            if (_loggedInEmployee.Supervisor != null)
                lblFdSg.Text = $"Supervisor: {_loggedInEmployee.Supervisor.Name}";
        }

        private void LoadDashboardStats()
        {
            var now = DateTime.Now;
            var period = _periodRepo.GetByMonthYear(now.Month, now.Year);

            if (period == null)
            {
                // No period yet — show zeroes and N/A
                lblTotalSubmissionNum.Text = "0";
                lblApprovalRateNum.Text = "0%";
                SetEvaluationLevel(0);
                return;
            }

            var scores = _kpmRepo.GetForEmployee(_loggedInEmployee.Id, period.Id);

            // --- Total Submissions ---
            lblTotalSubmissionNum.Text = scores.Count.ToString();

            // --- Approval Rate (% of KPMs that met or exceeded target) ---
            int metTarget = scores.Count(s => s.Actual.HasValue && s.Actual >= s.Target);
            decimal approvalRate = scores.Count > 0
                ? Math.Round((decimal)metTarget / scores.Count * 100, 1) : 0;
            lblApprovalRateNum.Text = $"{approvalRate}%";

            // --- Total KPM Score out of 100 ---
            decimal totalPoints = _kpmRepo.GetTotalPoints(_loggedInEmployee.Id, period.Id);

            // --- Evaluation Level based on total points ---
            SetEvaluationLevel(totalPoints);
        }

        private void SetEvaluationLevel(decimal totalPoints)
        {
            string level;
            Color badgeColor;
            Color textColor = Color.White;

            if (totalPoints >= 95)
            {
                level = "Excellent";
                badgeColor = Color.FromArgb(0, 150, 80);      // Green
            }
            else if (totalPoints >= 85)
            {
                level = "Great";
                badgeColor = Color.FromArgb(30, 130, 200);    // Blue
            }
            else if (totalPoints >= 75)
            {
                level = "Good";
                badgeColor = Color.FromArgb(255, 165, 0);     // Orange
                textColor = Color.White;
            }
            else if (totalPoints >= 65)
            {
                level = "Fair";
                badgeColor = Color.FromArgb(220, 120, 20);    // Dark Orange
            }
            else if (totalPoints > 0)
            {
                level = "Needs Improvement";
                badgeColor = Color.FromArgb(200, 30, 30);     // Red
            }
            else
            {
                level = "No Data";
                badgeColor = Color.Gray;
            }

            // Update the status badge label and its background panel
            materialLabel7.Text = level;
            materialLabel7.ForeColor = textColor;
            panel12.BackColor = badgeColor;
            materialLabel7.BackColor = badgeColor;
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
            lblEpt2.BackColor = Color.White;

            materialLabel11.BackColor = Color.White;
            
            materialLabel3.BackColor = Color.White;
            materialLabel6.BackColor = Color.White;
            materialLabel8.BackColor = Color.White;
            materialLabel4.BackColor = Color.White;
            materialLabel5.BackColor = Color.White;

        }
        private void ForeColorAdjustments()
        {
            Color myCrimson = Color.FromArgb(220, 20, 60);
            Color myDrakGray = Color.FromArgb(64, 64, 64);

            lblEpt.ForeColor = myCrimson;
            lblMsDes.ForeColor = Color.Gray;
            lblSp.ForeColor = Color.Gray;
            lblFdSg.ForeColor = Color.Gray;
            lblNpuDes.ForeColor = Color.Gray;

            lblKis.ForeColor = myDrakGray;
            lblSa.ForeColor = myDrakGray;
            lblKisDes.ForeColor = Color.Gray;
            lblSaDes.ForeColor = Color.Gray;

            lblTotalSubmissionNum.ForeColor = myCrimson;
            lblApprovalRateNum.ForeColor = myCrimson;

            lblAr.ForeColor = myDrakGray;
            lblTs.ForeColor = myDrakGray;

            lblEpt2.ForeColor = myCrimson;
            materialLabel1.ForeColor = Color.Gray;

            materialLabel3.ForeColor = myDrakGray;
            materialLabel6.ForeColor = myDrakGray;
            materialLabel8.ForeColor = myDrakGray;

            
        }

    }

}
