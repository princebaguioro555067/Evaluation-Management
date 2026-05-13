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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            materialLabel7.Text = _loggedInEmployee.Name;
            materialLabel18.Text = _loggedInEmployee.RoleDisplay;
            materialLabel9.Text = _loggedInEmployee.Designation;
            materialLabel10.Text = _loggedInEmployee.GroupLabel;
            materialLabel8.Text = $"As of {DateTime.Now:MMMM yyyy}";

            if (_loggedInEmployee.Role == EmployeeRole.Supervisor)
            {
                var staffList = _employeeRepo.GetStaffUnderSupervisor(_loggedInEmployee.Id);
                materialLabel4.Text = staffList.Count.ToString();
            }
            else if (_loggedInEmployee.Role == EmployeeRole.AAM)
            {
                var allStaff = _employeeRepo.GetByRole(EmployeeRole.Staff);
                materialLabel4.Text = allStaff.Count.ToString();
            }
        }

        private void LoadManagerStats()
        {
            var now = DateTime.Now;
            var period = _periodRepo.GetByMonthYear(now.Month, now.Year);

            if (period == null)
            {
                materialLabel2.Text = "0 pts";
                materialLabel5.Text = "0";
                materialLabel3.Text = "0%";
                materialLabel6.Text = "0 pts avg";
                return;
            }

            List<KpmScore> scores;

            if (_loggedInEmployee.Role == EmployeeRole.AAM)
                scores = _kpmRepo.GetAllForPeriod(period.Id);
            else
                scores = _kpmRepo.GetForGroup(_loggedInEmployee.GroupNumber, period.Id);

            decimal myPoints = _kpmRepo.GetTotalPoints(_loggedInEmployee.Id, period.Id);
            materialLabel2.Text = $"{myPoints:F2} pts";

            materialLabel5.Text = scores.Count.ToString();

            int metTarget = scores.Count(s => s.Actual.HasValue && s.Actual >= s.Target);
            decimal approvalRate = scores.Count > 0
                ? Math.Round((decimal)metTarget / scores.Count * 100, 1) : 0;
            materialLabel3.Text = $"{approvalRate}%";

            var employeesWithScores = scores
                .GroupBy(s => s.EmployeeId)
                .Select(g => g.Sum(s => s.Points ?? 0))
                .ToList();
            decimal avgScore = employeesWithScores.Count > 0
                ? Math.Round(employeesWithScores.Average(), 2) : 0;
            materialLabel6.Text = $"{avgScore:F2} pts avg";
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
            materialLabel2.BackColor = Color.White;

            materialLabel11.BackColor = Color.White;

            materialLabel3.BackColor = Color.White;
            materialLabel6.BackColor = Color.White;

            materialLabel4.BackColor = Color.White;

            materialLabel18.BackColor = Color.White;

            materialLabel7.BackColor = Color.White;
            materialLabel5.BackColor = Color.White;

            materialLabel10.BackColor = Color.White;
            materialLabel9.BackColor = Color.White;

            materialLabel27.BackColor = Color.White;
            materialLabel28.BackColor = Color.White;
            materialLabel20.BackColor = Color.White;
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

            materialLabel43.BackColor = Color.White;
            materialLabel44.BackColor = Color.White;
            materialLabel45.BackColor = Color.White;

            materialLabel49.BackColor = Color.White;
            materialLabel50.BackColor = Color.White;

            materialLabel47.BackColor = Color.White;
            materialLabel48.BackColor = Color.White;




        }
        private void ForeColorAdjustments()
        {
            Color myCrimson = Color.FromArgb(220, 20, 60);
            Color myDrakGray = Color.FromArgb(64, 64, 64);

            materialLabel2.ForeColor = myCrimson;
            materialLabel1.ForeColor = Color.Gray;

            materialLabel25.ForeColor = Color.Gray;
            materialLabel24.ForeColor = Color.Gray;
            materialLabel12.ForeColor = Color.Gray;
            materialLabel23.ForeColor = Color.Gray;
            materialLabel28.ForeColor = myDrakGray;

            materialLabel7.ForeColor = myCrimson;

            materialLabel5.ForeColor = myDrakGray;
            materialLabel4.ForeColor = myCrimson;

            materialLabel3.ForeColor = myDrakGray;
            materialLabel6.ForeColor = myDrakGray;

            materialLabel10.ForeColor = myCrimson;

            materialLabel9.ForeColor = Color.Gray;
            materialLabel8.ForeColor = Color.Gray;

            materialLabel14.ForeColor = Color.Gray;
            materialLabel15.ForeColor = Color.Gray;
            materialLabel16.ForeColor = Color.Gray;
            materialLabel17.ForeColor = Color.Gray;

            materialLabel20.ForeColor = myCrimson;
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


    }
}
