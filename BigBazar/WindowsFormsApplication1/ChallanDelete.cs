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
    public partial class ChallanDelete : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public ChallanDelete()
        {
            InitializeComponent();
        }

     

        private void ChallanDelete_Load(object sender, EventArgs e)
        {
            supload();
        }

        private void supload()
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            comboBox3.Visible = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || comboBox2.Text == "" || comboBox3.Text == "")
            {
                MessageBox.Show("You Left empty fields.Fill those correctly", "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                
                string q = "delete from challanIn where supplier='" + comboBox1.Text + "' and cdate='" + comboBox2.Text + "' and challanNo='" + comboBox3.Text + "';";
                SqlCommand cmd = new SqlCommand(q, aa);
              

                try
                {
                    aa.Open();
                    int affect = cmd.ExecuteNonQuery();
                    if (affect == 1)
                    {
                        MessageBox.Show("Challan deletion successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      
                    }
                    else if (affect == 0)
                    {
                        MessageBox.Show("Challan deletion faild.Challan number not found.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    aa.Close();
                    supload();
                    comboBox1_SelectedIndexChanged(sender, e);
                    comboBox2_SelectedIndexChanged(sender, e);
                    

                }
            }
        }
    }
}
