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
        public StaffPage()
        {
            InitializeComponent();
            Theme();
            BackColorAdjustments();
            ForeColorAdjustments();
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
            materialLabel2.BackColor = Color.White;

            materialLabel11.BackColor = Color.White;

            materialLabel3.BackColor = Color.White;
            materialLabel6.BackColor = Color.White;
            materialLabel8.BackColor = Color.White;
            materialLabel4.BackColor = Color.White;

            panel12.BackColor = Color.LightCoral;

            materialLabel7.BackColor = Color.LightCoral;
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

            materialLabel2.ForeColor = myCrimson;
            materialLabel1.ForeColor = Color.Gray;

            materialLabel3.ForeColor = myDrakGray;
            materialLabel6.ForeColor = myDrakGray;
            materialLabel8.ForeColor = myDrakGray;

            materialLabel7.ForeColor = Color.White;
        }

        
    }

}
