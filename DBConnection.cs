using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace POS_System_Proj_ni_Kelvs
{
    public class DBConnection
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public string MyConnection()
        {
            string con = @"Data Source=KELVS\SQLEXPRESS;Initial Catalog=POS_DB;Integrated Security=True";
            return con;
        }

        public double GetVal()
        {
            double vat = 0;
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("select * from tblVat", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                vat = Double.Parse(dr["vat"].ToString());
            }
            dr.Close();
            cn.Close();
            return vat;
        }
    }
}
