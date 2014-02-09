using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DurationCalculatorApp;
using System.Configuration;

namespace WindowsFormsApplication1
{
    public partial class employee_payment : Form
    {
        public static string id, name, salary, months, status, pdate, due, payment, balance, description;
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);

        public employee_payment()
        {
            InitializeComponent();
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
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void employee_payment_Load(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("Select empID from employeeinfo;", aa);

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

       

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                SqlDataAdapter da = new SqlDataAdapter();

                string q3 = "insert into empPayment values(@empID,@pDate,@dueAmount,@currentPayment,@balance,@description)";
                da.InsertCommand = new SqlCommand(q3, aa);
                da.InsertCommand.Parameters.AddWithValue("@empID", comboBox1.Text);
                da.InsertCommand.Parameters.AddWithValue("@pDate", textBox1.Text);
                da.InsertCommand.Parameters.AddWithValue("@dueAmount", textBox2.Text);
                da.InsertCommand.Parameters.AddWithValue("@currentPayment", textBox3.Text);
                da.InsertCommand.Parameters.AddWithValue("@balance", textBox4.Text);
                da.InsertCommand.Parameters.AddWithValue("@description", textBox5.Text);
                aa.Open();
                int affect =da.InsertCommand.ExecuteNonQuery();
                if (affect > 0)
                {
                    MessageBox.Show("Payment Record saved successfully", "INfo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    id = comboBox1.Text;
                    name = textBox6.Text;
                    salary = textBox7.Text;
                    months = textBox8.Text;
                    status = label12.Text;
                    pdate = textBox1.Text;
                    due = textBox2.Text;
                    payment = textBox3.Text;
                    balance = textBox4.Text;
                    description = textBox5.Text;
                    
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
              
                   
                }
                else
                {
                    MessageBox.Show("Payment saving process failed. tray again.....", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
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
            string  status="", salary="";
            string joinD = "";
            double curPayment =0.00;
            string q1 ="select empName,salary,status,joinDate from employeeInfo where empID='"+comboBox1.Text+"'";
            string q2 = "select sum(currentPayment) from empPayment where empID='" + comboBox1.Text + "'";
            SqlCommand cmd = new SqlCommand(q1, aa);
            
            try
            {
                aa.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    textBox6.Text = dr.GetValue(0).ToString();
                    salary = dr.GetValue(1).ToString();
                    textBox7.Text = dr.GetValue(1).ToString();
                    status = dr.GetValue(2).ToString();
                    joinD = dr.GetValue(3).ToString();
                }

                dr.Close();
                SqlCommand cmd2 = new SqlCommand(q2, aa);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                string curPtemp = "";
                while (dr2.Read())
                {
                    
                    
                        curPtemp = dr2.GetValue(0).ToString();
                    
                }

                if (curPtemp != "")
                {
                    curPayment = Convert.ToDouble(curPtemp);

                }
                else
                {
                    curPayment = 0.00;
                }
                dr2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "errror!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aa.Close();
            }
            //salary calculation
            string[] s = status.Split(' ');

           // MessageBox.Show(s[0] + "\t" + s[1]);

            if (s[0] == "Inactive")
            {
                label12.Text = "Inactive";
                string activeDate = joinD;

                DateTime jdate = Convert.ToDateTime(activeDate);
                DateTime currentdate = Convert.ToDateTime(s[1].ToString());
                DateDifference dateDifference = new DateDifference(jdate, currentdate);

                string DiffDate = dateDifference.ToString();
              //  MessageBox.Show(DiffDate);
                string[] sDD = DiffDate.Split('/');
                int yy = Convert.ToInt32(sDD[0]);
                int mm = Convert.ToInt32(sDD[1]);
                int dd = Convert.ToInt32(sDD[2]);
                //if (dd != 0)
                //{
                //    mm += 1;
                //}
                int totalMonth = (yy * 12) + mm;
                textBox8.Text = totalMonth.ToString();

                double totalSal = totalMonth * Convert.ToDouble(salary);
                double dueSal = totalSal - curPayment;
                textBox2.Text = dueSal.ToString();
            }
            else if (s[0] == "Active")
            {
                label12.Text = "Active";
                string activeDate = s[1].ToString();

                DateTime jdate = Convert.ToDateTime(activeDate);
                DateTime currentdate = DateTime.Now;
                DateDifference dateDifference = new DateDifference(jdate,currentdate);

                string DiffDate = dateDifference.ToString();
                string[] sDD = DiffDate.Split('/');
                int yy = Convert.ToInt32(sDD[0]);
                int mm = Convert.ToInt32(sDD[1]);
                int dd = Convert.ToInt32(sDD[2]);
                //if (dd != 0)
                //{
                //    mm += 1;
                //}
                int totalMonth = (yy * 12) + mm;
                textBox8.Text = totalMonth.ToString();

                double totalSal = totalMonth * Convert.ToDouble(salary);
                double dueSal = totalSal - curPayment;
                textBox2.Text = dueSal.ToString();
            }
            else
            {
                MessageBox.Show("Error to receive data.try again..........", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



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

        private void button3_Click(object sender, EventArgs e)
        {

            string date = "11/01/1991";
            DateTime dt = Convert.ToDateTime(date);
            DateTime dt2 = Convert.ToDateTime("07/03/2013");
            DateDifference dateDifference = new DateDifference(dt, dt2);
           
            MessageBox.Show(dateDifference.ToString());
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            printSlip();
        }

        private void printSlip()
        {
            try
            {
                

                empPayDataset newdataset = new empPayDataset();

                DataRow row = newdataset.Tables["Payment"].NewRow();
                row[0] = id;
                row[1] = name;
                row[2] = salary;
                row[3] = months;
                row[4] = status;
                row[5] = pdate;
                row[6] = due;
                row[7] = payment;
                row[8] = balance;
                row[9] = description;
                newdataset.Tables["Payment"].Rows.Add(row);
                empPrintSlip myreport = new empPrintSlip();
                myreport.SetDataSource(newdataset);
                EMPRepotViews emv = new EMPRepotViews();
                emv.crystalReportViewer1.ReportSource = myreport;
               
                emv.Show();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
            

    }
}
