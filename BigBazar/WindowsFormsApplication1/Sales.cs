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
    public partial class Sales : Form
    {
        public static string salescommit = "",sl="0";
        public double vat =0.00;
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public Sales()
        {
            InitializeComponent();
            dataGridView1.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Sales_Load(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString("yyMMddHHmmss");
            textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");
            radioButton3.Checked = true;
            loadcount();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {

                textBox5.Text = "3";
            }
            else
            {
                textBox5.Text = "";
            }
        }

       

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                   
                    string itemC = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();      
                    iteminfo(itemC, e.RowIndex);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            if (e.ColumnIndex == 2)
            {
                try
                {
                    

                    int quan = Convert.ToInt32(dataGridView1[e.ColumnIndex, e.RowIndex].Value);
                    double sprice = Convert.ToDouble(dataGridView1[3, e.RowIndex].Value);
                    dataGridView1[4, e.RowIndex].Value = quan * sprice;
                    dataGridView1[6, e.RowIndex].Value = (quan * sprice *vat)/100;

                    int dlen = dataGridView1.Rows.Count;
                    int i=0;
                    double total = 0.00;
                    double vatTotal = 0.00;
                    for(i=0;i<dlen-1;i++)
                    {
                        total += Convert.ToDouble(dataGridView1[4, i].Value);
                        vatTotal += Convert.ToDouble(dataGridView1[6, i].Value);
                    }
                    double gt = total+vatTotal;
                    textBox4.Text = total.ToString();
                    textBox7.Text = vatTotal.ToString();
                    textBox8.Text = gt.ToString();

                    

                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

            
        }

        private void iteminfo(string x, int y)
        {
            

            string q = "select itemName,salePrice,vat from itemInfo where itemCode='" + x + "';";
            // MessageBox.Show(q);
            SqlCommand data = new SqlCommand(q, aa);
            try
            {
                aa.Open();
                SqlDataReader dr = data.ExecuteReader();
                if (dr.Read())
                {
                    
                    this.dataGridView1.Rows[y].Cells["itemN"].Value = dr.GetValue(0).ToString();
                    this.dataGridView1.Rows[y].Cells["unitPrice"].Value = dr.GetValue(1).ToString();
                   // MessageBox.Show(dr.GetValue(2).ToString()+"\t"+vat.ToString());
                    if (dr.GetValue(2).ToString() == "1")
                    {
                        vat = 1.50;
                    }
                    else
                    {
                        vat = 0.00;
                    }
                    //MessageBox.Show(dr.GetValue(2).ToString() + "\t" + vat.ToString());
                   
                }
                else
                {
                    MessageBox.Show("This '" + x + "' item is not found !!", "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                aa.Close();
            }
           
           

        }


        private void textBox6_Leave(object sender, EventArgs e)
        {
            try
            {
                double discout = Convert.ToDouble(textBox6.Text);
                double dis =(Convert.ToDouble(textBox4.Text) * discout) / 100;
                double totalwithdis = Convert.ToDouble(textBox4.Text) - dis;
                double vatamount =Convert.ToDouble(textBox7.Text);
                double gandTotal = totalwithdis+vatamount;
                textBox8.Text = gandTotal.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            try
            {
                double receive = Convert.ToDouble(textBox9.Text);
                double change = receive - Convert.ToDouble(textBox8.Text);
                textBox10.Text = change.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double receive = Convert.ToDouble(textBox9.Text);
                double change = receive - Convert.ToDouble(textBox8.Text);
                textBox10.Text = change.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                int dlen = dataGridView1.Rows.Count;
                int i = 0,j=0;
                for (i = 0; i < dlen - 1; i++)
                {
                    if (Convert.ToString(dataGridView1[0, i].Value) == "")
                    {
                        j++;
                    }
                    else if (Convert.ToString(dataGridView1[1, i].Value) == "")
                    {
                        j++;
                    }
                    else if (Convert.ToString(dataGridView1[2, i].Value) == "")
                    {
                        j++;
                    }
                    else if (Convert.ToString(dataGridView1[3, i].Value) == "")
                    {
                        j++;
                    }
                    else if (Convert.ToString(dataGridView1[4, i].Value) == "")
                    {
                        j++;
                    }
                    else if (Convert.ToString(dataGridView1[6, i].Value) == "")
                    {
                        j++;
                    }

                }
                if (textBox4.Text =="0")
                {
                    j++;
                }
                else if (textBox8.Text == "0")
                {
                    j++;
                }
                //insert data to db
                if (j > 0)
                {
                    MessageBox.Show("Fill up all fields correctly.You left empty fields.!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int m = 0, n = 0;
                    string saleT="";
                    if(radioButton3.Checked)
                    {
                        saleT="CASH";
                    }
                    if(radioButton4.Checked)
                    {
                        saleT="Credit Card";
                    }
                   string queary2 = "insert into SalesSummary values(@salesNo,@sDate,@total,@salesType,@creditCardPer,@discountPer,@vatAmount,@grandTotal,@receive,@change,@remarks)";
                    string queary1 = "insert into Sales values(@salesNo,@sDate,@itemCode,@itemName,@quantity,@unitPrice,@Amount,@sReturn,@vatAmount)";
                    aa.Open();
                    for(int k=0;k<dlen-1;k++)
                    {
                         da.InsertCommand = new SqlCommand(queary1, aa);
                         da.InsertCommand.Parameters.AddWithValue("@salesNo", textBox1.Text);
                         da.InsertCommand.Parameters.AddWithValue("@sDate", textBox2.Text);
                         da.InsertCommand.Parameters.AddWithValue("@itemCode",dataGridView1[0,k].Value);
                         da.InsertCommand.Parameters.AddWithValue("@itemName", dataGridView1[1, k].Value);
                         da.InsertCommand.Parameters.AddWithValue("@quantity", dataGridView1[2, k].Value);
                         da.InsertCommand.Parameters.AddWithValue("@unitPrice", dataGridView1[3, k].Value);
                         da.InsertCommand.Parameters.AddWithValue("@Amount", dataGridView1[4, k].Value);
                         da.InsertCommand.Parameters.AddWithValue("@sReturn", Convert.ToString(dataGridView1[5, k].Value));
                         da.InsertCommand.Parameters.AddWithValue("@vatAmount", dataGridView1[6, k].Value);

                        da.InsertCommand.ExecuteNonQuery();
                        m++;
                    }
                    da.Dispose();
                    da.InsertCommand = new SqlCommand(queary2, aa);
                    da.InsertCommand.Parameters.AddWithValue("@salesNo", textBox1.Text);
                    da.InsertCommand.Parameters.AddWithValue("@sDate", textBox2.Text);
                    da.InsertCommand.Parameters.AddWithValue("@total", textBox4.Text);
                    da.InsertCommand.Parameters.AddWithValue("@salesType", saleT);
                    da.InsertCommand.Parameters.AddWithValue("@creditCardPer", textBox5.Text);
                    da.InsertCommand.Parameters.AddWithValue("@discountPer", textBox6.Text);
                    da.InsertCommand.Parameters.AddWithValue("@vatAmount", textBox7.Text);
                    da.InsertCommand.Parameters.AddWithValue("@grandTotal", textBox8.Text);
                    da.InsertCommand.Parameters.AddWithValue("@receive", textBox9.Text);
                    da.InsertCommand.Parameters.AddWithValue("@change", textBox10.Text);
                    da.InsertCommand.Parameters.AddWithValue("@remarks", textBox11.Text);
                    
                    n=da.InsertCommand.ExecuteNonQuery();
                    if (n > 0 && m==dlen-1)
                    {
                        MessageBox.Show(m.ToString() + " Items are sold successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sl = textBox1.Text;
                      
                        
                        textBox4.Text = "0";
                        textBox5.Text = "0";
                        textBox6.Text = "0";
                        textBox7.Text = "0";
                        textBox8.Text = "0";
                        textBox9.Text = "0";
                        textBox10.Text = "0";
                        textBox11.Text = "";
                        radioButton3.Checked = true;
                        salescommit = "1";

                        textBox1.Text = DateTime.Now.ToString("yyMMddHHmmss");
                        textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        radioButton3.Checked = true;
                        
                        
                    }
                    else
                    {
                        MessageBox.Show("Sales process failed!!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
             }
            catch (Exception ex)
            {
                string er = ex.ToString();
                if (er.IndexOf("Cannot insert duplicate key") != -1)
                {
                    MessageBox.Show("Sales No already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                aa.Close();
                loadcount();
                dataGridView1.Rows.Clear();
            }

           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("sales successfull.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataGridView1.Rows.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "0";
            textBox5.Text = "";
            textBox6.Text = "0";
            textBox7.Text = "";
            textBox8.Text = "0";
            textBox9.Text = "0";
            textBox10.Text = "";
        }

        private void loadcount()
        {

            SqlCommand cmd = aa.CreateCommand();
            cmd.CommandText = "select count(salesNo) from SalesSummary";

            try
            {
                aa.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    textBox3.Text = dr.GetValue(0).ToString();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                aa.Close();
            }
        }

    

        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0;

            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.RemoveAt(row.Index);
                    i++;
                }
                if (i == 0)
                {
                    MessageBox.Show("First select a row.!!!", "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    MessageBox.Show(i.ToString() + " items successfully deleted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                int dlen = dataGridView1.Rows.Count;
                int j = 0;
                double total = 0.00;
                double vatTotal = 0.00;
                for (j = 0; j < dlen - 1; j++)
                {
                    total += Convert.ToDouble(dataGridView1[4, j].Value);
                    vatTotal += Convert.ToDouble(dataGridView1[6, j].Value);
                }
                double gt = total + vatTotal;
                textBox4.Text = total.ToString();
                textBox7.Text = vatTotal.ToString();
                textBox8.Text = gt.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            loadPrint();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            saleEdit se = new saleEdit();
            se.Show();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.P))
            {
                loadPrint();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void loadPrint()
        {
            if (salescommit == "1")
            {
                
                string q1 = "select itemCode,itemName,quantity,unitPrice,Amount,sReturn from Sales where salesNo='" + sl + "'";
                string q2 = "select salesNo,sDate,total,salesType,creditCardPer,discountPer,vatAmount,grandTotal,receive,change from SalesSummary where salesNo='" + sl + "'";

                try
                {
                    aa.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand(q1, aa);
                    DataSet1 mydataset = new DataSet1();
                    da.Fill(mydataset,"Sales");
                    da.Dispose();
                    SqlDataAdapter da2 = new SqlDataAdapter();
                    da2.SelectCommand = new SqlCommand(q2,aa);
                    da2.Fill(mydataset,"SalesSummary");
                    CrystalReport1 myreport = new CrystalReport1();
                    myreport.SetDataSource(mydataset);
                    
                    PrintMemo pm = new PrintMemo();
                    pm.crystalReportViewer1.ReportSource = myreport;
                    pm.Show();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    aa.Close();
                }
            }
            else
            {
                MessageBox.Show("First commit sales by Clicking Save Button!", "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            saleEdit se = new saleEdit();
            se.Show();
        }

        

    }
}
