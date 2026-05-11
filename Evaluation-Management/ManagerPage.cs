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
        public ManagerPage()
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
        }
        private void BackColorAdjustments()
        {
            panel1.BackColor = Color.White;
            panel8.BackColor = Color.White;
            panel5.BackColor = Color.White;
            panel6.BackColor = Color.White;


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

            materialLabel7.ForeColor = myCrimson;
            materialLabel18.ForeColor = myCrimson;

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
        }
    }
}
