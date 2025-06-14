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
    public partial class frmProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        frmProductList flist;
        string stitle = "POS System";
        public frmProduct(frmProductList frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            flist = frm;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCategory()
        {
            cbxCategory.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select category from tblcategory", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cbxCategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void LoadBrand()
        {
            cbxBrand.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select brand from tblbrand", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cbxBrand.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void frmProduct_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure you want to save this product?",stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string brandid = ""; string categoryid = "";
                    cn.Open();
                    cm = new SqlCommand("Select id from tblBrand where brand  like '" + cbxBrand.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows) { brandid = dr[0].ToString(); }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select id from tblCategory where category  like '" + cbxCategory.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows) { categoryid = dr[0].ToString(); }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblProduct (pcode, barcode, pdesc, brandid, categoryid, price, reorder) VALUES(@pcode, @barcode,@pdesc, @brandid, @categoryid, @price, @reorder)", cn);
                    cm.Parameters.AddWithValue("@pcode",txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@brandid", brandid);
                    cm.Parameters.AddWithValue("@categoryid", categoryid);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReorder.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product has been successfully saved.");
                    Clear();
                    flist.LoadRecords();
                }
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            txtPrice.Clear();
            txtPdesc.Clear();
            txtPcode.Clear();
            txtBarcode.Clear();
            cbxBrand.Text = "";
            cbxCategory.Text = "";
            txtPcode.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this product?", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string brandid = ""; string categoryid = "";
                    cn.Open();
                    cm = new SqlCommand("Select id from tblBrand where brand  like '" + cbxBrand.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows) { brandid = dr[0].ToString(); }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select id from tblCategory where category  like '" + cbxCategory.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows) { categoryid = dr[0].ToString(); }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("UPDATE tblProduct SET barcode=@barcode, pdesc=@pdesc, brandid=@brandid, categoryid=@categoryid, price=@price, reorder=@reorder where pcode like @pcode", cn);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@brandid", brandid);
                    cm.Parameters.AddWithValue("@categoryid", categoryid);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReorder.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product has been successfully updated.",stitle,MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    flist.LoadRecords();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46)
            {
                //accept . character
            } else if (e.KeyChar == 8) {
                //accept . backspace
            } else if ((e.KeyChar < 48) || (e.KeyChar > 57)) //ascii code 48-57 between 0-9
            {
                e.Handled = true;
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPcode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
