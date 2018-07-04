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
    public partial class frmListProductStocksIn : Form
    {
        int productID;
        public frmListProductStocksIn(int prodID)
        {
            InitializeComponent();
            productID = prodID;
        }

        private void GetProductInfo()
        {
           
            try
            {

                SQLConn.sqL = "SELECT ProductCode, Description, UnitPrice, StocksOnHand FROM Product WHERE ProductNo =" + productID + "";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {
                    lblProductCode.Text = SQLConn.dr[0].ToString();
                    lblDescription.Text = SQLConn.dr[1].ToString();
                    lblPrice.Text = Strings.FormatNumber(SQLConn.dr[2]).ToString();
                    lblCurrentStocks.Text = SQLConn.dr[3].ToString();
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

        private void AddStockIn()
        {
            try
            {
                SQLConn.sqL = "INSERT INTO StockIn(ProductNo, Quantity, DateIn) Values('" + productID + "', '" + txtQuantity.Text + "', '" + System.DateTime.Now.ToString("MM/dd/yyyy") + "')";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Stocks successfully added.", MsgBoxStyle.Information, "Add Stocks");
                UpdateProductQuantity();
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

        private void UpdateProductQuantity()
        {
            try
            {
                SQLConn.sqL = "UPDATE Product SET StocksOnhand = StocksOnHand + '" + Conversion.Val(txtQuantity.Text.Replace(",", "")) + "' WHERE ProductNo = '" + productID + "'";
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

        private void frmListProductStocksIn_Load(object sender, EventArgs e)
        {
            GetProductInfo();
            txtQuantity.Text = "";
            txtTotalStocks.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddStockIn();
            if (System.Windows.Forms.Application.OpenForms["frmListProduct"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["frmListProduct"] as frmListProduct).LoadProducts("");
            }

            this.Close();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            txtTotalStocks.Text = Strings.Format(Conversion.Val(lblCurrentStocks.Text) + Conversion.Val(txtQuantity.Text), "#,##0.00");
        }
    }
}
