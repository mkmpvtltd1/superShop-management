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
    public partial class challan_info : Form
    {
        
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public challan_info()
        {
            InitializeComponent();
            dataGridView1.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        }

        private void challan_info_Load(object sender, EventArgs e)
        {
            supload();
        }
        private void supload()
        {

            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("Select DISTINCT supplier from challanIn;", aa);

            try
            {
                aa.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr.GetValue(0).ToString());
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                aa.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            comboBox2.Items.Clear();
            string q = "select DISTINCT cdate from challanIn where supplier='" + comboBox1.Text + "';";
            SqlCommand cmd = new SqlCommand(q, aa);

            try
            {
                aa.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox2.Items.Add(dr.GetValue(0).ToString());
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                aa.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Visible = true;
            comboBox3.Items.Clear();
            string q = "select DISTINCT challanNo from challanIn where cdate='" + comboBox2.Text + "';";
            SqlCommand cmd = new SqlCommand(q, aa);

            try
            {
                aa.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox3.Items.Add(dr.GetValue(0).ToString());
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                aa.Close();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            string q = "select itemCode,itemName,quantity,pPrice,tpPrice,sPrice,tsPrice from challanIn where supplier='" + comboBox1.Text + "' and cdate='" + comboBox2.Text + "' and challanNo='" + comboBox3.Text + "';";
            da.SelectCommand = new SqlCommand(q, aa);
            DataTable dt = new DataTable();

            try
            {
                aa.Open();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].HeaderText = "Item Code";
                dataGridView1.Columns[1].HeaderText = "Item Name";
                dataGridView1.Columns[2].HeaderText = "Quantity";
                dataGridView1.Columns[3].HeaderText = "Purchase Price";
                dataGridView1.Columns[4].HeaderText = "Purchase Amount";
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Show();

                object dr = dt.Compute("Sum(tpPrice)", "");
                textBox4.Text = dr.ToString();
                object dr1 = dt.Compute("SUM(tsPrice)", " ");
                textBox5.Text = dr1.ToString();

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                aa.Close();
                
               

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}
