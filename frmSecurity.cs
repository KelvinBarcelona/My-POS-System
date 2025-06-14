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
    public partial class frmSecurity : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public frmSecurity()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtPass.Clear();
            txtUser.Clear();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = "", role = "", name = "";
            try
            {
                bool found = false;
                cn.Open();
                cm = new SqlCommand("select * from tbluser where username = @username and password = @password", cn);
                cm.Parameters.AddWithValue("@username", txtUser.Text);
                cm.Parameters.AddWithValue("@password", txtPass.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;
                    username = dr["username"].ToString();
                    role = dr["role"].ToString();
                    name = dr["name"].ToString();
                }
                else
                {
                    found = false;
                }
                dr.Close();
                cn.Close();

                if (found == true)
                {

                    if (role == "Cashier")
                    {
                        MessageBox.Show("Welcome " + name + "!", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPass.Clear();
                        txtUser.Clear();
                        this.Hide();
                        frmPOS frm = new frmPOS(this);
                        frm.lblUser.Text = username;
                        frm.lblName.Text = name + " | " + role;
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Welcome " + name + "!", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPass.Clear();
                        txtUser.Clear();
                        this.Hide();
                        Form1 frm = new Form1();
                        frm.lblName.Text = name;
                        frm.lblRole.Text = role;
                        frm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
