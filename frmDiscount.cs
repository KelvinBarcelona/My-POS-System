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
    public partial class frmDiscount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        string stitle = "POS System";
        frmPOS f;
        public frmDiscount(frmPOS frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtPercent_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double discount = Double.Parse(txtPrice.Text) * Double.Parse(txtPercent.Text);
                txtAmount.Text = discount.ToString("#,##0.00");
            }
            catch(Exception ex)
            {
                txtAmount.Text = "0.00";    
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Add discount? Click yes to confirm.", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblcart set disc = @disc where id = @id", cn);
                    cm.Parameters.AddWithValue("@disc", Double.Parse(txtAmount.Text));
                    cm.Parameters.AddWithValue("@id", int.Parse(lblID.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    f.LoadCart();
                    this.Dispose();
                }
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
