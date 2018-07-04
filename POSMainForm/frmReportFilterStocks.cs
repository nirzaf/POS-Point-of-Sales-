using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace POSMainForm
{
    public partial class frmReportFilterStocks : Form
    {
        public frmReportFilterStocks()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((rbStocksIn.Checked == false) && (rbStocksOut.Checked == false))
            {
                Interaction.MsgBox("Please select report by Stocks-In or Stocks-Out", MsgBoxStyle.Information, "Select Report");
                return;
            }

            if (rbStocksIn.Checked == true)
            {
                frmReportStocksIn rs = new frmReportStocksIn(dtpStartDate.Value, dtpEndDate.Value);
                rs.Show();
            }

            if (rbStocksOut.Checked == true)
            {
                frmReportStocksOut rs = new frmReportStocksOut(dtpStartDate.Value, dtpEndDate.Value);
                rs.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbStocksIn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbStocksIn.Checked == true)
            {
                rbStocksOut.Checked = false;
            }
        }

        private void rbStocksOut_CheckedChanged(object sender, EventArgs e)
        {
            if (rbStocksOut.Checked == true)
            {
                rbStocksIn.Checked = false;
            }
        }
    }
}
