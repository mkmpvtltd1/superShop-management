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
    public partial class supplier_payment_insert : Form
    {
        public static string id, name, pDate, due, payment, balance, description;
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        
        public supplier_payment_insert()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

  

        private void supplier_payment_insert_Load(object sender, EventArgs e)
           
        {
            textBox1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            SqlCommand cmd = new SqlCommand("Select supplierName from supplierinfo;", aa);

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
            textBox1.Text = DateTime.Now.ToString("dd/MM/yyyy");

            try
            {
                aa.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                string q = "select supplierName,accountNo from supplierInfo where supplierName='"+comboBox1.Text+"'";
                string q2 = "select sum(currentPayment) from supplierPayment where supplierName='" + comboBox1.Text + "'";
                string q3 = "select sum(tpPrice) from challanIn where supplier='" + comboBox1.Text + "'";

                string payable = "", paid = "";
                SqlCommand cmd = new SqlCommand(q, aa);
                SqlCommand cmd2 = new SqlCommand(q2, aa);
                SqlCommand cmd3 = new SqlCommand(q3, aa);
                
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    textBox6.Text = dr.GetValue(0).ToString();
                    textBox7.Text = dr.GetValue(1).ToString();
                }
                dr.Close();
                SqlDataReader dr2 = cmd2.ExecuteReader();
                while (dr2.Read())
                {
                    textBox9.Text = dr2.GetValue(0).ToString();
                    paid = dr2.GetValue(0).ToString();
                }
                dr2.Close();
                SqlDataReader dr3 = cmd3.ExecuteReader();
                while (dr3.Read())
                {
                    textBox8.Text = dr3.GetValue(0).ToString();
                    payable = dr3.GetValue(0).ToString();
                }
                double paya=0.00;
                double pai=0.00;
                if (payable != "")
                {
                    paya = Convert.ToDouble(payable);
                }

                if(paid!="")
                {
                    pai = Convert.ToDouble(paid);
                }
                 
                double due = paya - pai;
                textBox2.Text = due.ToString();


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

        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.InsertCommand = new SqlCommand("insert into supplierPayment values(@supplierName,@paymentDate,@dueAmount,@currentPayment,@balance,@description)", aa);
            da.InsertCommand.Parameters.AddWithValue("@supplierName", comboBox1.Text);
            da.InsertCommand.Parameters.AddWithValue("@paymentDate", textBox1.Text);
            da.InsertCommand.Parameters.AddWithValue("@dueAmount", textBox2.Text);
            da.InsertCommand.Parameters.AddWithValue("@currentPayment", textBox3.Text);
            da.InsertCommand.Parameters.AddWithValue("@balance", textBox4.Text);
            da.InsertCommand.Parameters.AddWithValue("@description", textBox5.Text);

            try
            {
                aa.Open();
                int aff = da.InsertCommand.ExecuteNonQuery();
                if (aff > 0)
                {
                    MessageBox.Show("Payment saved successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    id = comboBox1.Text;
                    name = textBox6.Text;
                    pDate = textBox1.Text;
                    due = textBox2.Text;
                    payment = textBox3.Text;
                    balance = textBox4.Text;
                    description = textBox5.Text;
                    
                    comboBox1.Text = "";
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
                else
                {
                    MessageBox.Show("Payment saving process failed. tray again.....", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            try
            {
                double due = Convert.ToDouble(textBox2.Text);
                double cur = Convert.ToDouble(textBox3.Text);
                textBox4.Text = (due - cur).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Balance Field shouldn't empty and use only numbers!!", "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.P))
            {
                printSlip();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            printSlip();
        }

        private void printSlip()
        {
            try
            {


                suplierPaymentDataset mydataset = new suplierPaymentDataset();

                DataRow row = mydataset.Tables["supBalPay"].NewRow();
                row[0] = id;
                row[1] = name;
                row[2] = pDate;
                row[3] = due;
                row[4] = payment;
                row[5] = balance;
                row[6] = description;

                mydataset.Tables["supBalPay"].Rows.Add(row);
                supBalPaymentSlip myreport = new supBalPaymentSlip();
                myreport.SetDataSource(mydataset);

                supBalPaymentRViews sbpmrv = new supBalPaymentRViews();
                sbpmrv.crystalReportViewer1.ReportSource = myreport;
                sbpmrv.Show();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       
    }
}
