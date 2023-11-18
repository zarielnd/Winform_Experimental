using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZarielMartApplication
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (Form1.loginName != null)
            {
                toolStripStatusLabel2.Text = Form1.loginName;
            }
            if(Form1.loginName != null && Form1.loginType=="Employee")
            {
                categoryToolStripMenuItem.Enabled = false;
                categoryToolStripMenuItem.ForeColor = Color.Red;
                productToolStripMenuItem.Enabled = false;
                productToolStripMenuItem.ForeColor = Color.Red;
                addUserToolStripMenuItem.Enabled = false;
                addUserToolStripMenuItem.ForeColor = Color.Red;
            }
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategory fcat = new frmCategory();
            fcat.Show();
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to close this application?","CLOSE", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if(dialog== DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Exit();
            }
        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewEmployee fem = new frmAddNewEmployee();
            fem.Show();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin frmAdmin = new frmAdmin();
            frmAdmin.Show();
        }

        private void manageProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProduct frmProduct = new frmProduct();  
            frmProduct.ShowDialog();
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSelling frmSelling = new frmSelling();
            frmSelling.ShowDialog();
        }
    }
}
