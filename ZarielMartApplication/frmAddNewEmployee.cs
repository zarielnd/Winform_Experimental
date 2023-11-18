
ing System;
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
    public partial class frmAddNewEmployee : Form
    {
        DBContext DBContext = new DBContext();
        public frmAddNewEmployee()
        {
            InitializeComponent();
        }

        private void frmAddNewEmployee_Load(object sender, EventArgs e)
        {
            lblEmployeeID.Visible = false;
            btnDelete.Visible = false;
            btnUpdate.Visible = false;
            btnAdd.Visible = true;
            bindEmployee();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtEmpName.Text == String.Empty)
            {
                MessageBox.Show("Please enter Employee Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmpName.Focus();
                return;
            }

            else if (txtEmpPassword.Text == String.Empty)
            {
                MessageBox.Show("Please enter Employee Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmpPassword.Focus();
                return;
            }
            else
            {
                String SQLString1 = "select [Name] from tblEmployee where [Name] = @EmpName";
                SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                cmd1.Parameters.AddWithValue("@EmpName", txtEmpName.Text);
                DBContext.openConnection();
                var result = cmd1.ExecuteScalar();

                if (result != null)
                {
                    MessageBox.Show(String.Format("Employee Name {0} already exist", txtEmpName.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClear();
                }
                else
                {
                    String SQLString2 = "spEmpInsert";
                    SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                    cmd2.Parameters.AddWithValue("@EmpName", txtEmpName.Text);
                    cmd2.Parameters.AddWithValue("@EmpAge", txtEmpAge.Text);
                    cmd2.Parameters.AddWithValue("@EmpPhone", Convert.ToInt32(txtEmpPhone.Text));
                    cmd2.Parameters.AddWithValue("@EmpPassword", txtEmpPassword.Text);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    int i = cmd2.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Employee Inserted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        bindEmployee();
                    }
                }
                DBContext.closeConnection();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblEmployeeID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Employee Id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtEmpName.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Employee Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmpName.Focus();
                    return;
                }

                else if (txtEmpPassword.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Employee Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmpPassword.Focus();
                    return;
                }
                else
                {
                    String SQLString1 = "select [Name] from tblEmployee where [Name]=@EmpName and ID != @EmpID";
                    SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                    cmd1.Parameters.AddWithValue("@EmpName", txtEmpName.Text);
                    cmd1.Parameters.AddWithValue("@EmpID", Convert.ToInt32(lblEmployeeID.Text));
                    DBContext.openConnection();
                    var result = cmd1.ExecuteScalar();

                    if (result != null)
                    {
                        MessageBox.Show(String.Format("Employee Name {0} already exist", txtEmpName.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClear();
                    }
                    else
                    {
                        String SQLString2 = "spEmpUpdate";
                        SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                        cmd2.Parameters.AddWithValue("@EmpID", Convert.ToInt32(lblEmployeeID.Text));
                        cmd2.Parameters.AddWithValue("@EmpName", txtEmpName.Text);
                        cmd2.Parameters.AddWithValue("@EmpAge", txtEmpAge.Text);
                        cmd2.Parameters.AddWithValue("@EmpPhone", Convert.ToInt32(txtEmpPhone.Text));
                        cmd2.Parameters.AddWithValue("@EmpPassword", txtEmpPassword.Text);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        int i = cmd2.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Employee Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindEmployee();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblEmployeeID.Visible = false;
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Employee Updated Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                    }
                    DBContext.closeConnection();
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
                if (lblEmployeeID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Employee Id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("Confirm Deletion?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        String SQLString2 = "spEmpDelete";
                        SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                        cmd2.Parameters.AddWithValue("@EmpID", Convert.ToInt32(lblEmployeeID.Text));
                        cmd2.CommandType = CommandType.StoredProcedure;
                        DBContext.openConnection();
                        int i = cmd2.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Employee Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindEmployee();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblEmployeeID.Visible = false;
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Employee Delete Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                        DBContext.closeConnection();
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bindEmployee()
        {
            String SQLString = "select * from tblEmployee";
            SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
            DBContext.openConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
            DBContext.closeConnection();
        }

        private void txtClear()
        {
            txtEmpName.Clear();
            txtEmpAge.Clear();
            txtEmpPassword.Clear();
            txtEmpPhone.Clear();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblEmployeeID.Visible = true;
            btnAdd.Visible = false;

            lblEmployeeID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtEmpName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtEmpAge.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtEmpPhone.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtEmpPassword.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }
    }
}
