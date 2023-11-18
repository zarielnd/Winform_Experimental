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
    public partial class frmCategory : Form
    {
        DBContext DBContext = new DBContext();
        public frmCategory()
        {
            InitializeComponent();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            lblCatID.Visible = false;
            bindCategory();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtCatName.Text == String.Empty)
            {
                MessageBox.Show("Please enter Category Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCatName.Focus();
                return;
            }

            else if (rtbCatDesc.Text == String.Empty)
            {
                MessageBox.Show("Please enter Caregory Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtbCatDesc.Focus();
                return;
            }
            else
            {
                String SQLString1 = "select [Name] from tblCategory where [Name]=@CatName";
                SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                cmd1.Parameters.AddWithValue("@CatName", txtCatName.Text);
                DBContext.openConnection();
                var result = cmd1.ExecuteScalar();

                if (result != null)
                {
                    MessageBox.Show(String.Format("Category Name {0} already exist", txtCatName.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClear();
                }
                else
                {
                    String SQLString2 = "spCatInsert";
                    SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                    cmd2.Parameters.AddWithValue("@CatName", txtCatName.Text);
                    cmd2.Parameters.AddWithValue("@CatDesc", rtbCatDesc.Text);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    int i = cmd2.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Category Inserted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        bindCategory();
                    }
                }
                DBContext.closeConnection();
            }
        }

        private void txtClear()
        {
            txtCatName.Clear();
            rtbCatDesc.Clear();
        }

        private void bindCategory()
        {
            String SQLString = "select * from tblCategory";
            SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
            DBContext.openConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;

            DBContext.closeConnection();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblCatID.Visible = true;
            btnAdd.Visible = false;

            lblCatID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtCatName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            rtbCatDesc.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblCatID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Category Id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtCatName.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Category Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCatName.Focus();
                    return;
                }

                else if (rtbCatDesc.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Caregory Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rtbCatDesc.Focus();
                    return;
                }
                else
                {
                    String SQLString1 = "select [Name] from tblCategory where [Name]=@CatName and ID != @CatID";
                    SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                    cmd1.Parameters.AddWithValue("@CatName", txtCatName.Text);
                    cmd1.Parameters.AddWithValue("@CatID", Convert.ToInt32(lblCatID.Text));
                    DBContext.openConnection();
                    var result = cmd1.ExecuteScalar();

                    if (result != null)
                    {
                        MessageBox.Show(String.Format("Category Name {0} already exist", txtCatName.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClear();
                    }
                    else
                    {
                        String SQLString2 = "spCatUpdate";
                        SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                        cmd2.Parameters.AddWithValue("@CatID", Convert.ToInt32(lblCatID.Text));
                        cmd2.Parameters.AddWithValue("@CatName", txtCatName.Text);
                        cmd2.Parameters.AddWithValue("@CatDesc", rtbCatDesc.Text);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        int i = cmd2.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindCategory();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblCatID.Visible = false;
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Category Updated Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                    }
                    DBContext.closeConnection();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblCatID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Category Id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if(DialogResult.Yes==MessageBox.Show("Confirm Deletion?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        String SQLString2 = "spCatDelete";
                        SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                        cmd2.Parameters.AddWithValue("@CatID", Convert.ToInt32(lblCatID.Text));
                        cmd2.CommandType = CommandType.StoredProcedure;
                        DBContext.openConnection();
                        int i = cmd2.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindCategory();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblCatID.Visible = false;
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Category Delete Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
