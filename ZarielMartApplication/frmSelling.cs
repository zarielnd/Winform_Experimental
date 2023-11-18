using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZarielMartApplication.ADO;

namespace ZarielMartApplication
{
    public partial class frmSelling : Form
    {
        DBContext DBContext = new DBContext();
        public frmSelling()
        {
            InitializeComponent();
        }
        double GrandTotal = 0.0;
        int n = 0;
        private void frmSelling_Load(object sender, EventArgs e)
        {
            bindCategory();
            lblDate.Text = DateTime.Now.ToShortDateString();
            bindBill();
        }

        private void bindBill()
        {
            try
            {
                String SQLString = "select * from tblBill";
                SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
                DBContext.openConnection();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                dgvBill.DataSource = dt;
                DBContext.closeConnection();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bindCategory()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bindSearchedProductList()
        {
            try
            {
                String SQLString = "spGetAllProductList_SearchByCat";
                SqlCommand cmd = new SqlCommand(SQLString, DBContext.getConnection());
                cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                DBContext.openConnection();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                dgvProduct.DataSource = dt;

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

        private void dgvProduct_Click(object sender, EventArgs e)
        {
            txtProductID.Clear();
            txtProductID.Text = dgvProduct.SelectedRows[0].Cells[0].Value.ToString();
            txtProductName.Clear();
            txtProductName.Text = dgvProduct.SelectedRows[0].Cells[1].Value.ToString();
            //cmbCategory.SelectedValue = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtPrice.Clear();
            txtPrice.Text = dgvProduct.SelectedRows[0].Cells[4].Value.ToString();
            //txtQuantity.Text = dgvProduct.SelectedRows[0].Cells[5].Value.ToString();
            txtQuantity.Clear();
            txtQuantity.Focus();
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQuantity.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter valid quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    double Total = Convert.ToDouble(txtPrice.Text) * Convert.ToDouble(txtQuantity.Text);
                    DataGridViewRow addrow = new DataGridViewRow();
                    addrow.CreateCells(dgvOrder);
                    addrow.Cells[0].Value = ++n;
                    addrow.Cells[1].Value = txtProductName.Text;
                    addrow.Cells[2].Value = txtPrice.Text;
                    addrow.Cells[3].Value = txtQuantity.Text;
                    addrow.Cells[4].Value = Total;
                    dgvOrder.Rows.Add(addrow);
                    GrandTotal += Total;
                    lblGrandTotal.Text = "$" + GrandTotal.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvProduct.DataSource = null;
        }

        private void btnAddBill_Click(object sender, EventArgs e)
        {
            try
            {
                String SQLString2 = "spBillInsert";
                SqlCommand cmd2 = new SqlCommand(SQLString2, DBContext.getConnection());
                cmd2.Parameters.AddWithValue("@EmployeeID", Form1.loginName);
                cmd2.Parameters.AddWithValue("@SellDate", lblDate.Text);
                cmd2.Parameters.AddWithValue("@Total", Convert.ToDouble(GrandTotal));
                DBContext.openConnection();
                cmd2.CommandType = CommandType.StoredProcedure;
                int i = cmd2.ExecuteNonQuery();
                if (i > 0)
                {
                    MessageBox.Show("Product Insert Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bindBill();
                    dgvOrder.Rows.Clear();

                }
                DBContext.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|Excel 2007 (*.xls)|*.xls";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = Excel.DataGridView_To_Datatable(dgvBill);
                    dt.exportToExcel(openFileDialog.FileName);
                    MessageBox.Show("Data is exported!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
    public static class Excel
    {
        public static void exportToExcel(this System.Data.DataTable DataTable, string ExcelFilePath = null)
        {
            try
            {
                int ColumnsCount;
                if (DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();
                Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.ActiveSheet;
                object[] Header = new object[ColumnsCount];
                for (int i = 0; i < ColumnsCount; i++)
                    Header[i] = DataTable.Columns[i].ColumnName;
                Microsoft.Office.Interop.Excel.Range HeaderRange = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, ColumnsCount]));
                HeaderRange.Value = Header;
                HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                HeaderRange.Font.Bold = true;
                int RowsCount = DataTable.Rows.Count;
                object[,] Cells = new object[RowsCount, ColumnsCount];

                for (int j = 0; j < RowsCount; j++)
                    for (int i = 0; i < ColumnsCount; i++)
                        Cells[j, i] = DataTable.Rows[j][i];

                Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[RowsCount + 1, ColumnsCount])).Value = Cells;
                if (ExcelFilePath != null && ExcelFilePath != "")
                {
                    try
                    {
                        Worksheet.SaveAs(ExcelFilePath);
                        Excel.Quit();
                        System.Windows.Forms.MessageBox.Show("Excel file saved!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                          + ex.Message);
                    }
                }
                else  // no filepath is given
                {
                    Excel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }
        public static DataTable DataGridView_To_Datatable(DataGridView dg)
        {
            DataTable ExportDataTable = new DataTable();
            foreach (DataGridViewColumn col in dg.Columns)
            {
                ExportDataTable.Columns.Add(col.Name);
            }
            foreach (DataGridViewRow row in dg.Rows)
            {
                DataRow dRow = ExportDataTable.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                ExportDataTable.Rows.Add(dRow);
            }
            return ExportDataTable;
        }
    }
}
