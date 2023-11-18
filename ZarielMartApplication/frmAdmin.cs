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
    public partial class frmAdmin : Form
    {
        DBContext DBContext = new DBContext();
        public frmAdmin()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAdminId.Text == String.Empty || txtAdminName.Text == String.Empty || txtAdminPassword.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Valid Admin Id, Admin Name and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtClear();
            }
            else
            {
                String SQLString1 = "select [ID] from tblAdmin where [ID] = @ID";
                SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                cmd1.Parameters.AddWithValue("@ID", lblAdminID.Text);
                DBContext.openConnection();
                var result = cmd1.ExecuteScalar();
                if (result != null)
                {
                    MessageBox.Show(String.Format("Admin ID {0} already exist", lblAdminID.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClear();
                }
                else
                {
                    String SQLString2 = "spAdminInsert";
                    SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                    cmd2.Parameters.AddWithValue("@AdminID", txtAdminId.Text);
                    cmd2.Parameters.AddWithValue("@AdminPass", txtAdminPassword.Text);
                    cmd2.Parameters.AddWithValue("@AdminName", txtAdminName.Text);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    int i = cmd2.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Employee Inserted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        bindAdmin();
                    }
                }
            }
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = false;
            btnUpdate.Visible = false;
            lblAdminID.Visible = false;
            btnAdd.Visible = true;
            bindAdmin();
        }

        private void txtClear()
        {
            txtAdminId.Clear();
            txtAdminName.Clear();
            txtAdminPassword.Clear();
            txtAdminId.Focus();
        }

        private void bindAdmin()
        {
            String SQLString = "select * from tblAdmin";
            SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
            DBContext.openConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
            DBContext.closeConnection();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminId.Text == String.Empty || txtAdminName.Text == String.Empty || txtAdminPassword.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Valid Admin Id, Admin Name and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClear();
                }
                else
                {
                    String SQLString1 = "select [ID] from tblAdmin where [ID] = @ID";
                    SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                    cmd1.Parameters.AddWithValue("@ID", lblAdminID.Text);
                    DBContext.openConnection();
                    var result = cmd1.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show(String.Format("Admin ID {0} already exist", lblAdminID.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClear();
                    }
                    else
                    {
                        String SQLString2 = "spAdminUpdate";
                        SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                        cmd2.Parameters.AddWithValue("@AdminID", lblAdminID.Text);
                        cmd2.Parameters.AddWithValue("@AdminPass", txtAdminPassword.Text);
                        cmd2.Parameters.AddWithValue("@AdminName", Convert.ToInt32(txtAdminName.Text));
                        cmd2.CommandType = CommandType.StoredProcedure;
                        int i = cmd2.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Employee Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtAdminId.ReadOnly = false;
                            txtClear();
                            bindAdmin();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblAdminID.Visible = false;
                            btnAdd.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminId.Text == String.Empty || txtAdminName.Text == String.Empty || txtAdminPassword.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Valid Admin Id, Admin Name and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClear();
                }
                else
                {
                    String SQLString1 = "select [ID] from tblAdmin where [ID] = @ID";
                    SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                    cmd1.Parameters.AddWithValue("@ID", lblAdminID.Text);
                    DBContext.openConnection();
                    var result = cmd1.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show(String.Format("Admin ID {0} already exist", lblAdminID.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClear();
                    }
                    else
                    {
                        String SQLString2 = "spAdminDelete";
                        SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                        cmd2.Parameters.AddWithValue("@AdminID", lblAdminID.Text);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        int i = cmd2.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Employee Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtAdminId.ReadOnly = false;
                            txtClear();
                            bindAdmin();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblAdminID.Visible = false;
                            btnAdd.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            txtAdminId.ReadOnly = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Visible = false;

            lblAdminID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtAdminId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtAdminPassword.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtAdminName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }
    }
}
