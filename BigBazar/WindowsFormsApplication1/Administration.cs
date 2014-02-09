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
    public partial class Administration : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlDataAdapter da = new SqlDataAdapter();
        public Administration()
        {
            InitializeComponent();
        }

        private void Administration_Load(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
            groupBox4.Visible = false;
            groupBox2.Visible = true;
        }

        private void create_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text=="")
            {
                MessageBox.Show("You left empty fields!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                da.InsertCommand = new SqlCommand("insert into UserList values(@UserID,@Password,@UserType)", con);
                da.InsertCommand.Parameters.AddWithValue("@UserID", textBox1.Text);
                da.InsertCommand.Parameters.AddWithValue("@Password", textBox2.Text);
                da.InsertCommand.Parameters.AddWithValue("@UserType", comboBox1.SelectedItem.ToString());
                try
                {
                    con.Open();
                    da.InsertCommand.ExecuteNonQuery();
                    MessageBox.Show("User creation succesfull.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
                catch (System.Exception ex)
                {
                    string er = ex.ToString();
                    if (er.IndexOf("Cannot insert duplicate key") != -1)
                    {
                        MessageBox.Show("User already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                   
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
           
            try
            {
                string q = "delete from UserList where UserID='" + comboBox3.Text.ToString() + "';";
                SqlCommand cmd = new SqlCommand(q, con);
                con.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("User deletion succesfull.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(affect==0)
                {
                    MessageBox.Show("User not found to do delete operaton!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
               
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
                radioButton2_Click(sender, e);
            }

        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "" || textBox5.Text == "" || comboBox2.Text=="")
            {
                MessageBox.Show("You left empty fields!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                
                try
                {
                    string q = "update UserList set Password='" + textBox5.Text + "' where UserID='" + comboBox2.Text + "' and Password='" + textBox4.Text + "';";
                  

                    SqlCommand cmd = new SqlCommand(q, con);
                    con.Open();
                    int affect = cmd.ExecuteNonQuery();
                    if (affect == 1)
                    {
                        MessageBox.Show("Password Update succesfull.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (affect == 0)
                    {
                        MessageBox.Show("User name or password wrong!try again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    textBox5.Text = "";
                    textBox4.Text = "";

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                    radioButton3_Click(sender, e);
                }
            }


        }

        private void radioButton2_Click(object sender, EventArgs e)
        {

            if (radioButton2.Checked)
            {
                groupBox2.Visible = false;
                groupBox4.Visible = false;
                groupBox3.Visible = true;
               
                comboBox3.Items.Clear();
                SqlCommand data = new SqlCommand("select UserID from UserList;", con);
                try
                {
                    con.Open();
                    SqlDataReader dr = data.ExecuteReader();
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
                    con.Close();
                }

            }
            
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                groupBox2.Visible = false;
                groupBox3.Visible = false;
                groupBox4.Visible = true;
                
                comboBox2.Items.Clear();

                SqlCommand data = new SqlCommand("select UserID from UserList;", con);
                try
                {
                    con.Open();
                    SqlDataReader dr = data.ExecuteReader();
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
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                groupBox2.Visible = true;
                groupBox3.Visible = false;
                groupBox4.Visible = false;
            }
        }

    
      
    }
}
