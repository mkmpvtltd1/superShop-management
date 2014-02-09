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
    public partial class damage_edit : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public damage_edit()
        {
            InitializeComponent();
            dataGridView1.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
        }

        private void damage_edit_Load(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            comboBox2.Visible = false;
            countdamage();
            challanDateLoad();

        }
        private void challanDateLoad()
        {
            comboBox1.Items.Clear();

            string q = "select DISTINCT cDate from damageItem;";
            SqlCommand cmd = new SqlCommand(q, aa);

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



        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            comboBox2.Items.Clear();
            string q = "select DISTINCT damageNo from damageItem where cDate='" + comboBox1.Text + "' ;";
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

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            string q = "select itemCode,itemName,quantity,unitPrice,pPrice from damageItem where cdate='" + comboBox1.Text + "' and damageNo='" + comboBox2.Text + "';";
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
                dataGridView1.Columns[3].HeaderText = "Unit Price";
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Show();
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

        private void dataGridView1_RowHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            groupBox3.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox2.Text = row.Cells["itemCode"].Value.ToString();
                textBox3.Text = row.Cells["itemName"].Value.ToString();
                textBox4.Text = row.Cells["quantity"].Value.ToString();
                textBox5.Text = row.Cells["unitPrice"].Value.ToString();
                textBox6.Text = row.Cells["pPrice"].Value.ToString();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int quan = Convert.ToInt32(textBox4.Text);
                double pp = Convert.ToDouble(textBox6.Text);
                double unitp = quan * pp;

                SqlCommand cmd = aa.CreateCommand();
                cmd.CommandText = "execute  updateDamageItem @quan,@unitP,@damageNO,@date,@itemC";
                cmd.Parameters.AddWithValue("@quan", quan);
                cmd.Parameters.AddWithValue("@unitP", unitp);
                cmd.Parameters.AddWithValue("@damageNO", comboBox2.Text);
                cmd.Parameters.AddWithValue("@date", comboBox1.Text);
                cmd.Parameters.AddWithValue("@itemC", textBox2.Text);
                aa.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("Information updated successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";

                }
                else if (affect == 0)
                {
                    MessageBox.Show("Infromation update faild.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aa.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string q = "delete from damageItem where damageNo='" + comboBox2.Text + "' and cdate='" + comboBox1.Text + "' and itemCode='" + textBox2.Text + "';";
            SqlCommand cmd = aa.CreateCommand();
            cmd.CommandText = q;
            try
            {
                aa.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("Item deletion successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox6.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                }
                else if (affect == 0)
                {
                    MessageBox.Show("Item deletion faild.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aa.Close();
            }
        }

        private void countdamage()
        {

            try
            {
                aa.Open();
                string q = "select count(damageNo) from damageItem";
                SqlCommand cmd = new SqlCommand(q, aa);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    textBox1.Text = dr.GetValue(0).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aa.Close();
            }
        }
        
    }
}
