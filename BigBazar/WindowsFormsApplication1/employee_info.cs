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
    
    public partial class employee_info : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public employee_info()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text=="" || comboBox1.Text=="")
            {
                MessageBox.Show("You left empty fileds.", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                
                SqlDataAdapter da = new SqlDataAdapter();
                da.InsertCommand = new SqlCommand("insert into employeeInfo values(@empID,@empName,@address,@cellNo,@joinDate,@salary,@status)", con);
                da.InsertCommand.Parameters.AddWithValue("@empID", textBox2.Text);
                da.InsertCommand.Parameters.AddWithValue("@empName", textBox1.Text);
                da.InsertCommand.Parameters.AddWithValue("@address", textBox3.Text);
                da.InsertCommand.Parameters.AddWithValue("@cellNo", textBox4.Text);
                da.InsertCommand.Parameters.AddWithValue("@joinDate",textBox5.Text);
                da.InsertCommand.Parameters.AddWithValue("@salary", textBox6.Text);
                da.InsertCommand.Parameters.AddWithValue("@status", comboBox1.Text+" " + DateTime.Now.ToString("MM/dd/yyyy"));

                try
                {
                    con.Open();
                    int affect =da.InsertCommand.ExecuteNonQuery();
                    if (affect > 0)
                    {
                        MessageBox.Show("Employee record saved successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        comboBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Information Saving failed.try again....", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (System.Exception ex)
                {
                    string er = ex.ToString();
                    if (er.IndexOf("Cannot insert duplicate key") != -1)
                    {
                        MessageBox.Show("Employee ID already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string theDate = dateTimePicker1.Value.ToString("MM/dd/yyyy");
            textBox5.Text = theDate.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            employee_edit empe = new employee_edit();
            empe.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            employee_delete ed = new employee_delete();
            ed.Show();
        }

        private void employee_info_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
