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
    public partial class ChallanEdit : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public ChallanEdit()
        {
            InitializeComponent();
            dataGridView1.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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

        private void ChallanEdit_Load(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            supload();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
           // dataGridView1.Rows.Clear();
            SqlDataAdapter da = new SqlDataAdapter();
            string q = "select itemCode,itemName,quantity,pPrice,tpPrice,sPrice,tsPrice from challanIn where supplier='" + comboBox1.Text + "' and cdate='"+comboBox2.Text+"' and challanNo='"+comboBox3.Text+"';";
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
               object dr1 = dt.Compute("SUM(tsPrice)"," ");
               textBox5.Text = dr1.ToString();
                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                aa.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                
            }
        }

       

        

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            groupBox3.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["itemCode"].Value.ToString();
                textBox2.Text = row.Cells["itemName"].Value.ToString();
                textBox3.Text = row.Cells["quantity"].Value.ToString();
                textBox6.Text = row.Cells["pPrice"].Value.ToString();
                textBox7.Text = row.Cells["tpPrice"].Value.ToString();
                textBox8.Text = row.Cells["sPrice"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            try
            {
                int quan = Convert.ToInt32(textBox3.Text);
                double pp = Convert.ToDouble(textBox6.Text);
                double sp = Convert.ToDouble(textBox8.Text);
                double tpp, tsp;
                tpp = quan * pp;
                tsp = quan * sp;

                SqlCommand cmd = aa.CreateCommand();
                cmd.CommandText = "execute updateChallanIn @quan,@tpp,@tsp,@itemC,@date,@sup,@challanNO";
                cmd.Parameters.AddWithValue("@quan", quan);
                cmd.Parameters.AddWithValue("@tpp", tpp);
                cmd.Parameters.AddWithValue("@tsp", tsp);
                cmd.Parameters.AddWithValue("@itemC", textBox1.Text);
                cmd.Parameters.AddWithValue("@date", comboBox2.Text);
                cmd.Parameters.AddWithValue("@sup", comboBox1.Text);
                cmd.Parameters.AddWithValue("@challanNO", comboBox3.Text);

                aa.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("Information updated successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                }
                else if (affect == 0)
                {
                    MessageBox.Show("Infromation update faild.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                aa.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string q = "delete from challanIn where challanNo='"+comboBox3.Text+"' and cdate='"+comboBox2.Text+"' and supplier='"+comboBox1.Text+"' and itemCode='"+textBox1.Text+"';";
            SqlCommand cmd = aa.CreateCommand();
            cmd.CommandText = q;
            try
            {
                aa.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("Item deletion successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                }
                else if (affect == 0)
                {
                    MessageBox.Show("Item deletion faild.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                aa.Close();
            }
        }
    }
}
