using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;

using MySql.Data.MySqlClient;

namespace POSMainForm
{
    public partial class frmAddEditProduct : Form
    {
        int productID;
        int categoryID;
        public frmAddEditProduct(int prodID)
        {
            InitializeComponent();
            productID = prodID;
        }

        private void GetProductNo()
        {
            try
            {
                SQLConn.sqL = "SELECT ProductNo FROM Product ORDER BY ProductNo DESC";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {
                    lblProductNo.Text = (Convert.ToInt32(SQLConn.dr["ProductNo"]) + 1).ToString();
                }
                else
                {
                    lblProductNo.Text = "1";
                }
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


        private void LoadUpdateCategory()
        {
            try
            {
                SQLConn.sqL = "SELECT ProductNo, ProductCode, P.Description, Barcode, P.CategoryNo, CategoryName, UnitPrice, StocksOnHand, ReorderLevel FROM Product as P LEFT JOIN Category as C ON P.CategoryNo = C.CategoryNo WHERE ProductNo = '" + productID + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {
                    lblProductNo.Text = SQLConn.dr["ProductNo"].ToString();
                    txtProductCode.Text = SQLConn.dr["ProductCode"].ToString();
                    txtDescription.Text = SQLConn.dr["Description"].ToString();
                    txtBarcode.Text = SQLConn.dr["Barcode"].ToString();
                    txtCategory.Text = SQLConn.dr["CategoryName"].ToString();
                    txtCategory.Tag = SQLConn.dr["CategoryNo"];
                    txtUnitPrice.Text = Strings.Format(SQLConn.dr["UnitPrice"], "#,##0.00");
                    txtStocksOnHand.Text = SQLConn.dr["StocksOnHand"].ToString();
                    txtReorderLevel.Text = SQLConn.dr["ReorderLevel"].ToString();
                }
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


        private void AddProducts()
        {
            string barcode = "";

            if (txtBarcode.Text.Trim() == "")
            {
                barcode = "NO BARCODE";
            }
            else
            {
                barcode = txtBarcode.Text;
            }

            try
            {
                SQLConn.sqL = "INSERT INTO Product(ProductCode, Description, Barcode, UnitPrice, StocksOnHand, ReorderLevel, CategoryNo) VALUES('" + txtProductCode.Text + "', '" + txtDescription.Text + "', '" + barcode + "', '" + txtUnitPrice.Text.Replace(",", "") + "', '" + txtStocksOnHand.Text.Replace(",", "") + "', '" + txtReorderLevel.Text + "', '" + categoryID + "')";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Product successfully added.", MsgBoxStyle.Information, "Add Product");
                AddStockIn();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }
        }

        private void AddStockIn()
        {
            try
            {
                SQLConn.sqL = "INSERT INTO StockIn(ProductNo, Quantity, DateIn) Values('" + lblProductNo.Text + "', '" + txtStocksOnHand.Text + "', '" + System.DateTime.Now.ToString("MM/dd/yyyy") + "')";
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

        private void UpdateProduct()
        {
            try
            {
                SQLConn.sqL = "UPDATE Product SET ProductCode = '" + txtProductCode.Text + "', Description = '" + txtDescription.Text + "', Barcode = '" + txtBarcode.Text.Trim() + "', UnitPrice = '" + txtUnitPrice.Text.Replace(",", "") + "', StocksOnHand = '" + txtStocksOnHand.Text.Replace(",", "") + "', ReorderLevel = '" + txtReorderLevel.Text + "', CategoryNo ='" + categoryID + "' WHERE ProductNo = '" + productID + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();

                Interaction.MsgBox("Product successfully Updated.", MsgBoxStyle.Information, "Update Product");
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

        private void ClearFields()
        {
            lblProductNo.Text = "";
            txtProductCode.Text = "";
            txtDescription.Text = "";
            txtBarcode.Text = "";
            txtCategory.Text = "";
            txtUnitPrice.Text = "";
            txtStocksOnHand.Text = "";
            txtReorderLevel.Text = "";
        }

        private void frmAddEditProduct_Load(object sender, EventArgs e)
        {
            if (SQLConn.adding == true)
            {
                lblTitle.Text = "Adding New Product";
                ClearFields();
                GetProductNo();
            }
            else
            {
                lblTitle.Text = "Updating Product";
                LoadUpdateCategory();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtCategory.Text == "")
            {
                Interaction.MsgBox("Please select category.", MsgBoxStyle.Information, "Category");
                return;
            }
            
            if (SQLConn.adding == true)
            {
                AddProducts();
            }
            else
            {
                UpdateProduct();
               
            }

            if (System.Windows.Forms.Application.OpenForms["frmListProduct"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["frmListProduct"] as frmListProduct).LoadProducts("");
            }

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            frmSelectCategory flc = new frmSelectCategory(this, false);
            flc.ShowDialog();
        }

        public string Category
        {
            get { return txtCategory.Text; }
            set { txtCategory.Text = value; }
        }

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }
    }
}
