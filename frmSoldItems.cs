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
    public partial class frmSoldItems : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public string kelvs;
        
        public frmSoldItems()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            dt1.Value = DateTime.Today;
            dt2.Value = DateTime.Today;
            LoadRecord();
            LoadCashier();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadRecord()
        {
            int i = 0;
            double total = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            string startDate = dt1.Value.ToString("MM-dd-yyyy");
            string endDate = dt2.Value.AddDays(1).ToString("MM-dd-yyyy");
            if (cbxCashier.Text == "All Cashier") { cm = new SqlCommand("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total from tblcart as c inner join tblproduct as p on c.pcode = p.pcode where status like 'Sold' and sdate >= '" + startDate + "' and sdate < '" + endDate + "'", cn); }
            else { cm = new SqlCommand("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total from tblcart as c inner join tblproduct as p on c.pcode = p.pcode where status like 'Sold' and sdate >= '" + startDate + "' and sdate < '" + endDate + "' and cashier like '" + cbxCashier.Text + "'", cn); }
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                total += double.Parse(dr["total"].ToString());
                dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();
            lblTotal.Text = total.ToString("#,##0.00");
        }

        private void dt1_ValueChanged(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void dt2_ValueChanged(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmReportSold frm = new frmReportSold(this);
            frm.LoadReport();
            frm.ShowDialog();
        }

        private void cbxCashier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void LoadCashier()
        {
            cbxCashier.Items.Clear();
            cbxCashier.Items.Add("All Cashier");
            cn.Open();
            cm = new SqlCommand("select * from tbluser where role like 'Cashier'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cbxCashier.Items.Add(dr["username"].ToString());
            }
            dr.Close() ;
            cn.Close() ;
        }

        private void cbxCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecord() ;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "colCancel")
            {
                frmCancelDetails f = new frmCancelDetails(this);
                f.txtID.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                f.txtTransNo.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                f.txtPCode.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                f.txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                f.txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                f.txtQty.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                f.txtDiscount.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                f.txtTotal.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                f.txtCancel.Text = kelvs;
                f.ShowDialog();
            }
        }
    }
}
