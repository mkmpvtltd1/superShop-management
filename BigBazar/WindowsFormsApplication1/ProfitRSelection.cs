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
    public partial class ProfitRSelection : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public ProfitRSelection()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox1.Visible = false;
                dateTimePicker1.Visible = false;
                button2.Visible = false;

                button1.Visible = true;
            }
            else
            {
                textBox1.Visible = true;
                dateTimePicker1.Visible = true;
                button2.Visible = true;

                button1.Visible = false;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = dateTimePicker1.Value.ToString("dd/MM/yyyy");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                con.Open();


                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("execute allProfitDate @date", con);
                da.SelectCommand.Parameters.AddWithValue("@date", textBox1.Text);

                DataTable dt = new DataTable();
                da.Fill(dt);

                profitReportDate myreport = new profitReportDate();
                myreport.SetDataSource(dt);
                profitView pv = new profitView();
                pv.crystalReportViewer1.ReportSource = myreport;
                pv.Show();


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
                da.SelectCommand = new SqlCommand("execute allProfit", con);
                
                DataTable dt = new DataTable();
                da.Fill(dt);

                allProfit myreport = new allProfit();
                myreport.SetDataSource(dt);
                profitView pv = new profitView();
                pv.crystalReportViewer1.ReportSource = myreport;
                pv.Show();
                

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
    }
}
