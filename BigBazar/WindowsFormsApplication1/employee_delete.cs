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
    public partial class employee_delete : Form
    {


        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public employee_delete()
        {
            InitializeComponent();
           
        }

        private void employee_delete_Load(object sender, EventArgs e)
        {
            empload();
        }
        private void empload()
        {
            comboBox1.Items.Clear();
            SqlCommand data = new SqlCommand("select empID from employeeInfo;", con);
            try
            {
                con.Open();
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
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "delete from employeeInfo where empID='"+comboBox1.Text +"';";
               
                SqlCommand cmd = new SqlCommand(q, con);
                con.Open();
                int affect = cmd.ExecuteNonQuery();
                if (affect == 1)
                {
                    MessageBox.Show("Employee deletion succesfull.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (affect == 0)
                {
                    MessageBox.Show("Employee not found to do delete operaton!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
                empload();
            }
        }
    }
}
