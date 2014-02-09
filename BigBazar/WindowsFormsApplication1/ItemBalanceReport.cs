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
    public partial class ItemBalanceReport : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public ItemBalanceReport()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label1.Visible = false;
                label2.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                dateTimePicker2.Visible = false;
                dateTimePicker3.Visible = false;
                button1.Visible = false;
                label3.Visible = true;
                textBox3.Visible = true;
                dateTimePicker1.Visible = true;
                button2.Visible = true;
            }
            else
            {
                label1.Visible = true;
                label2.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                dateTimePicker2.Visible = true;
                dateTimePicker3.Visible = true;
                button1.Visible = true;

                label3.Visible = false;
                textBox3.Visible = false;
                dateTimePicker1.Visible = false;
                button2.Visible = false;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                con.Open();

           
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("execute allItemBalance @date", con);
                da.SelectCommand.Parameters.AddWithValue("@date",textBox3.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);
                AllitemBalDate myreport = new AllitemBalDate();

                myreport.SetDataSource(dt);
                allitemBalView albv = new allitemBalView();
                albv.crystalReportViewer1.ReportSource = myreport;
                albv.Show();
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox3.Text = dateTimePicker1.Value.ToString("dd/MM/yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                con.Open();


                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("execute allItemBalanceRange @fdate,@tdate", con);
                da.SelectCommand.Parameters.AddWithValue("@fdate",textBox1.Text);
                da.SelectCommand.Parameters.AddWithValue("@tdate", textBox2.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);
                AllBalReportRange myreport = new AllBalReportRange();

                myreport.SetDataSource(dt);
                allitemBalView albv = new allitemBalView();
                albv.crystalReportViewer1.ReportSource = myreport;
                albv.Show();
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

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = dateTimePicker2.Value.ToString("dd/MM/yyyy");
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = dateTimePicker3.Value.ToString("dd/MM/yyyy");
        }
        
    }

}
