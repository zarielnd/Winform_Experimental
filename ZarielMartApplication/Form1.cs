using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZarielMartApplication.ADO;

namespace ZarielMartApplication
{
    public partial class Form1 : Form
    {
        DBContext DBContext = new DBContext();
        public static string loginName, loginType;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbRole.SelectedIndex = 1;
            txtUsername.Text = "admin";
            txtPassword.Text = "admin";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRole.SelectedIndex > 0)
                {
                    // validate
                    if (txtUsername.Text == String.Empty)
                    {
                        MessageBox.Show("Please enter valid username", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUsername.Focus();
                        return;
                    }

                    if (txtPassword.Text == String.Empty)
                    {
                        MessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Focus();
                        return;
                    }

                    //login code
                    if (cmbRole.Text == "Admin")
                    {
                        String SQLString = "Select * from tblAdmin where ID=@AdminID and [Password]=@Password";
                        SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
                        cmd.Parameters.AddWithValue("@AdminID", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                        DBContext.openConnection();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Login Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loginName = txtUsername.Text;
                            loginType = cmbRole.Text;
                            clrValue();
                            this.Hide();
                            frmMain frmMain = new frmMain();
                            frmMain.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Login Information, Please Check Username or Password Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (cmbRole.Text == "Employee")
                    {
                        String SQLString = "Select * from tblEmployee where [Name]=@EmployeeName and [Password]=@Password";
                        SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
                        cmd.Parameters.AddWithValue("@EmployeeName", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                        DBContext.openConnection();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Login Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loginName = txtUsername.Text;
                            loginType = cmbRole.Text;
                            clrValue();
                            this.Hide();
                            frmMain frmMain = new frmMain();
                            frmMain.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Login Information, Please Check Username or Password Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select any role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clrValue();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clrValue();
        }

        private void clrValue()
        {
            cmbRole.SelectedIndex = 0;
            txtUsername.Clear();
            txtPassword.Clear();
        }
    }

}
