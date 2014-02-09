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
    public partial class SalesViewItemDAte : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public SalesViewItemDAte()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("You left empty fields.Fill up all information.", "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand("execute salesDateWise @itemCode,@date",con);
                    da.SelectCommand.Parameters.AddWithValue("@itemCode",comboBox1.Text);
                    da.SelectCommand.Parameters.AddWithValue("@date",textBox1.Text);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    SalesItemBalDAte myreport = new SalesItemBalDAte();
                    myreport.SetDataSource(dt);
                    saleItemBalDAteView sibdv = new saleItemBalDAteView();
                    sibdv.crystalReportViewer1.ReportSource = myreport;
                    sibdv.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void SalesViewItemDAte_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string q = "select distinct itemCode from Sales;";

                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr.GetValue(0).ToString());
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
