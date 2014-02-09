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
    public partial class supplierEdit : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public supplierEdit()
        {
            InitializeComponent();
        }

        private void supplierEdit_Load(object sender, EventArgs e)
        {

            loadsup();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string  q ="Select * from supplierInfo where supplierName='"+comboBox1.SelectedItem.ToString()+"';";
            SqlCommand cmd = new SqlCommand(q, aa);
             try
           {
               aa.Open();
               SqlDataReader dr = cmd.ExecuteReader();
               while (dr.Read())
               {
                   textBox1.Text=dr.GetValue(0).ToString();
                    textBox2.Text=dr.GetValue(1).ToString();
                    textBox3.Text=dr.GetValue(2).ToString();
                    textBox6.Text=dr.GetValue(3).ToString();
                    textBox7.Text=dr.GetValue(4).ToString();
                    textBox8.Text=dr.GetValue(5).ToString();
                    textBox9.Text=dr.GetValue(6).ToString();

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

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = aa.CreateCommand();
            cmd.CommandText = "execute updatesupplier @oldsupname,@supName,@acc,@add,@phone,@fax,@email,@contact";

            cmd.Parameters.AddWithValue("@oldsupname", comboBox1.Text);
            cmd.Parameters.AddWithValue("@supName", textBox1.Text);
            cmd.Parameters.AddWithValue("@acc", textBox2.Text);
            cmd.Parameters.AddWithValue("@add", textBox3.Text);
            cmd.Parameters.AddWithValue("@phone", textBox6.Text);
            cmd.Parameters.AddWithValue("@fax", textBox7.Text);
            cmd.Parameters.AddWithValue("@email", textBox8.Text);
            cmd.Parameters.AddWithValue("@contact", textBox9.Text);

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
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                }
                else if (affect == 0)
                {
                    MessageBox.Show("Infromation update faild.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aa.Close();
                loadsup();
            }
           
        }
        private void loadsup()
        {
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("Select supplierName from supplierInfo;", aa);

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
    }
}
