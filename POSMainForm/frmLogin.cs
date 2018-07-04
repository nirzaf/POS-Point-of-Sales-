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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            SQLConn.getData();
            
        }

        private void Login()
        {
            try
            {
                SQLConn.sqL = "SELECT * FROM Staff WHERE Username = '" + txtusername.Text + "' AND UPassword = '" + txtPassword.Text + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {

                    if (SQLConn.dr["Role"].ToString().ToUpper() == "ADMIN")
                    {
                        frmMain m = new frmMain(SQLConn.dr["Username"].ToString(),Convert.ToInt32(SQLConn.dr["StaffID"]));
                        m.Show();
                        this.Hide();
                    }
                    else 
                    {
                       
                    }
                }
                else
                {
                    Interaction.MsgBox("Invalid Password. Please try again.",MsgBoxStyle.Exclamation, "Login");
                }

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

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void txtusername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                frmDatabaseConfig dc = new frmDatabaseConfig();
                dc.ShowDialog();
            }
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            Login();
        }

        
    }
}
