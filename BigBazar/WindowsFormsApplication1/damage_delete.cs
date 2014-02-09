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
    public partial class damage_delete : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public damage_delete()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("You Left empty fields.Fill those correctly", "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string q = "delete from damageItem where cdate='" + comboBox1.Text + "' and damageNo='" + comboBox2.Text + "';";
                SqlCommand cmd = new SqlCommand(q, aa);


                try
                {
                    aa.Open();
                    int affect = cmd.ExecuteNonQuery();
                    if (affect == 1)
                    {
                        MessageBox.Show("Damge imformation deletion successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (affect == 0)
                    {
                        MessageBox.Show("Damge imformation deletion faild.Damage number not found.Try again..!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    aa.Close();
                    loadDate();
                    comboBox1_SelectedIndexChanged(sender, e);



                }
            }




        }

        private void damage_delete_Load(object sender, EventArgs e)
        {
            loadDate();

        }


        private void loadDate()
        {
            comboBox1.Items.Clear();
            try
            {
                aa.Open();
                SqlCommand cmd = new SqlCommand("select  DISTINCT cdate from damageItem", aa);
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
            comboBox2.Items.Clear();
            try
            {
                aa.Open();
                string q = "select DISTINCT damageNo from damageItem where cdate='" + comboBox1.Text + "';";
                SqlCommand cmd = new SqlCommand(q, aa);
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
    }
}
