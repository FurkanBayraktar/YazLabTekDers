using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YazLabTekDers
{
    public partial class userForm : Form
    {
        
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=c:\users\furka\documents\visual studio 2017\Projects\YazLabTekDers\YazLabTekDers\Database1.mdf;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter da;

        public userForm()
        {
            InitializeComponent();
        }

        private void userForm_Load(object sender, EventArgs e)
        {
            updateDatabase();
        }

        private void updateDatabase()
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select name,surname From dbo.userTable Where Id=" + this.Tag+ "";
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            label4.Text = "Sn." + dr["name"] + " " + dr["surname"] + "";
            con.Close();


            con.Open();
            da = new SqlDataAdapter("Select * From dbo.Product", con);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            con.Close();

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "CİNS";
            dataGridView1.Columns[2].HeaderText = "BİRİM";
            dataGridView1.Columns[3].HeaderText = "FİYAT";
            dataGridView1.Columns[4].HeaderText = "AÇIKLAMA";
            dataGridView1.Columns[5].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Lütfen ürün cinsini yazınız.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Lütfen ürün birimini yazınız.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (String.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Lütfen ürün fiyatını yazınız.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (String.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Lütfen ürün açıklamasını yazınız.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Insert into dbo.Product(cins,birim,fiyat,dec,userID) values (@Cins,@Birim,@Fiyat,@Dec,@UserID)";
                cmd.Parameters.AddWithValue("@Cins",textBox1.Text);
                cmd.Parameters.AddWithValue("@Birim",textBox2.Text);
                cmd.Parameters.AddWithValue("@Fiyat",textBox3.Text);
                cmd.Parameters.AddWithValue("@Dec",richTextBox1.Text);
                cmd.Parameters.AddWithValue("@UserId",this.Tag);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            updateDatabase();
            temizle();
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            if (System.Text.RegularExpressions.Regex.IsMatch(textBox2.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru biçimde giriniz. Örnek : 300 ");
                textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            if (System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru biçimde giriniz. Örnek : 300 ");
                textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Delete From dbo.Product Where Id=" + dataGridView1.CurrentRow.Cells[0].Value + "";
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            updateDatabase();
            temizle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Update dbo.Product Set cins=@Cins,birim=@Birim,fiyat=@Fiyat,dec=@Dec where Id=" + dataGridView1.CurrentRow.Cells[0].Value + " ";
            cmd.Parameters.AddWithValue("@Cins", textBox1.Text);
            cmd.Parameters.AddWithValue("@Birim", textBox2.Text);
            cmd.Parameters.AddWithValue("@Fiyat", textBox3.Text);
            cmd.Parameters.AddWithValue("@Dec", richTextBox1.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            updateDatabase();
            temizle();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            richTextBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }

        private void temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            richTextBox1.Clear();
        }
    }
}
