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
using Microsoft.Reporting.WinForms;

namespace POSMainForm
{
    public partial class frmReportDailSalesByUser : Form
    {
        DateTime ReportDate;
        public frmReportDailSalesByUser(DateTime reportDate)
        {
           
            InitializeComponent();

            ReportDate = reportDate;
        }

        private void frmReportDailSalesByUser_Load(object sender, EventArgs e)
        {

            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                if (InvoiceSetting() == 1)
                {
                    SQLConn.sqL = "SELECT CONCAT(Lastname, ', ', Firstname, ' ', MI) as StaffName, ProductCode, P.Description, REPLACE(TDate, '-', '/') as TDate, TTime,TD.ItemPrice, SUM(TD.Quantity) as totalQuantity, (TD.ItemPrice * SUM(TD.Quantity)) as TotalPrice  FROM Product as P, Transactions as T, TransactionDetails as TD, Staff as St WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND St.StaffID = T.StaffID AND  TDate = '" + ReportDate.ToString("MM/dd/yyyy") + "' AND T.Status != 1 GROUP BY  St.StaffID, P.ProductNo, TDate ORDER By TDate";
                }
                else
                {
                    SQLConn.sqL = "SELECT CONCAT(Lastname, ', ', Firstname, ' ', MI) as StaffName, ProductCode, P.Description, REPLACE(TDate, '-', '/') as TDate, TTime,TD.ItemPrice, SUM(TD.Quantity) as totalQuantity, (TD.ItemPrice * SUM(TD.Quantity)) as TotalPrice  FROM Product as P, Transactions as T, TransactionDetails as TD, Staff as St WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND St.StaffID = T.StaffID AND  TDate = '" + ReportDate.ToString("MM/dd/yyyy") + "' GROUP BY  St.StaffID, P.ProductNo, TDate ORDER By TDate";
                }
                
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.da = new MySqlDataAdapter(SQLConn.cmd);

                this.dsReportC.DailySalesByStaff.Clear();
                SQLConn.da.Fill(this.dsReportC.DailySalesByStaff);

                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomPercent = 90;
                this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;

                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
        }

        int InvoiceSetting()
        {
            int ret = 0;
            try
            {
                SQLConn.sqL = "SELECT HInvoice FROM Company";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();


                if (SQLConn.dr.Read() == true)
                {
                    ret = Convert.ToInt32(SQLConn.dr["HInvoice"]);
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

            return ret;
        }
    }
}
