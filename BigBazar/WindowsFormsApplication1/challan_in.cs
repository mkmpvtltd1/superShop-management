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
    public partial class challan_in : Form
    {
        public double Sprice = 0.00;
      
        public double pamount = 0.00;
        public double TSprice = 0.00;
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        
        SqlConnection aa = new SqlConnection(constr);
        public challan_in()
        {
            InitializeComponent();
            dataGridView1.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void challan_in_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            textBox1.Text = DateTime.Now.ToString("yyMMddHHmmss");

            textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");

            supload();
        }
        private void supload()
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

        private void button4_Click(object sender, EventArgs e)
        {
            ChallanDelete chd = new ChallanDelete();
            chd.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChallanEdit che = new ChallanEdit();
            che.Show();
        }
        //grid calculation goes from here.......................

      
       

        //get item infor

        private double iteminfo(string x,int y)
        {
            double sprice=0.00;
            
            string q ="select itemName,purchasePrice,salePrice from itemInfo where itemCode='"+x+"' and supplier='"+comboBox1.Text+"';";
          // MessageBox.Show(q);
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
                        this.dataGridView1.Rows[y].Cells["salePrice"].Value = dr.GetValue(2).ToString();
                        sprice = Convert.ToDouble(dr.GetValue(2).ToString());
                    //}
                }
                else
                {
                    MessageBox.Show("This '"+x+"' item is not found under this '"+comboBox1.Text+"' suppiler!!", "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

               
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"error!!!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }
            finally
            {
                aa.Close();
            }
          //  MessageBox.Show(sprice.ToString());
            return sprice;
            
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                    //int rowIndex;

                   // rowIndex = dataGridView1.CurrentCell.RowIndex;

                    string itemC = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                   // MessageBox.Show(itemC + "Loc:" + e.RowIndex.ToString() + e.ColumnIndex.ToString());
                    Sprice = iteminfo(itemC, e.RowIndex);
                    


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
                   // int rowIndex;

                  //  rowIndex = dataGridView1.CurrentCell.RowIndex;

                    int quan = Convert.ToInt32(dataGridView1[e.ColumnIndex, e.RowIndex].Value);
                    double pPrice = Convert.ToDouble(dataGridView1[3, e.RowIndex].Value);
                    double sPrice = Convert.ToDouble(dataGridView1[5,e.RowIndex].Value);
                    pamount += quan * pPrice;
                    dataGridView1[4, e.RowIndex].Value = quan * pPrice;
                    dataGridView1[6, e.RowIndex].Value = quan * sPrice;
                    TSprice += quan * Sprice;
                    textBox4.Text = pamount.ToString();
                    textBox5.Text = TSprice.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }

        //data add to database............
        private void button2_Click(object sender, EventArgs e)
        {
            int n = 0, m = 0;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                int dlen = dataGridView1.Rows.Count;
                int i = 0, j = 0;

                if (textBox1.Text == "")
                {
                    j++;
                }
                else if (textBox2.Text == "")
                {
                    j++;
                }
                else if (comboBox1.Text == "")
                {
                    j++;
                }

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
                //insert data to db
                if (j > 0)
                {
                    MessageBox.Show("Fill up all fields correctly.You left empty fields.!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                 {
                     aa.Open();
                    for (i = 0; i < dlen-1; i++)
                    {
                        da.InsertCommand = new SqlCommand("insert into challanIn values(@challanNO,@cdate,@supplier,@itemCode,@itemName,@quantity,@pPrice,@tpPrice,@sPrice,@tsPrice)", aa);
                        da.InsertCommand.Parameters.AddWithValue("@challanNo", textBox1.Text);
                        da.InsertCommand.Parameters.AddWithValue("@cdate",textBox2.Text);
                        da.InsertCommand.Parameters.AddWithValue("@supplier", comboBox1.Text);
                        da.InsertCommand.Parameters.AddWithValue("@itemcode", dataGridView1[0, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@itemName", dataGridView1[1, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@quantity", dataGridView1[2, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@pPrice", dataGridView1[3, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@tpPrice", dataGridView1[4, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@sPrice", dataGridView1[5, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@tsPrice", dataGridView1[6, i].Value);

                        da.InsertCommand.ExecuteNonQuery();
                        n++;
                        

                    }
                    if (n>0)
                    {
                        MessageBox.Show(n.ToString() + "'s items are saved successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.Rows.Clear();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        comboBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Challan process failed!!!Check if there have any empty fields.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    
                    

                }

            }
            catch (Exception ex)
            {
                string er = ex.ToString();
                if (er.IndexOf("which was not supplied") != -1)
                {
                    MessageBox.Show("Fill up all fields correctly.You left empty fields.!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (er.IndexOf("Cannot insert duplicate key") != -1)
                {
                    MessageBox.Show("Challan kNo already exits!!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


                int dlen = dataGridView1.Rows.Count;
               
                int j = 0;
                double totalPPrice = 0.00;
                double TotalSalePrice = 0.00;
                for (j = 0; j < dlen-1; j++)
                {
                    totalPPrice += Convert.ToDouble(dataGridView1[4, j].Value);
                    TotalSalePrice += Convert.ToDouble(dataGridView1[6, j].Value);
                }
                textBox4.Text = totalPPrice.ToString();
                textBox5.Text = TotalSalePrice.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                aa.Open();
                string q = "select count(challanNo) from challanIn where supplier='" + comboBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(q, aa);

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

        
       
    }
}
