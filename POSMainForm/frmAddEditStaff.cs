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
    public partial class frmAddEditStaff : Form
    {
        int LSStaffID;
        public frmAddEditStaff(int staffID)
        {
            
            InitializeComponent();
            LSStaffID = staffID;
        }

     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SQLConn.adding == true)
            {
                AddStaff();
            }
            else
            {
                UpdateStaff();
            }

            if (System.Windows.Forms.Application.OpenForms["frmListStaff"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["frmListStaff"] as frmListStaff).LoadStaffs("");
            }

            this.Close();
        }

        private void GetStaffID()
        {
            try
            {
                SQLConn.sqL = "SELECT StaffID FROM STAFF ORDER BY StaffID DESC";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {
                    lblProductNo.Text = (Convert.ToInt32(SQLConn.dr["StaffID"]) + 1).ToString();
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

        private void AddStaff()
        {
            try
            {
                SQLConn.sqL = "INSERT INTO STAFF(Lastname, Firstname, MI, Street, Barangay, City, Province, ContactNo, Username, Role, UPassword) VALUES('" + txtLastname.Text + "', '" + txtFirstname.Text + "', '" + txtMI.Text + "', '" + txtStreet.Text + "', '" + txtBarangay.Text + "', '" + txtCity.Text + "', '" + txtProvince.Text + "', '" + txtContractNo.Text + "', '" + txtUsername.Text + "', '" + txtRole.Text + "', '" + txtPassword.Text + "')";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("New staff successfully added.", MsgBoxStyle.Information, "Add Staff");
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

        private void UpdateStaff()
        {
            try
            {
                SQLConn.sqL = "Update STAFF SET Lastname = '" + txtLastname.Text + "', Firstname = '" + txtFirstname.Text + "', MI = '" + txtMI.Text + "', Street= '" + txtStreet.Text + "', Barangay = '" + txtBarangay.Text + "', City = '" + txtCity.Text + "', Province = '" + txtProvince.Text + "', ContactNo = '" + txtContractNo.Text + "', Username ='" + txtUsername.Text + "', Role = '" + txtRole.Text + "', UPassword = '" + txtPassword.Text + "' WHERE StaffID = '" + LSStaffID + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Staff record successfully updated", MsgBoxStyle.Information, "Update Staff");

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

        private void LoadUpdateStaff()
        {
            try
            {
                SQLConn.sqL = "SELECT * FROM STAFF WHERE StaffID = '" + LSStaffID + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {
                    lblProductNo.Text = SQLConn.dr["StaffID"].ToString();
                    txtLastname.Text = SQLConn.dr["lastname"].ToString();
                    txtFirstname.Text = SQLConn.dr["Firstname"].ToString();
                    txtMI.Text = SQLConn.dr["MI"].ToString();
                    txtStreet.Text = SQLConn.dr["Street"].ToString();
                    txtBarangay.Text = SQLConn.dr["barangay"].ToString();
                    txtCity.Text = SQLConn.dr["City"].ToString();
                    txtProvince.Text = SQLConn.dr["Province"].ToString();
                    txtContractNo.Text = SQLConn.dr["ContactNo"].ToString();
                    txtUsername.Text = SQLConn.dr["username"].ToString();
                    txtRole.Text = SQLConn.dr["Role"].ToString();
                    txtPassword.Text = SQLConn.dr["UPassword"].ToString();
                    txtConfirmPWD.Text = SQLConn.dr["UPassword"].ToString();

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

        private void ClearFields()
        {
            lblProductNo.Text = "";
            txtLastname.Text = "";
            txtFirstname.Text = "";
            txtMI.Text = "";
            txtStreet.Text = "";
            txtBarangay.Text = "";
            txtCity.Text = "";
            txtProvince.Text = "";
            txtContractNo.Text = "";
            txtUsername.Text = "";
            txtRole.Text = "";
            txtPassword.Text = "";
            txtConfirmPWD.Text = "";
        }

        private void frmAddEditStaff_Load(object sender, EventArgs e)
        {
            if (SQLConn.adding == true)
            {
                lblTitle.Text = "Adding New Staff";
                ClearFields();
                GetStaffID();
            }
            else
            {
                lblTitle.Text = "Updating Staff";
                LoadUpdateStaff();

            }
        }
    }
}
