using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class mainform : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public mainform()
        {
            InitializeComponent();
           
        }


        private void groupInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            group_view groupf = new group_view();
            groupf.Show();
        }

        private void suplierInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            supplier suppliers = new supplier();
            suppliers.Show();
        }

        private void itemsInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            item itemsf = new item();
            itemsf.Show();
        }

        private void challanInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            challan_in challanIN = new challan_in();
            challanIN.Show();
        }

        private void challanReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            challan_return challanRet = new challan_return();
            challanRet.Show();
        }

        private void challanInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            challan_info challanInfo = new challan_info();
            challanInfo.Show();
        }

        private void damageItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            damage damagef = new damage();
            damagef.Show();
            
        }

        private void transformItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            transformItem tfi = new transformItem();
            tfi.Show();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sales sales = new Sales();
            sales.Show();
        }

        private void suppliersPaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            supplier_payment_insert supplierPayment = new supplier_payment_insert();
            supplierPayment.Show();
        }

       

        private void itemBalanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemBalanceReport iBalR = new ItemBalanceReport();
            iBalR.Show();
        }

        private void administrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Administration adf = new Administration();
            adf.Show();
        }

        private void supplierPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            supplier_payment_insert a = new supplier_payment_insert();
            a.ShowDialog();
        }


        private void employeePaymentToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            employee_payment a = new employee_payment();
            a.ShowDialog();
        }

        private void employeeInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            employee_info a = new employee_info();
            a.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sales sales = new Sales();
            sales.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saleEdit se = new saleEdit();
            se.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            item I = new item();
            I.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            challan_in chI = new challan_in();
            chI.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            challan_info chIn = new challan_info();
            chIn.Show();
        }

        System.Windows.Forms.Timer tmr = null;
        private void StartTimer()
        {
            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 1000;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Enabled = true;
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString(" hh:mm:ss tt");
        }

        private void mainform_Load(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            StartTimer();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void salesEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saleEdit se = new saleEdit();
            se.Show();
        }

        private void userGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fbPath = Application.StartupPath;
            string fname = "Home.html";
            string filename = fbPath + @"\help\" + fname;
            FileInfo fi = new FileInfo(filename);
            if (fi.Exists)
            {
                Help.ShowHelp(this, filename, HelpNavigator.Find, "");
            }
            else
            {
                MessageBox.Show("Help file Is in Progress.. ", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void aboutBigBazarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about_bb ab = new about_bb();
            ab.Show();
        }

        private void applicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab1 = new AboutBox1();
            ab1.Show();
        }

        private void developerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            devel d = new devel();
            d.Show();
            
        }

        private void itemLagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemLager il = new ItemLager();
            il.Show();
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            supplierReportViews srv = new supplierReportViews();
            srv.Show();
        }

        private void profitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfitRSelection ps = new ProfitRSelection();
            ps.Show();
        }

        private void allBalanceReportSalesWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemBalanceSaleW ibsw = new ItemBalanceSaleW();
            ibsw.Show();
        }

        private void salesitemDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesViewItemDAte svidvk = new SalesViewItemDAte();
            svidvk.Show();
        }

        private void itemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("select * from itemInfo", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                itemInformationReport myreport = new itemInformationReport();
                myreport.SetDataSource(dt);
                itemInformatonReportViews iirv = new itemInformatonReportViews();
                iirv.crystalReportViewer1.ReportSource = myreport;
                iirv.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
       
    }
}
