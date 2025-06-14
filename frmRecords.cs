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
    public partial class frmRecords : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public frmRecords()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadRecord()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            cn.Open();
            string startDate = dateTimePicker1.Value.ToString("MM-dd-yyyy");
            string endDate = dateTimePicker2.Value.AddDays(1).ToString("MM-dd-yyyy");
            cm = new SqlCommand("select top 10 pcode, pdesc, sum(qty) as qty from vwSoldItems where sdate between '" + startDate + "' and '" + endDate + "' and status like 'Sold' group by pcode, pdesc order by qty desc", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["qty"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.Clear();
                int i = 0;
                cn.Open();
                string startDate = dateTimePicker4.Value.ToString("MM-dd-yyyy");
                string endDate = dateTimePicker3.Value.AddDays(1).ToString("MM-dd-yyyy");
                cm = new SqlCommand("select c.pcode, p.pdesc, c.price, sum(c.qty) as tot_qty, sum(c.disc) as tot_disc, sum(c.total) as total from tblcart as c inner join tblproduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between '" + startDate + "' and '" + endDate + "' group by c.pcode, p.pdesc, c.price", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView2.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), Double.Parse(dr["price"].ToString()).ToString("#,##0.00"), dr["tot_qty"].ToString(), dr["tot_disc"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select isnull(sum(total),0) from tblcart where status like 'Sold' and sdate between '" + startDate + "' and '" + endDate + "'", cn);
                lblTotal.Text = Double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##.00");
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadCriticalItems()
        {
            try
            {
                dataGridView3.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select * from vwCriticalItems", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView3.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
                cn.Close();
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
