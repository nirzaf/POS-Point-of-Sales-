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
    public partial class frmReportFilterDailySales : Form
    {
        public frmReportFilterDailySales()
        {
            InitializeComponent();
        }

        private void rbUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUser.Checked == true)
            {
                rbInvoice.Checked = false;
            }
        }

        private void rbInvoice_CheckedChanged(object sender, EventArgs e)
        {
            if (rbInvoice.Checked == true)
            {
                rbUser.Checked = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((rbUser.Checked == false) && (rbInvoice.Checked == false))
            {
                Interaction.MsgBox("Please select report by User or Invoice No",MsgBoxStyle.Information, "Select Report");
                return;
            }


            if (rbUser.Checked == true)
            {
                frmReportDailSalesByUser R = new frmReportDailSalesByUser(DateTimePicker1.Value);
                R.Show();
            }
            else
            {
               
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
