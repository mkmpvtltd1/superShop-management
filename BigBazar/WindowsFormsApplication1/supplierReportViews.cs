using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class supplierReportViews : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public supplierReportViews()
        {
            InitializeComponent();
        }

        private void supplierReportViews_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string q = "select distinct supplier from challanIn;";

                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr.GetValue(0).ToString());
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "errorr!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                con.Open();

           
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("execute supllierBalance @supName", con);
                da.SelectCommand.Parameters.AddWithValue("@supName", comboBox1.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);
                supBalReport myreport = new supBalReport();

                myreport.SetDataSource(dt);
                crystalReportViewer1.ReportSource = myreport;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"errorr!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
            }
        
    }

}
