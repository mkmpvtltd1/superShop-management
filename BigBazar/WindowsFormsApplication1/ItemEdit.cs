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
    public partial class ItemEdit : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public ItemEdit()
        {
            InitializeComponent();
        }

     
        private void loadsupGrup()
        {
            comboBox1.Items.Clear();
            SqlCommand data = new SqlCommand("select supplierName from supplierInfo;", aa);
            try
            {
                aa.Open();
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
                aa.Close();
            }

            comboBox2.Items.Clear();
            SqlCommand group = new SqlCommand("select GroupName from Groups;", aa);
            try
            {
                aa.Open();
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
                aa.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            checkBox1.Checked = false;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            string q = "Select * from itemInfo where itemCode='" + textBox5.Text.ToString() + "';";
            SqlCommand cmd = new SqlCommand(q, aa);
            try
            {
                aa.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                checkBox1.Checked = false;
                while (dr.Read())
                {
                    textBox1.Text = dr.GetValue(0).ToString();
                    textBox2.Text = dr.GetValue(1).ToString();
                    textBox3.Text = dr.GetValue(2).ToString();
                    textBox4.Text = dr.GetValue(3).ToString();
                    comboBox1.Text = dr.GetValue(4).ToString();
                    comboBox2.Text = dr.GetValue(5).ToString();
                    if (dr.GetValue(6).ToString() == "1")
                    {
                        checkBox1.Checked = true;
                    }


                }


            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                aa.Close();
                loadsupGrup();
            }
            
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
                    datainsert(pprice, sprice);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Prices should be numaric value.!!", "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                
                SqlCommand cmd = aa.CreateCommand();
                cmd.CommandText = "execute updateitem @olditemC,@itemC,@itemN,@pPrice,@sPrice,@supplier,@gname,@vat";
                cmd.Parameters.AddWithValue("@olditemC",textBox5.Text);
                cmd.Parameters.AddWithValue("@itemC",textBox1.Text);
                cmd.Parameters.AddWithValue("@itemN",textBox2.Text);
                cmd.Parameters.AddWithValue("@pPrice",textBox3.Text);
                cmd.Parameters.AddWithValue("@sPrice",textBox4.Text);
                cmd.Parameters.AddWithValue("@supplier",comboBox1.Text);
                cmd.Parameters.AddWithValue("@gname",comboBox2.Text);
                cmd.Parameters.AddWithValue("@vat",vat);
                
                aa.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("Information updated successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox4.Text = "";

                    comboBox1.Text = "";
                    comboBox2.Text = "";
                    checkBox1.Checked = false;
                }
                else if (affect == 0)
                {
                    MessageBox.Show("Infromation update faild.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aa.Close();
                textBox5.Text = "";
            }
        }

        private void ItemEdit_Load(object sender, EventArgs e)
        {
            loadsupGrup();
        }
    }
}
