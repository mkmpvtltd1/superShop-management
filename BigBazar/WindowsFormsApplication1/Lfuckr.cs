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
    public partial class Lfuckr : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public Lfuckr()
        {
            InitializeComponent();
        }

        private void Lfuckr_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string q = "select itemCode from itemInfo;";

                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr.GetValue(0).ToString());
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = dateTimePicker1.Value.ToString("dd/MM/yyyy");
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = dateTimePicker2.Value.ToString("dd/MM/yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Fill up all fields correctly.");
            }
            else
            {
                try
                {

                    string q = "select sum(quantity) as quantity from challanIn where itemCode='" + comboBox1.Text + "' and cdate>='" + textBox1.Text + "' and cdate <='" + textBox2.Text + "'";
                    string q2 = "select distinct itemCode,itemName,pPrice,sPrice from challanIn where itemCode='" + comboBox1.Text + "' and cdate>='" + textBox1.Text + "' and cdate <='" + textBox2.Text + "'";
                    string q3 = "select sum(quantity) as squantity from Sales where itemCode='" + comboBox1.Text + "' and sDate>='" + textBox1.Text + "' and sDate <='" + textBox2.Text + "'";
                    con.Open();


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand(q, con);
                    DataTable dt = new DataTable();

                    ItemLager mydataset = new ItemLager();
                    da.Fill(mydataset, "Qty");
                    da.Dispose();

                    SqlDataAdapter da2 = new SqlDataAdapter();
                    da2.SelectCommand = new SqlCommand(q2, con);
                    da2.Fill(mydataset, "itemInfo");
                    da2.Dispose();

                    SqlDataAdapter da3 = new SqlDataAdapter();
                    da3.SelectCommand = new SqlCommand(q3, con);
                    da3.Fill(mydataset, "salesQty");
                    da3.Dispose();

                    sLCrystal myreport = new sLCrystal();
                    myreport.SetDataSource(mydataset);
                    crystalReportViewer1.ReportSource = myreport;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
