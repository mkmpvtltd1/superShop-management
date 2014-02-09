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
    public partial class employee_edit : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public employee_edit()
        {
            InitializeComponent();

        }

        private void employee_edit_Load(object sender, EventArgs e)
        {
            loademp();
        }

        

       

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || comboBox1.Text == "" || comboBox2.Text=="")
            {
                MessageBox.Show("You left empty fileds.", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = aa.CreateCommand();

                cmd.CommandText = "execute updatemp @oldempid,@empid,@empN,@add,@cellno,@jdate,@sal,@stat";
                cmd.Parameters.AddWithValue("@oldempid", comboBox1.Text);
                cmd.Parameters.AddWithValue("@empid", textBox1.Text);
                cmd.Parameters.AddWithValue("@empN", textBox2.Text);
                cmd.Parameters.AddWithValue("@add", textBox3.Text);
                cmd.Parameters.AddWithValue("@cellno", textBox4.Text);
                cmd.Parameters.AddWithValue("@jdate", textBox5.Text);
                cmd.Parameters.AddWithValue("@sal", textBox6.Text);
                cmd.Parameters.AddWithValue("@stat", comboBox2.Text + " " + DateTime.Now.ToString("MM/dd/yyyy"));

                try
                {
                    aa.Open();
                    int affect = cmd.ExecuteNonQuery();
                    if (affect == 1)
                    {
                        MessageBox.Show("Information upsated successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        comboBox1.Text = "";
                        comboBox2.Text = "";
                    }
                    else if (affect == 0)
                    {
                        MessageBox.Show("Infromation update faild.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (System.Exception ex)
                {
                    string er = ex.ToString();
                    if (er.IndexOf("Cannot insert duplicate key") != -1)
                    {
                        MessageBox.Show("Employee ID already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    aa.Close();
                    loademp();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void loademp()
        {


            comboBox1.Items.Clear();
            SqlCommand data = new SqlCommand("select empID from employeeInfo;", aa);
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
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            string q = "Select * from employeeInfo where empID='" + comboBox1.SelectedItem.ToString() + "';";
            SqlCommand cmd = new SqlCommand(q, aa);
            try
            {
                aa.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    textBox1.Text = dr.GetValue(0).ToString();
                    textBox2.Text = dr.GetValue(1).ToString();
                    textBox3.Text = dr.GetValue(2).ToString();
                    textBox4.Text = dr.GetValue(3).ToString();
                    textBox5.Text = dr.GetValue(4).ToString();
                    textBox6.Text = dr.GetValue(5).ToString();
                    string[] s = dr.GetValue(6).ToString().Split(' ');
                    textBox7.Text = s[0].ToString();
                    if (s[0].ToString() == "Active")
                    {
                        comboBox2.Visible = true;
                    }
                    else
                    {
                        comboBox2.Visible = false;
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

            }
        }

       

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox7.Text = comboBox2.SelectedItem.ToString();
        }

        
        
    }
}
