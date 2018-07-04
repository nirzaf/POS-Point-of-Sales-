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
    public partial class frmSystemSetting : Form
    {

        bool isAddingVat;

        public frmSystemSetting()
        {
            InitializeComponent();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSystemSetting_Load(object sender, EventArgs e)
        {
            //Company Setting
            txtName.Tag = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPhoneNo.Text = "";
            txtEmail.Text = "";
            txtWebsite.Text = "";
            txtTINNumber.Text = "";
            GetCompanyInfo();

            //VAT Setting
            txtPercent.Text = "";
            GetVATInfo();

        }
#region "Company"

     
        public void AddEditCompany(bool isAdding)
        {
            try
            {
                if (isAdding == true)
                {
                    SQLConn.sqL = "INSERT INTO Company(Name, Address, PhoneNo, Email, Website, TINNumber) VALUES(@Name, @Address, @PhoneNo, @Email, @Website, @TINNumber)";
                }
                else
                {
                    SQLConn.sqL = "UPDATE Company SET Name=@Name, Address=@Address, PhoneNo=@PhoneNO, Email=@Email, Website=@Website, TINNumber =@TINNumber WHERE CompanyID=@CompanyID ";
                }
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);

                SQLConn.cmd.Parameters.AddWithValue("@Name", txtName.Text);
                SQLConn.cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                SQLConn.cmd.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text);
                SQLConn.cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                SQLConn.cmd.Parameters.AddWithValue("@Website", txtWebsite.Text);
                SQLConn.cmd.Parameters.AddWithValue("@TINNumber", txtTINNumber.Text);
                
                if (isAdding == false)
                {
                    SQLConn.cmd.Parameters.AddWithValue("@CompanyID", txtName.Tag);
                }

                int i = SQLConn.cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    if (isAdding == true)
                    {
                        Interaction.MsgBox("Company Information Successfully Added", MsgBoxStyle.Information, "Adding Company");
                    }
                    else
                    {
                        Interaction.MsgBox("Company Information Successfully Updated", MsgBoxStyle.Information, "Editing Company");
                    }
                }
                else
                {
                    Interaction.MsgBox("Saving Company Information Failed", MsgBoxStyle.Exclamation, "Failed");
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

        public void GetCompanyInfo()
        {
            try
            {
                SQLConn.sqL = "SELECT CompanyID, Name, Address, PhoneNo, Email, Website, TINNumber FROM Company";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();


                if (SQLConn.dr.Read())
                {
                    txtName.Tag = SQLConn.dr[0].ToString();
                    txtName.Text = SQLConn.dr[1].ToString();
                    txtAddress.Text = SQLConn.dr[2].ToString();
                    txtPhoneNo.Text = SQLConn.dr[3].ToString();
                    txtEmail.Text = SQLConn.dr[4].ToString();
                    txtWebsite.Text = SQLConn.dr[5].ToString();
                    txtTINNumber.Text = SQLConn.dr[6].ToString();
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

        private bool IsAdding()
        {
            bool ret = false;
            try
            {
                SQLConn.sqL = "SELECT * FROM Company";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();
                if (SQLConn.dr.Read() == true)
                {
                    ret = false;
                }
                else
                {
                    ret = true;
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

        private void button3_Click(object sender, EventArgs e)
        {
            AddEditCompany(IsAdding());
        }

#endregion

#region "VAT"

        public void AddEditVAT(bool isAdding)
        {
            try
            {
            
                if (isAdding == true)
                {
                    SQLConn.sqL = "INSERT INTO VATSetting(VatPercent) VALUES(@VatPercent)";
                }
                else
                {
                    SQLConn.sqL = "UPDATE VATSetting SET VatPercent=@VatPercent WHERE VATID=@VATID ";
                }
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);

                SQLConn.cmd.Parameters.AddWithValue("@VatPercent", txtPercent.Text);
               
                if (isAdding == false)
                {
                    SQLConn.cmd.Parameters.AddWithValue("@VATID", txtPercent.Tag);
                }

                int i = SQLConn.cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    if (isAdding == true)
                    {
                        Interaction.MsgBox("VAT Information Successfully Added", MsgBoxStyle.Information, "Adding VAT");
                    }
                    else
                    {
                        Interaction.MsgBox("VAT Information Successfully Updated", MsgBoxStyle.Information, "Editing VAT");
                    }

                }
                else
                {
                    Interaction.MsgBox("Saving VAT Information Failed", MsgBoxStyle.Exclamation, "Failed");
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

        public void GetVATInfo()
        {
            try
            {
                SQLConn.sqL = "SELECT VATID, VatPercent FROM VatSetting";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();


                if (SQLConn.dr.Read())
                {
                    txtPercent.Tag = SQLConn.dr[0];
                    txtPercent.Text = SQLConn.dr[1].ToString();
                    isAddingVat = false;
                }
                else
                {
                    isAddingVat = true;
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


#endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPercent.Text == "")
            {
                AddEditVAT(isAddingVat);
            }
            else
            {
                AddEditVAT(isAddingVat);
            }
        }

    }
}
