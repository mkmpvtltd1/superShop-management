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
    public partial class supplier : Form
    {
       
       public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
       SqlConnection aa = new SqlConnection(constr);
        public supplier()
        {
            InitializeComponent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("You have left empty fields!\nMinimum information required(Name,account no,address,phone no).", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                string add =  textBox3.Text+textBox4.Text+textBox5.Text;
                try
                {
                    aa.Open();

                    SqlDataAdapter ab = new SqlDataAdapter();

                    ab.InsertCommand = new SqlCommand("insert into supplierInfo values(@supplierName,@accountNo,@address,@phoneNo,@faxNo,@email,@contactPerson)", aa);
                    ab.InsertCommand.Parameters.AddWithValue("@supplierName",textBox1.Text);
                    ab.InsertCommand.Parameters.AddWithValue("@accountNo", textBox2.Text);
                    ab.InsertCommand.Parameters.AddWithValue("@address",add);   
                    ab.InsertCommand.Parameters.AddWithValue("@phoneNo", textBox6.Text);
                    ab.InsertCommand.Parameters.AddWithValue("@faxNo", textBox7.Text);
                    ab.InsertCommand.Parameters.AddWithValue("@email",textBox8.Text);
                    ab.InsertCommand.Parameters.AddWithValue("@contactPerson",textBox9.Text);
                    ab.InsertCommand.ExecuteNonQuery();

                    MessageBox.Show("Supplier information saved successfully", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                }

                catch (System.Exception ex)
                {
                    string er = ex.ToString();
                    if (er.IndexOf("Cannot insert duplicate key") != -1)
                    {
                        MessageBox.Show("Supplier already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    aa.Close();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            supplierEdit se = new supplierEdit();
            se.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            supplierDelete sd = new supplierDelete();
            sd.Show();
        }

        private void supplier_Load(object sender, EventArgs e)
        {

        }

      
    }
}
