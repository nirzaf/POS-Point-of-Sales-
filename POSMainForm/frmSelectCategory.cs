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
    public partial class frmSelectCategory : Form
    {
        private frmAddEditProduct frmProduct = null;
        private frmAddProductQuick frmProductQuick = null;
        bool IsQuickAdd;
        public frmSelectCategory(Form callingForm, bool isQuickAdd)
        {
            InitializeComponent();

            if (isQuickAdd != true)
            {
                frmProduct = callingForm as frmAddEditProduct;
            }
            else
            {
                frmProductQuick = callingForm as frmAddProductQuick;
            }
            
            
            IsQuickAdd = isQuickAdd;
        }

        private void LoadCategory()
        {
            try
            {
                SQLConn.sqL = "SELECT * FROM Category WHERE CategoryName LIKE '" + txtCatName.Text + "%' ORDER BY CategoryName ";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SQLConn.dr.Read() == true)
                {
                    x = new ListViewItem(SQLConn.dr["CategoryNo"].ToString());
                    x.SubItems.Add(SQLConn.dr["CategoryName"].ToString());
                    x.SubItems.Add(SQLConn.dr["Description"].ToString());

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

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSelectCategory_Load(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {

            int id = Convert.ToInt32(ListView1.FocusedItem.Text);

            if (IsQuickAdd != true)
            {
                this.frmProduct.CategoryID = id;
                this.frmProduct.Category = ListView1.FocusedItem.SubItems[1].Text;
            }
            else
            {
                this.frmProductQuick.CategoryID = id;
                this.frmProductQuick.Category = ListView1.FocusedItem.SubItems[1].Text;
            }
           
            this.Close();
        }

        
    }
}
