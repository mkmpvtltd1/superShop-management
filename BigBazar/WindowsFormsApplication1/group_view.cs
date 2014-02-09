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
    public partial class group_view : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlDataAdapter da = new SqlDataAdapter();

        public group_view()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void group_view_Load(object sender, EventArgs e)
        {
            groupsload();
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Provide a group name.", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
            }
            else
            {
                da.InsertCommand = new SqlCommand("insert into Groups values(@GroupName)", con);
                da.InsertCommand.Parameters.AddWithValue("@GroupName", textBox1.Text);
                try
                {
                    con.Open();
                    da.InsertCommand.ExecuteNonQuery();
                   
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                    groupsload();
                    textBox1.Text = "";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex<0)
            {
                MessageBox.Show("Select group name form groups list.", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            else
            {
                string q = "delete from Groups where GroupName='" + listBox1.SelectedItem.ToString() + "';";
                SqlCommand cmd = new SqlCommand(q, con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                   
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                    groupsload();
                }
            }
        }
        private void groupsload()
        {
            listBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("select * from Groups;", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listBox1.Items.Add(dr.GetValue(0).ToString());
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
}
