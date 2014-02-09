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
    public partial class supplierDelete : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public supplierDelete()
        {
            InitializeComponent();
        }

        private void supplierDelete_Load(object sender, EventArgs e)
        {
            supplierload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "delete from supplierInfo where supplierName='" + comboBox1.Text.ToString() + "';";
                SqlCommand cmd = new SqlCommand(q, aa);
                aa.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("Supplier deletion succesfull.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (affect == 0)
                {
                    MessageBox.Show("Supplier not found to do delete operaton!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aa.Close();
                supplierload();
                
            }
        }
        private void supplierload()
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
