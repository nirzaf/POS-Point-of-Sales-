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
    public partial class frmAddEditCategory : Form
    {
        int categoryID;
        public frmAddEditCategory(int catID)
        {
            InitializeComponent();

            categoryID = catID;
        }

        private void GetCategoryNo()
        {
            try
            {
                SQLConn.sqL = "SELECT CategoryNo FROM Category ORDER BY CategoryNo DESC";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {
                    lblCategoryNo.Text = (Convert.ToInt32(SQLConn.dr["CategoryNo"]) + 1).ToString();
                }
                else
                {
                    lblCategoryNo.Text = "1";
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

        private void LoadUpdateCategory()
        {
            try
            {
                SQLConn.sqL = "SELECT * FROM Category WHERE CategoryNo = '" + categoryID + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {
                    lblCategoryNo.Text = SQLConn.dr["CategoryNo"].ToString();
                    txtCatName.Text = SQLConn.dr["CategoryName"].ToString();
                    txtDescription.Text = SQLConn.dr["Description"].ToString();
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

        private void AddCategory()
        {
            try
            {
                SQLConn.sqL = "INSERT INTO Category(CategoryName, Description) VALUES('" + txtCatName.Text + "', '" + txtDescription.Text + "')";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("New category successfully added.", MsgBoxStyle.Information, "Add Category");
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

        private void UpdateCategory()
        {
            try
            {
                SQLConn.sqL = "UPDATE Category SET CategoryName= '" + txtCatName.Text + "', Description = '" + txtDescription.Text + "' WHERE CategoryNo = '" + categoryID + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Category successfully updated.", MsgBoxStyle.Information, "Update Category");
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

        private void CLearFields()
        {
            lblCategoryNo.Text = "";
            txtCatName.Text = "";
            txtDescription.Text = "";
        }

        private void frmAddEditCategory_Load(object sender, EventArgs e)
        {
            if (SQLConn.adding == true)
            {
                lblTitle.Text = "Adding New Category";
                CLearFields();
                GetCategoryNo();
            }
            else
            {
                lblTitle.Text = "Updating Category";
                LoadUpdateCategory();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SQLConn.adding == true)
            {
                AddCategory();
            }
            else
            {
                UpdateCategory();
               
            }
            if (System.Windows.Forms.Application.OpenForms["frmListCategory"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["frmListCategory"] as frmListCategory).LoadCategories("");
            }

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
