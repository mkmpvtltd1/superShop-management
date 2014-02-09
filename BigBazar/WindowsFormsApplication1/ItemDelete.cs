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
    public partial class ItemDelete : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public ItemDelete()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "delete from itemInfo where itemCode='" + textBox1.Text.ToString() + "';";
                SqlCommand cmd = new SqlCommand(q, aa);
                aa.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("Item deletion succesfull.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                }
                else if (affect == 0)
                {
                    MessageBox.Show("Item not found to do delete operaton!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ItemDelete_Load(object sender, EventArgs e)
        {

        }
    }
}
