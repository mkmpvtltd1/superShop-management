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
    public partial class transformItem : Form
    {
        public static string constr = ConfigurationManager.ConnectionStrings["condb"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlDataAdapter da = new SqlDataAdapter();

        public transformItem()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            transFormEdit te = new transFormEdit();
            te.Show();
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

        }

        private void button3_Click(object sender, EventArgs e)
        {
            transfer_delete a = new transfer_delete();
            a.ShowDialog();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string theDate = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            textBox2.Text = theDate.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
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


                }
                if (textBox2.Text == "")
                {
                    j++;
                }
                else if (textBox3.Text == "")
                {
                    j++;
                }
                else if (textBox4.Text == "")
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
                    for (i = 0; i < dlen - 1; i++)
                    {
                        da.InsertCommand = new SqlCommand("insert into transformItem values(@transNo,@tdate,@transFormId,@transToId,@itemCode,@itemName,@quantity)", con);

                        da.InsertCommand.Parameters.AddWithValue("@transNo", textBox1.Text);
                        da.InsertCommand.Parameters.AddWithValue("@tdate", textBox2.Text);
                        da.InsertCommand.Parameters.AddWithValue("@transFormId", textBox3.Text);
                        da.InsertCommand.Parameters.AddWithValue("@transToId", textBox4.Text);
                        da.InsertCommand.Parameters.AddWithValue("@itemcode", dataGridView1[0, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@itemName", dataGridView1[1, i].Value);
                        da.InsertCommand.Parameters.AddWithValue("@quantity", dataGridView1[2, i].Value);


                        da.InsertCommand.ExecuteNonQuery();
                        k++;

                    }
                    if (k > 0)
                    {
                        MessageBox.Show(k.ToString() + "'s items are saved successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.Rows.Clear();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("TransForm information saving  process failed!!!Check if there have any empty fields.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void transformItem_Load(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");
            textBox1.Text = DateTime.Now.ToString("yyMMddHHmmss");

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
    }
}
