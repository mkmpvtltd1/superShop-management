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
    public partial class saleEdit : Form
    {
        public static string commit="",sl="";
        public double vat = 0.00;
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public saleEdit()
        {
            InitializeComponent();
            dataGridView1.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                string q = "select itemCode,itemName,quantity,unitPrice,Amount,sReturn,vatAmount from Sales where salesNo='" + textBox1.Text + "'";
                aa.Open();
                da.SelectCommand = new SqlCommand(q, aa);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].HeaderText = "Item Code";
                dataGridView1.Columns[1].HeaderText = "Item Name";
                dataGridView1.Columns[2].HeaderText = "Quantity";
                dataGridView1.Columns[3].HeaderText = "Unit Price";
                dataGridView1.Columns[4].HeaderText = "Amoutn";
                dataGridView1.Columns[5].HeaderText = "Return";
                dataGridView1.Columns[6].HeaderText = "vatAmount";
                dataGridView1.Show();
                string q2 = "select sDate,total,salesType,creditCardPer,discountPer,vatAmount,grandTotal,receive,change,remarks from SalesSummary where salesNo='" + textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(q2, aa);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    textBox2.Text = dr.GetValue(0).ToString();
                    textBox4.Text = dr.GetValue(1).ToString();
                    if (dr.GetValue(2).ToString() == "CASH")
                    {
                        radioButton3.Checked = true;
                    }
                    else
                    {
                        radioButton4.Checked = true;
                    }
                    textBox5.Text = dr.GetValue(3).ToString();
                    textBox6.Text = dr.GetValue(4).ToString();
                    textBox7.Text = dr.GetValue(5).ToString();
                    textBox8.Text = dr.GetValue(6).ToString();
                    textBox9.Text = dr.GetValue(7).ToString();
                    textBox10.Text = dr.GetValue(8).ToString();
                    textBox11.Text = dr.GetValue(9).ToString();
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
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
                        dataGridView1[6, e.RowIndex].Value = (quan * sprice * vat) / 100;

                        int dlen = dataGridView1.Rows.Count;
                        int i = 0;
                        double total = 0.00;
                        double vatTotal = 0.00;
                        for (i = 0; i < dlen - 1; i++)
                        {
                            total += Convert.ToDouble(dataGridView1[4, i].Value);
                            vatTotal += Convert.ToDouble(dataGridView1[6, i].Value);
                        }
                        double gt = total + vatTotal;
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
            else
            {
                MessageBox.Show("Enter Sales No first!", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                aa.Open();
                SqlCommand del = aa.CreateCommand();
                del.CommandText = "execute delSale @sNo";
                del.Parameters.AddWithValue("@sNo", textBox1.Text);
                int affect = del.ExecuteNonQuery();
                if(affect>0)
                {

                        SqlDataAdapter da = new SqlDataAdapter();
                        int dlen = dataGridView1.Rows.Count;
                        int i = 0, j = 0;
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
                        if (textBox1.Text == "")
                        {
                            j++;
                        }
                        else if (textBox2.Text == "")
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
                            string saleT = "";
                            if (radioButton3.Checked)
                            {
                                saleT = "CASH";
                            }
                            if (radioButton4.Checked)
                            {
                                saleT = "Credit Card";
                            }
                            string queary2 = "insert into SalesSummary values(@salesNo,@sDate,@total,@salesType,@creditCardPer,@discountPer,@vatAmount,@grandTotal,@receive,@change,@remarks)";
                            string queary1 = "insert into Sales values(@salesNo,@sDate,@itemCode,@itemName,@quantity,@unitPrice,@Amount,@sReturn,@vatAmount)";
                            //aa.Open();
                            for (int k = 0; k < dlen - 1; k++)
                            {
                                da.InsertCommand = new SqlCommand(queary1, aa);
                                da.InsertCommand.Parameters.AddWithValue("@salesNo", textBox1.Text);
                                da.InsertCommand.Parameters.AddWithValue("@sDate", textBox2.Text);
                                da.InsertCommand.Parameters.AddWithValue("@itemCode", dataGridView1[0, k].Value);
                                da.InsertCommand.Parameters.AddWithValue("@itemName", dataGridView1[1, k].Value);
                                da.InsertCommand.Parameters.AddWithValue("@quantity", dataGridView1[2, k].Value);
                                da.InsertCommand.Parameters.AddWithValue("@unitPrice", dataGridView1[3, k].Value);
                                da.InsertCommand.Parameters.AddWithValue("@Amount", dataGridView1[4, k].Value);
                                da.InsertCommand.Parameters.AddWithValue("@sReturn", Convert.ToString(dataGridView1[5, k].Value));
                                da.InsertCommand.Parameters.AddWithValue("@vatAmount", dataGridView1[6, k].Value);

                                m += da.InsertCommand.ExecuteNonQuery();
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

                            n = da.InsertCommand.ExecuteNonQuery();
                            if (n > 0 && m == dlen - 1)
                            {
                                MessageBox.Show(m.ToString() + " Items are sold successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                sl = textBox1.Text;
                                    textBox1.Text = "0";
                                    textBox2.Text = "";
                                    textBox4.Text = "0";
                                    textBox5.Text = "0";
                                    textBox6.Text = "0";
                                    textBox7.Text = "0";
                                    textBox8.Text = "0";
                                    textBox9.Text = "0";
                                    textBox10.Text = "0";
                                    textBox11.Text = "";
                                    radioButton3.Checked = true;
                                    commit = "1";
                                    
                               
                                //print process 
                            }
                            else
                            {
                                MessageBox.Show("Sales process failed!!!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }
               }
                else
                {
                    MessageBox.Show("Sales No wrong!! try again....", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string er = ex.ToString();
                if (er.IndexOf("Cannot insert duplicate key") != -1)
                {
                    MessageBox.Show("Challan No already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                aa.Close();
                textBox1_Leave(sender, e);
            }
        
       
        }
        //tiem info

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

                    this.dataGridView1.Rows[y].Cells["itemName"].Value = dr.GetValue(0).ToString();
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
                double dis = (Convert.ToDouble(textBox4.Text) * discout) / 100;
                double totalwithdis = Convert.ToDouble(textBox4.Text) - dis;
                double vatamount = Convert.ToDouble(textBox7.Text);
                double gandTotal = totalwithdis + vatamount;
                textBox8.Text = gandTotal.ToString();
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

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {

                textBox5.Text = "3";
            }
            else
            {
                textBox5.Text = "0";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = "0";
                textBox2.Text = "";
                textBox4.Text = "0";
                textBox5.Text = "0";
                textBox6.Text = "0";
                textBox7.Text = "0";
                textBox8.Text = "0";
                textBox9.Text = "0";
                textBox10.Text = "0";
                textBox11.Text = "";
                radioButton3.Checked = true;
                textBox1_Leave(sender, e);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saleEdit_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadPrint();
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
            if(commit == "1")
            {
                
                string q1 = "select itemCode,itemName,quantity,unitPrice,Amount,sReturn from Sales where salesNo='" + sl + "'";
                string q2 = "select salesNo,sDate,total,salesType,creditCardPer,discountPer,vatAmount,grandTotal,receive,change from SalesSummary where salesNo='" + sl + "'";

                try
                {
                    aa.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand(q1, aa);
                    DataSet1 mydataset = new DataSet1();
                    da.Fill(mydataset, "Sales");
                    da.Dispose();
                    SqlDataAdapter da2 = new SqlDataAdapter();
                    da2.SelectCommand = new SqlCommand(q2, aa);
                    da2.Fill(mydataset, "SalesSummary");
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
    }
}
