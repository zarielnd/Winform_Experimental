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
    public partial class frmProduct : Form
    {
        DBContext DBContext = new DBContext();
        public frmProduct()
        {
            InitializeComponent();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            bindCategory();
            bindProduct();
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            lblProductID.Visible = false;
            btnAdd.Visible = true;
            searchByCategory();
        }

        private void bindProduct()
        {
            try
            {
                String SQLString = "spGetAllProductList";
                SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                DBContext.openConnection();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                dataGridView1.DataSource = dt;

                DBContext.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void bindCategory()
        {
            String SQLString = "select * from tblCategory order by [Name] ASC";
            SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
            DBContext.openConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            cmbCategory.DataSource = dt;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "ID";
            DBContext.closeConnection();
        }
        private void searchByCategory()
        {
            String SQLString = "select * from tblCategory order by [Name] ASC";
            SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
            DBContext.openConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            cmbSearch.DataSource = dt;
            cmbSearch.DisplayMember = "Name";
            cmbSearch.ValueMember = "ID";
            DBContext.closeConnection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductName.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Product Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductName.Focus();
                    return;
                }

                else if (txtPrice.Text == String.Empty || Convert.ToInt32(txtPrice.Text) < 0)
                {
                    MessageBox.Show("Please enter valid Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                else if (txtQuantity.Text == String.Empty || Convert.ToInt32(txtQuantity.Text) < 0)
                {
                    MessageBox.Show("Please enter valid Quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQuantity.Focus();
                    return;
                }
                else
                {
                    String SQLString1 = "select [Name] from tblProduct where [Name]=@ProName";
                    SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                    cmd1.Parameters.AddWithValue("@ProName", txtProductName.Text);
                    DBContext.openConnection();
                    var result = cmd1.ExecuteScalar();

                    if (result != null)
                    {
                        MessageBox.Show(String.Format("Product Name {0} already exist", txtProductName.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClear();
                    }
                    else
                    {
                        String SQLString2 = "spProductInsert";
                        SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                        cmd2.Parameters.AddWithValue("@ProName", txtProductName.Text);
                        cmd2.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text));
                        cmd2.Parameters.AddWithValue("@CatID", Convert.ToInt32(cmbCategory.SelectedValue));
                        cmd2.CommandType = CommandType.StoredProcedure;
                        int i = cmd2.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Insert Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindProduct();
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

        private void txtClear()
        {
            txtProductName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            cmbCategory.SelectedIndex = 0;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductName.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Product Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductName.Focus();
                    return;
                }

                else if (txtPrice.Text == String.Empty || Convert.ToInt32(txtPrice.Text) < 0)
                {
                    MessageBox.Show("Please enter valid Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                else if (txtQuantity.Text == String.Empty || Convert.ToInt32(txtQuantity.Text) < 0)
                {
                    MessageBox.Show("Please enter valid Quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQuantity.Focus();
                    return;
                }
                else
                {
                    String SQLString1 = "select [Name] from tblProduct where [Name]=@ProName and ID != @ProID";
                    SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                    cmd1.Parameters.AddWithValue("@ProName", txtProductName.Text);
                    cmd1.Parameters.AddWithValue("@ProID", lblProductID.Text);
                    DBContext.openConnection();
                    var result = cmd1.ExecuteScalar();

                    if (result != null)
                    {
                        MessageBox.Show(String.Format("Product Name {0} already exist", txtProductName.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClear();
                    }
                    else
                    {
                        String SQLString2 = "spProductUpdate";
                        SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                        cmd2.Parameters.AddWithValue("@ProID", Convert.ToInt32(lblProductID.Text));
                        cmd2.Parameters.AddWithValue("@ProName", txtProductName.Text);
                        cmd2.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text));
                        cmd2.Parameters.AddWithValue("@CatID", Convert.ToInt32(cmbCategory.SelectedValue));
                        cmd2.CommandType = CommandType.StoredProcedure;
                        int i = cmd2.ExecuteNonQuery();

                        if (i > 0)
                        {
                            MessageBox.Show("Product Update Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindProduct();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblProductID.Visible = false;
                            btnAdd.Visible = true;
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
                if (txtProductName.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Product Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductName.Focus();
                    return;
                }

                else if (txtPrice.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                else if (txtQuantity.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQuantity.Focus();
                    return;
                }
                else
                {
                    //String SQLString1 = "select [Name] from tblProduct where [Name]=@ProName and ID != @ProID";
                    //SqlCommand cmd1 = new SqlCommand(SQLString1, DBContext.getConnection());
                    //cmd1.Parameters.AddWithValue("@ProName", txtProductName.Text);
                    //cmd1.Parameters.AddWithValue("@ProID", lblProductID.Text);
                    //DBContext.openConnection();
                    //var result = cmd1.ExecuteScalar();

                    //if (result != null)
                    //{
                    //    MessageBox.Show(String.Format("Product Name {0} already exist", txtProductName.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    txtClear();
                    //}
                    //else
                    //{
                    String SQLString2 = "spProductDelete";
                    SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                    cmd2.Parameters.AddWithValue("@ProID", Convert.ToInt32(lblProductID.Text));
                    cmd2.CommandType = CommandType.StoredProcedure;
                    DBContext.openConnection();
                    int i = cmd2.ExecuteNonQuery();

                    if (i > 0)
                    {
                        MessageBox.Show("Product Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        bindProduct();
                        btnUpdate.Visible = false;
                        btnDelete.Visible = false;
                        lblProductID.Visible = false;
                        btnAdd.Visible = true;
                    }
                    //}
                    DBContext.closeConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            bindProduct();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            lblProductID.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Visible = false;

            lblProductID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtProductName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            cmbCategory.SelectedValue = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtPrice.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            txtQuantity.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void bindSearchedProductList()
        {
            try
            {
                String SQLString = "spGetAllProductList_SearchByCat";
                SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
                cmd.Parameters.AddWithValue("@ProdCatID",cmbSearch.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                DBContext.openConnection();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(); 
                dataAdapter.Fill(dt);
                dataGridView1.DataSource = dt;

                DBContext.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            bindSearchedProductList();
        }
    }
}
