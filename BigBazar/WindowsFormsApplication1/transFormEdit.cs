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

    public partial class transFormEdit : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        public transFormEdit()
        {
            InitializeComponent();
        }

        private void transFormEdit_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            string q = "select DISTINCT tdate from transformItem;";
            SqlCommand cmd = new SqlCommand(q, con);

            try
            {
                con.Open();
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
                con.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            comboBox2.Items.Clear();
            string q = "select DISTINCT transNo from transformItem where tdate='" + comboBox1.Text + "' ;";
            SqlCommand cmd = new SqlCommand(q, con);

            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox2.Items.Add(dr.GetValue(0).ToString());
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
            string q = "select transFormId,transToId from transformItem where tdate='" + comboBox1.Text + "' and transNo='" + comboBox2.Text + "';";
           SqlCommand cmd = new SqlCommand(q,con);
           SqlDataReader dr = cmd.ExecuteReader();

           while (dr.Read())
           {
               textBox1.Text = dr.GetValue(0).ToString();
               textBox2.Text = dr.GetValue(1).ToString();
           }
           dr.Close();

           
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

        private void button5_Click(object sender, EventArgs e)
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
        }

        private void iteminfo(string x, int y)
        {


            string q = "select itemName from itemInfo where itemCode='" + x + "';";

            SqlCommand data = new SqlCommand(q, con);
            try
            {
                con.Open();
                SqlDataReader dr = data.ExecuteReader();
                if (dr.Read())
                {

                    this.dataGridView1.Rows[y].Cells["itemName"].Value = dr.GetValue(0).ToString();



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
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //

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


                }
                if (textBox2.Text == "")
                {
                    j++;
                }
                else if (textBox1.Text == "")
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
                    con.Open();
                        string q = "delete from transformItem where tdate='" + comboBox1.Text + "' and transNo='" + comboBox2.Text + "' and transFormId='"+textBox1.Text+"' and transToId='"+textBox2.Text+"'";
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        da2.DeleteCommand = new SqlCommand(q, con);
                        int aff=da2.DeleteCommand.ExecuteNonQuery();
                        if (aff > 0)
                        {

                            for (i = 0; i < dlen - 1; i++)
                            {
                                da.InsertCommand = new SqlCommand("insert into transformItem values(@transNo,@tdate,@transFormId,@transToId,@itemCode,@itemName,@quantity)", con);

                                da.InsertCommand.Parameters.AddWithValue("@transNo",comboBox2.Text);
                                da.InsertCommand.Parameters.AddWithValue("@tdate", comboBox1.Text);
                                da.InsertCommand.Parameters.AddWithValue("@transFormId", textBox1.Text);
                                da.InsertCommand.Parameters.AddWithValue("@transToId", textBox2.Text);
                                da.InsertCommand.Parameters.AddWithValue("@itemcode", dataGridView1[0, i].Value);
                                da.InsertCommand.Parameters.AddWithValue("@itemName", dataGridView1[1, i].Value);
                                da.InsertCommand.Parameters.AddWithValue("@quantity", dataGridView1[2, i].Value);


                                da.InsertCommand.ExecuteNonQuery();
                                k++;

                            }
                            if (k > 0)
                            {
                                MessageBox.Show(k.ToString() + "'s items are saved successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //dataGridView1.Rows.Clear();
                                textBox1.Text = "";
                                textBox2.Text = "";


                            }
                            else
                            {
                                MessageBox.Show("TransForm information saving  process failed!!!Check if there have any empty fields.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }

                        }
                        else
                        {
                            MessageBox.Show("Information Update failed.try again.....!", "error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                           
                            


                }

            }
            catch (Exception ex)
            {
                string er = ex.ToString();
                if (er.IndexOf("Cannot insert duplicate key") != -1)
                {
                    MessageBox.Show("Transform No already exits!!try again....", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //dataGridView1.Rows.Clear();
                con.Open();
                string q2 = "select itemCode,itemName,quantity from transformItem where tdate='" + comboBox1.Text + "' and transNo='" + comboBox2.Text + "' and transFormId='" + textBox1.Text + "' and transToId='" + textBox2.Text + "'";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand(q2, con);
                DataTable dt = new DataTable();
                dt.Rows.Clear();
                da.Fill(dt);


                

                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].HeaderText = "Item Code";
                dataGridView1.Columns[1].HeaderText = "Item Name";
                dataGridView1.Columns[2].HeaderText = "Quantity";


                dataGridView1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                con.Close();
            }
            
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        
        
    }
}
