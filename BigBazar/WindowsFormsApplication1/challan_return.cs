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
    public partial class challan_return : Form
    {
        public double Pprice = 0.00;
       public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection aa = new SqlConnection(constr);
        public challan_return()
        {
            InitializeComponent();
            dataGridView1.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string theDate = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            textBox3.Text = theDate.ToString();
        }

        private void challan_return_Load(object sender, EventArgs e)
        {
            textBox3.Focus();
            textBox2.Text = DateTime.Now.ToString("yyMMddHHmmss");
            dataGridView1.Columns[4].Visible = false;
            countchallan();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                    

                    string itemC = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    Pprice = iteminfo(itemC, e.RowIndex);



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
                    dataGridView1[3, e.RowIndex].Value = quan * Pprice;
                  
                   

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private double iteminfo(string x, int y)
        {
            double pprice = 0.00;

            string q = "select itemName,purchasePrice from itemInfo where itemCode='" + x +"';";
            
            SqlCommand data = new SqlCommand(q, aa);
            try
            {
                aa.Open();
                SqlDataReader dr = data.ExecuteReader();
                if (dr.Read())
                {
                    //while (dr.Read())
                    //{
                    this.dataGridView1.Rows[y].Cells["itemName"].Value = dr.GetValue(0).ToString();
                    this.dataGridView1.Rows[y].Cells["purPrice"].Value = dr.GetValue(1).ToString();
                   
                    pprice = Convert.ToDouble(dr.GetValue(1).ToString());
                    //}
                }
                else
                {
                    MessageBox.Show("Item is not found!!", "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //  MessageBox.Show(sprice.ToString());
            return pprice;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int k = 0;
            try
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
                    

                }
                if (textBox2.Text == "")
                {
                    j++;
                }
                else if (textBox3.Text == "")
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
                    aa.Open();
                    for (i = 0; i < dlen - 1; i++)
                    {
                        da.InsertCommand = new SqlCommand("insert into challanReturn values(@challanNo,@cDate,@itemCode,@itemName,@quantity,@pPrice,@unitPrice)", aa);
                        da.InsertCommand.Parameters.AddWithValue("@challanNo", textBox2.Text);
                        da.InsertCommand.Parameters.AddWithValue("@cDate", textBox3.Text);
                        da.InsertCommand.Parameters.AddWithValue("@itemcode", dataGridView1[0, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@itemName", dataGridView1[1, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@quantity", dataGridView1[2, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@pPrice", dataGridView1[4, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@unitPrice", dataGridView1[3, i].Value);

                        da.InsertCommand.ExecuteNonQuery();
                        k++;
                        

                    }
                    if (k >0)
                    {
                        MessageBox.Show(k.ToString() + "'s items are saved successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.Rows.Clear();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Challan return process failed!!!Check if there have any empty fields.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                   
                    
                   

                }

            }
            catch (Exception ex)
            {
                string er = ex.ToString();
               
                if (er.IndexOf("Cannot insert duplicate key") != -1)
                {
                    MessageBox.Show("Challan Return No already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button3_Click(object sender, EventArgs e)
        {
            challan_return_edit cre = new challan_return_edit();
            cre.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            challan_return_delete crd = new challan_return_delete();
            crd.Show();
        }

        private void button1_Click(object sender, EventArgs e)
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



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void countchallan()
        {
            try
            {
                aa.Open();
                string q = "select count(challanNo) from challanReturn";
                SqlCommand cmd = new SqlCommand(q, aa);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    textBox1.Text = dr.GetValue(0).ToString();
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
      
       
    }
}
