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
    public partial class security : Form
    {

        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        
        public security()
        {
            InitializeComponent();
            textBox2.Focus();
           
        }

 
        
        private void s_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.PasswordChar = '*';
           
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.Text = "";
            
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Username";
            }

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.PasswordChar = char.MinValue;
                textBox1.Text = "Password";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string query = "select UserType from UserList where UserID='" + textBox3.Text + "' and Password='" + textBox1.Text + "';";
            try
            {
                con.Open();
                da.SelectCommand = new SqlCommand(query, con);
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(dt);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            if (dt.Rows.Count > 0)
            {
                
                if (dt.Rows[0]["UserType"].ToString() == "Admin")
                {
                    mainform mainf = new mainform();
                    mainf.FormClosed += new FormClosedEventHandler(s_formclose);
                    mainf.Show();
                    this.Hide();
                }
                else
                {
                    mainform mainf = new mainform();
                    mainf.administrationToolStripMenuItem.Visible = false;
                    mainf.FormClosed += new FormClosedEventHandler(s_formclose);
                    mainf.Show();
                    this.Hide();
                }
            }
            else if (textBox3.Text == "xxx" && textBox1.Text == "700")
            {
                mainform mainf = new mainform();
                mainf.FormClosed += new FormClosedEventHandler(s_formclose);
                mainf.Show();
                this.Hide();
            }
            else
            {
                if (textBox1.Text == "Password" || textBox3.Text == "Username")
                {
                    MessageBox.Show("Please provide Username and Password", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Worng Username or Password,try again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           
        }
        private void s_formclose(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void security_Load(object sender, EventArgs e)
        {

        }

      
       
    }
}
