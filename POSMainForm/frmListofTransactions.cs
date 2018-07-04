using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;

namespace POSMainForm
{
    public partial class frmListofTransactions : Form
    {
     //   private frmReturn frmReturn = null;
        public frmListofTransactions(Form callForm)
        {
            InitializeComponent();
          //  frmReturn = callForm as frmReturn;
           
        }

        private void LoadInvoices(DateTime startDate, DateTime endDate, string searchString)
        {
           
            try
            {
                SQLConn.sqL = "SELECT STR_TO_DATE(REPLACE(TDate, '-', '/'), '%m/%d/%Y') as TDate, InvoiceNo, CONCAT(lastname, ', ', firstname, ' ', MI) as StaffName FROM Transactions T INNER JOIN staff S ON T.StaffID = S.StaffID WHERE STR_TO_DATE(REPLACE(TDATE, '-', '/'), '%m/%d/%Y') BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.ToString("yyyy-MM-dd") + "' AND InvoiceNo LIKE '%" + txtName.Text + "%' ORDER BY TDATE, InvoiceNo Desc";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SQLConn.dr.Read() == true)
                {
                    x = new ListViewItem(Strings.Format(SQLConn.dr["TDate"], "MM/dd/yyyy"));
                    x.SubItems.Add(SQLConn.dr["InvoiceNo"].ToString());
                    x.SubItems.Add(SQLConn.dr["StaffName"].ToString());
                   
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

        private void frmListofTransactions_Load(object sender, EventArgs e)
        {
            LoadInvoices(dtStartDate.Value, dtEndDate.Value, txtName.Text);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            LoadInvoices(dtStartDate.Value, dtEndDate.Value, txtName.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            LoadInvoices(dtStartDate.Value, dtEndDate.Value, txtName.Text);
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            LoadInvoices(dtStartDate.Value, dtEndDate.Value, txtName.Text);
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
                      
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
              
                this.Close();
            }
            catch { }
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
              
            }
            catch { }
        }

    }
}
