using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POS_System_Proj_ni_Kelvs
{
    public partial class frmBrandModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        frmBrand frmbrand;
        public frmBrandModule(frmBrand fbrand)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frmbrand = fbrand;
        }

        private void frmBrandModule_Load(object sender, EventArgs e)
        {

        }
        private void Clear()
        {
            btnSave.Enabled = true;
            btnCancel.Enabled = false;
            txtBrandName.Clear();
            txtBrandName.Focus();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure you want to save this brand?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("INSERT INTo tblBrand(Brand)VALUEs(@brand)", cn);
             
                    
                    cm.Parameters.AddWithValue("@brand", txtBrandName.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Record has been successfully saved.");
                    Clear();
                    frmbrand.LoadRecords();
                }
                
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure you want to update this brand?","Update Record",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblbrand set brand = @brand where id like '" + lblD.Text + "'", cn);
                    cm.Parameters.AddWithValue("@brand", txtBrandName.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Brand has been successfully updated.");
                    Clear();
                    frmbrand.LoadRecords();
                    this.Dispose();

                }
            }catch(Exception ex)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
