using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;

namespace POSMainForm
{
    public partial class frmImportProduct : Form
    {
        public frmImportProduct()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;  

            OpenFileDialog file = new OpenFileDialog();
            file.CheckFileExists = true; 
            file.AddExtension = true;
            file.Multiselect = true;
            file.Filter = "Excel files (*.xlsx)|*.xlsx;*.xls";

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = file.FileName; //get the path of the file 
                lblFilePath.Text = filePath;
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt); //read excel file  
                    
                        dataGridView1.DataSource = dtExcel;
                        //dataGridView1.Refresh();
                        GroupBox1.Text = dtExcel.Rows.Count.ToString("N0") + " Products to be imported.";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                }  
            }
        }
        private String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;
            string connString = "";

            try
            {
                // Connection String. Change the excel file to the file you
                // will search.
                if (excelFile.CompareTo(".xls") == 0)
                    connString = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFile + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                else
                    connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFile + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }

                // Loop through all of the sheets if you want too...
                for (int j = 0; j < excelSheets.Length; j++)
                {
                    // Query each excel sheet.
                }

                return excelSheets;
            }
            catch 
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }


        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HRD=Yes;IMEX=1';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    string sheetName = GetExcelSheetNames(fileName)[0];
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [" + sheetName + "] Order by Description", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            return dtexcel;
        }

        public void ImportProducts()
        {
            try
            {
               
                int importCount = 0;
                int duplicateCount = 0 ;
                for (int i = 0; i <= dataGridView1.Rows.Count - 2; i++)
                {

                    if ((dataGridView1.Rows[i].Cells[0].Value.ToString() != "") && dataGridView1.Rows[i].Cells[1].Value.ToString() != "")
                    {
                        int catID = 0;
                        if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                        {
                             catID = GetCategoryId(dataGridView1.Rows[i].Cells[5].Value.ToString());

                            if (catID <= 0)
                            {
                                AddCategory(dataGridView1.Rows[i].Cells[5].Value.ToString());
                                catID = GetCategoryId(dataGridView1.Rows[i].Cells[5].Value.ToString());
                            }
                        }
                        string productCode = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string productName = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        string barcode = dataGridView1.Rows[i].Cells[2].Value.ToString();

                        double unitPrice = 0;

                        if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")
                        {
                            unitPrice = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        }

                        int stocks=0;
                        if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")
                        {
                            stocks = Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value.ToString());
                        }
                       
                        int reOrderLevel =0;

                        if (dataGridView1.Rows[i].Cells[6].Value.ToString() != "")
                        {
                            reOrderLevel = Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value.ToString());
                        }

                        if (IsProductExsit(productCode, productName) == true)
                        {
                            duplicateCount++;
                        }
                        else
                        {
                            Addproducts(productCode, productName, barcode, unitPrice, stocks, catID, reOrderLevel);

                            if (stocks > 0)
                            {
                                AddStockIn(GetProductID(productCode, productName), stocks);
                            }
                            importCount++;
                        }
                    }
                }

                Interaction.MsgBox(importCount + " Products successfully imported. " + duplicateCount + " Duplicate product.", MsgBoxStyle.Information, "Import Product");
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
          
        }

        private bool IsProductExsit(string productCode, string productName)
        {

            bool ret = false;
            try
            {
                string strSql = "SELECT * FROM Product WHERE ProductCode= '" + productCode + "' AND Description ='" + productName + "'";
                using (MySqlConnection conn = new MySqlConnection("Server = '" + SQLConn.ServerMySQL + "';  " + "Port = '" + SQLConn.PortMySQL + "'; " + "Database = '" + SQLConn.DBNameMySQL + "'; " + "user id = '" + SQLConn.UserNameMySQL + "'; " + "password = '" + SQLConn.PwdMySQL + "'"))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(strSql, conn))
                    {

                        SQLConn.dr = cmd.ExecuteReader();
                        if (SQLConn.dr.Read())
                        {
                            ret = true;
                        }
                    }
                }

                return ret;
            }
            catch (Exception)
            {
                
            }

            return ret;
        }

        private int GetProductID(string productCode, string productName)
        {

            int ret = 0;
            try
            {
                string strSql = "SELECT ProductNo FROM Product WHERE ProductCode= '" + productCode + "' AND Description ='" + productName + "'";
                using (MySqlConnection conn = new MySqlConnection("Server = '" + SQLConn.ServerMySQL + "';  " + "Port = '" + SQLConn.PortMySQL + "'; " + "Database = '" + SQLConn.DBNameMySQL + "'; " + "user id = '" + SQLConn.UserNameMySQL + "'; " + "password = '" + SQLConn.PwdMySQL + "'"))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(strSql, conn))
                    {

                        SQLConn.dr = cmd.ExecuteReader();
                        if (SQLConn.dr.Read())
                        {
                            ret =Convert.ToInt32( SQLConn.dr[0]);
                        }
                    }
                }

                return ret;
            }
            catch (Exception)
            {

            }

            return ret;
        }

        private void AddStockIn(int productId, int quantity)
        {
            try
            {
                SQLConn.sqL = "INSERT INTO StockIn(ProductNo, Quantity, DateIn) Values('" + productId + "', '" + quantity + "', '" + System.DateTime.Now.ToString("MM/dd/yyyy") + "')";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
               
               
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }
        }


        private int GetCategoryId(string category)
        {
            int ret = 0;
            try
            {
                string strSql = "SELECT CategoryNo FROM Category WHERE CategoryName= '" + category + "'";
                using (MySqlConnection conn = new MySqlConnection("Server = '" + SQLConn.ServerMySQL + "';  " + "Port = '" + SQLConn.PortMySQL + "'; " + "Database = '" + SQLConn.DBNameMySQL + "'; " + "user id = '" + SQLConn.UserNameMySQL + "'; " + "password = '" + SQLConn.PwdMySQL + "'"))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(strSql, conn))
                    {
                        SQLConn.dr = cmd.ExecuteReader();
                        if (SQLConn.dr.Read())
                        {
                            ret = Convert.ToInt32( SQLConn.dr[0]);
                        }
                    }
                }
                return ret;
            }
            catch (Exception)
            {

            }

            return ret;
        }


        private void Addproducts(string productCode, string productName, string barcodeImport, double unitPrice, int stocks, int categoryNo, int reOrderLevel)
        {
            string barcode = "";

            if (barcodeImport.Trim() == "")
            {
                barcode = "NO BARCODE";
            }
            else
            {
                barcode = barcodeImport.Trim();
            }

            try
            {
                SQLConn.sqL = "INSERT INTO PRODUCT(ProductCode, Description, Barcode, UnitPrice, StocksOnhand, CategoryNo, ReOrderLevel) VALUES(@ProductCode, @Description, @Barcode, @UnitPrice, @StocksOnhand, @CategoryNo, @ReOrderLevel)";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.Parameters.AddWithValue("@ProductCode", productCode);
                SQLConn.cmd.Parameters.AddWithValue("@Description", productName);
                SQLConn.cmd.Parameters.AddWithValue("@Barcode", barcode);
                SQLConn.cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                SQLConn.cmd.Parameters.AddWithValue("@StocksOnhand", stocks);
                SQLConn.cmd.Parameters.AddWithValue("@CategoryNo", categoryNo);
                SQLConn.cmd.Parameters.AddWithValue("@ReOrderLevel", reOrderLevel);
                SQLConn.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }

        }


        private void AddCategory(string category) 
        {

            try
            {
                SQLConn.sqL = "INSERT INTO Category(CategoryName, Description) VALUES('" + category + "','" + category + "')";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }
        
        }


        
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0){
                ImportProducts();
                
                if (System.Windows.Forms.Application.OpenForms["frmListProduct"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["frmListProduct"] as frmListProduct).LoadProducts("");
                }

                this.Close();
            }
            else {
                Interaction.MsgBox("No Product to Import! Please select file to import.", MsgBoxStyle.Exclamation,"Import Product");
            }
        }
    }
}
