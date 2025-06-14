using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;

namespace POS_System_Proj_ni_Kelvs
{
    public partial class frmReceipt : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        string store = "S & K Mart";
        string address = "Vibal St. Brgy 3, Bacacay, Albay";
        frmPOS f;
        public frmReceipt(frmPOS frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void frmReceipt_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        public void LoadReport(string pcash, string pchange)
        {
            ReportDataSource rptDataSource;
            try
            {
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report1.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet2 ds = new DataSet2();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("select c.id, c.transno, c.pcode, c.price, c.qty, c.disc, c.total, c.sdate, c.status, p.pdesc from tblcart as c inner join tblproduct as p on p.pcode = c.pcode where transno like '" + f.lblTransno.Text + "'", cn);
                da.Fill(ds.Tables["dtSold"]);
                cn.Close();

                ReportParameter pSubTotal = new ReportParameter("pTotal", f.lblTotal.Text);
                ReportParameter pCash = new ReportParameter("pCash", pcash);
                ReportParameter pChange = new ReportParameter("pChange", pchange);
                ReportParameter pVATableSales = new ReportParameter("pVatable", f.lblVatable.Text);
                ReportParameter pVATAmount= new ReportParameter("pVat", f.lblVat.Text);
                ReportParameter pDiscount = new ReportParameter("pDiscount", f.lblDiscount.Text);
                ReportParameter pStore = new ReportParameter("pStore", store);
                ReportParameter pAddress = new ReportParameter("pAdress", address);
                ReportParameter pTransaction = new ReportParameter("pTransaction", "Invoice #: " + f.lblTransno.Text);
                ReportParameter pCashier = new ReportParameter("pCashier", f.lblUser.Text);

                reportViewer1.LocalReport.SetParameters(pSubTotal);
                reportViewer1.LocalReport.SetParameters(pCash);
                reportViewer1.LocalReport.SetParameters(pChange);
                reportViewer1.LocalReport.SetParameters(pVATableSales);
                reportViewer1.LocalReport.SetParameters(pVATAmount);
                reportViewer1.LocalReport.SetParameters(pDiscount);
                reportViewer1.LocalReport.SetParameters(pStore);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(pTransaction);
                reportViewer1.LocalReport.SetParameters(pCashier);

                rptDataSource = new ReportDataSource("DataSet2", ds.Tables["dtSold"]);
                reportViewer1.LocalReport.DataSources.Add(rptDataSource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 30;

                // Render the report to an image
                byte[] renderedBytes = reportViewer1.LocalReport.Render("Image", null, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);

                // Convert the byte array to a MemoryStream
                using (MemoryStream stream = new MemoryStream(renderedBytes))
                {
                    // Create a Bitmap from the MemoryStream
                    Bitmap bitmap = new Bitmap(stream);

                    // Print the image to a thermal printer
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += (sender, e) =>
                    {
                        e.Graphics.DrawImage(bitmap, 0, 0);
                    };

                    // Set the printer name to your thermal printer
                    pd.PrinterSettings.PrinterName = "";

                    // Print the document
                    pd.Print();
                }
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
