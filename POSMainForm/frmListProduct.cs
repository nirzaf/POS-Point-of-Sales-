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
    public partial class frmListProduct : Form
    {
        int productID;
        public frmListProduct()
        {
            InitializeComponent();
        }

        public void LoadProducts(string strSearch)
        {
            try
            {
                SQLConn.sqL = "SELECT ProductNo, ProductCOde, P.Description, Barcode, UnitPrice, StocksOnHand, ReorderLevel, CategoryName FROM Product as P LEFT JOIN Category C ON P.CategoryNo = C.CategoryNo WHERE P.Description LIKE  @Description OR P.Barcode LIKE @Barcode   ORDER BY Description";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.Parameters.AddWithValue("@Description", "%" + strSearch + "%");
                SQLConn.cmd.Parameters.AddWithValue("@Barcode", "%" + strSearch + "%");
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();


                while (SQLConn.dr.Read() == true)
                {
                    x = new ListViewItem(SQLConn.dr["ProductNo"].ToString());
                    x.SubItems.Add(SQLConn.dr["ProductCode"].ToString());
                    x.SubItems.Add(SQLConn.dr["Description"].ToString());
                    x.SubItems.Add(SQLConn.dr["Barcode"].ToString());
                    x.SubItems.Add(SQLConn.dr["CategoryName"].ToString());
                    x.SubItems.Add(Strings.Format(SQLConn.dr["UnitPrice"], "#,##0.00"));
                    x.SubItems.Add(SQLConn.dr["StocksOnHand"].ToString());
                    x.SubItems.Add(SQLConn.dr["ReOrderLevel"].ToString());

                    ListView1.Items.Add(x);
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            SQLConn.adding = true;
            SQLConn.updating = false;
            int init = 0;
            frmAddEditProduct aeP = new frmAddEditProduct(init);
            aeP.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ListView1.Items.Count == 0)
            {
                Interaction.MsgBox("Please select record to update", MsgBoxStyle.Exclamation, "Update");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(ListView1.FocusedItem.Text))
                {

                }
                else
                {
                    SQLConn.adding = false;
                    SQLConn.updating = true;
                    productID = Convert.ToInt32(ListView1.FocusedItem.Text);
                    frmAddEditProduct aeP = new frmAddEditProduct(productID);
                    aeP.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to update", MsgBoxStyle.Exclamation, "Update");
                return;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SQLConn.strSearch = Interaction.InputBox("ENTER PRODUCT NAME OR BARCODE.", "Search Product", " ");

            if (SQLConn.strSearch.Length >= 1)
            {
                LoadProducts(SQLConn.strSearch.Trim());
            }
            else if (string.IsNullOrEmpty(SQLConn.strSearch))
            {
                return;
            }
        }

        private void frmListProduct_Load(object sender, EventArgs e)
        {
            LoadProducts("");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStocksIn_Click(object sender, EventArgs e)
        {
            if (ListView1.Items.Count == 0)
            {
                Interaction.MsgBox("Please select record to add stock", MsgBoxStyle.Exclamation, "StocksIn");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(ListView1.FocusedItem.Text))
                {

                }
                else
                {
                    
                    productID = Convert.ToInt32(ListView1.FocusedItem.Text);
                    frmListProductStocksIn aeP = new frmListProductStocksIn(productID);
                    aeP.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to add stock", MsgBoxStyle.Exclamation, "StocksIn");
                return;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            frmImportProduct ip = new frmImportProduct();
            ip.ShowDialog();
        }
    }
}
