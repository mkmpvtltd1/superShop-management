using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace WindowsFormsApplication1
{
    public partial class item : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public item()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == ""||comboBox1.Text==""||comboBox2.Text=="")
            {
                MessageBox.Show("You have left empty fields!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    double pprice = double.Parse(textBox3.Text);
                    double sprice = double.Parse(textBox4.Text);
                    datainsert(pprice,sprice);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Prices should be numaric value.!!","error!!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                finally
                {
                    aa.Close();
                }
                

            }
        }
        private void datainsert(double x,double y)
        {
            int vat;
            if (checkBox1.Checked)
            {
                vat = 1;
            }
            else
            {
                vat = 0;
            }
            try
            {
                aa.Open();

                SqlDataAdapter ab = new SqlDataAdapter();

                ab.InsertCommand = new SqlCommand("insert into itemInfo values(@itemCode,@itemName,@purchasePrice,@salePrice,@supplier,@groupName,@vat)", aa);
                ab.InsertCommand.Parameters.AddWithValue("@itemCode", textBox1.Text);
                ab.InsertCommand.Parameters.AddWithValue("@itemName", textBox2.Text);
                ab.InsertCommand.Parameters.AddWithValue("@purchasePrice", x);
                ab.InsertCommand.Parameters.AddWithValue("@salePrice", y);
                ab.InsertCommand.Parameters.AddWithValue("@supplier", comboBox1.Text);
                ab.InsertCommand.Parameters.AddWithValue("@groupName", comboBox2.Text);
                ab.InsertCommand.Parameters.AddWithValue("@vat", vat);
                ab.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("Item Information Successfully saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                comboBox1.Text = "";
                comboBox2.Text = "";
                checkBox1.Checked = false;
            }
            catch (System.Exception ex)
            {
                string er = ex.ToString();
                if (er.IndexOf("Cannot insert duplicate key") != -1)
                {
                    MessageBox.Show("Item already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                aa.Close();
            }
        }
        private void item_Load(object sender, EventArgs e)
        {
           string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
           SqlConnection con = new SqlConnection(constr);
            comboBox1.Items.Clear();
            SqlCommand data = new SqlCommand("select supplierName from supplierInfo;", con);
            try
            {
                con.Open();
                SqlDataReader dr = data.ExecuteReader();
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
                con.Close();
            }

            comboBox2.Items.Clear();
            SqlCommand group = new SqlCommand("select GroupName from Groups;", con);
            try
            {
                con.Open();
                SqlDataReader dr = group.ExecuteReader();
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
                con.Close();
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            checkBox1.Checked = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ItemEdit ie = new ItemEdit();
            ie.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ItemDelete id = new ItemDelete();
            id.Show();
        }
 
    }
}
