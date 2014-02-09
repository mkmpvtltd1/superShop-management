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
    public partial class challan_return_delete : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public challan_return_delete()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "" || comboBox3.Text == "")
            {
                MessageBox.Show("You Left empty fields.Fill those correctly", "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string q = "delete from challanReturn where cdate='" + comboBox2.Text + "' and challanNo='" + comboBox3.Text + "';";
                SqlCommand cmd = new SqlCommand(q, aa);


                try
                {
                    aa.Open();
                    int affect = cmd.ExecuteNonQuery();
                    if (affect == 1)
                    {
                        MessageBox.Show("Challan Return deletion successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (affect == 0)
                    {
                        MessageBox.Show("Challan Return deletion faild.Challan number not found.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    aa.Close();
                    challanDateLoad();
                    comboBox2_SelectedIndexChanged(sender, e);
                   


                }
            }
        }

        private void challanDateLoad()
        {
            comboBox2.Items.Clear();
            string q = "select DISTINCT cDate from challanReturn;";
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

        private void challan_return_delete_Load(object sender, EventArgs e)
        {
            challanDateLoad();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox3.Visible = true;
            string q = "select DISTINCT challanNo from challanReturn;";
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

       
    }
}
