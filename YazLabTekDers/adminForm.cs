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
    public partial class adminForm : Form
    {
        public adminForm()
        {
            InitializeComponent();
        }

        private void adminForm_Load(object sender, EventArgs e)
        {

            updataDatabase();

        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=c:\users\furka\documents\visual studio 2017\Projects\YazLabTekDers\YazLabTekDers\Database1.mdf;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter da;

        private void updataDatabase()
        {


            con.Open();
            da = new SqlDataAdapter("Select Id, userName, userPassword, name, surname, email, phoneNumber From dbo.userTable", con);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            con.Close();

            dataGridView1.Columns[0].Visible = false;
            List<int> idList = new List<int>();

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select DISTINCT Id From dbo.userTable" ;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                idList.Add((int)dr["Id"]);
            }
           
            con.Close();
            for(int i=0;i<idList.Count;i++)
            {
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select birim From dbo.Product Where userId=" + idList[i] + "";
                con.Open();
                dr = cmd.ExecuteReader();
                int toplam = 0;
                while (dr.Read())
                { 
                    toplam = toplam + (int)dr["birim"];
                }
                con.Close();
                var name="as"; 
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select userName From dbo.userTable Where Id=" + idList[i] + "";
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    name = (string) dr["userName"];
                }
                con.Close();
                Console.WriteLine(toplam);
                this.chart1.Series["ADET"].Points.AddXY(name,toplam);
            }
            for (int i = 0; i<3; i++) ;
            //this.chart1.Series["ADET"].Points.AddXY(,);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            String name, surname, phoneNumber, email, username, password;
            name = textBox1.Text;
            surname = textBox2.Text;
            phoneNumber = textBox4.Text;
            email = textBox3.Text;
            username = textBox5.Text;
            password = textBox6.Text;
            String errorMessage;

            if(String.IsNullOrEmpty(name))
            {
                MessageBox.Show("Lütfen isminizi yazınız.", "HATALI İSİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(String.IsNullOrEmpty(surname))
            {
                MessageBox.Show("Lütfen soyisminizi yazınız.", "HATALI SOYİSİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (String.IsNullOrEmpty(email))
            {
                MessageBox.Show("Lütfen E-Mail yazınız.", "HATALI E-Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(String.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Lütfen Telefon Numaranızı yazınız.", "HATALI Tel. No.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (String.IsNullOrEmpty(username))
            {
                MessageBox.Show("Lütfen Kullanıcı Adınızı yazınız.", "HATALI KULLANICI ADI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen Şifrenizi yazınız.", "HATALI ŞİFRE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkEmail(email, out errorMessage))
            {
                MessageBox.Show(errorMessage, "HATALI E-MAIL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Insert into dbo.userTable(userName, userPassword, boolAdmin, name, surname, email, phoneNumber) values (@userName,@userPassword,@boolAdmin,@name,@surname,@email,@phoneNumber)";
                cmd.Parameters.AddWithValue("@userName",username);
                cmd.Parameters.AddWithValue("@userPassword", password);
                cmd.Parameters.AddWithValue("@boolAdmin",0);
                cmd.Parameters.AddWithValue("@name",name);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phoneNumber",phoneNumber);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }

            MessageBox.Show(name + " " + surname + " başarıyla eklendi.", "ONAY", MessageBoxButtons.OK, MessageBoxIcon.Information);
            updataDatabase();

        }

        public bool checkEmail(String email,out string errorMessage)
        {

            // Confirm that there is an "@" and a "." in the e-mail address, and in the correct order.
            if (email.IndexOf("@") > -1)
            {
                if (email.IndexOf(".", email.IndexOf("@")) > email.IndexOf("@"))
                {
                    errorMessage = "";
                    return true;
                }
            }

            errorMessage = "E-Mail adresi doğru formatta değil.\n" +
               "Örnek :  'someone@example.com' ";
            return false;
        }

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
           

        //}
        // SİLME
        private void button2_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select boolAdmin From dbo.userTable Where Id=" + dataGridView1.CurrentRow.Cells[0].Value + "";
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int ifAdmin = (int)dr["boolAdmin"];
            con.Close();
            if (ifAdmin!=1)
            {
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Delete From dbo.userTable Where Id=" + dataGridView1.CurrentRow.Cells[0].Value + "";
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                updataDatabase();
            }
            else
            {
                MessageBox.Show("Seçtiğiniz hesap bir YÖNETİCİ hesabıdır silinemez.", "YÖNETİCİ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            
        }
        // GÜNCELLEME
        private void button3_Click(object sender, EventArgs e)
        {

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Update dbo.userTable Set userName=@serName, userPassword=@serPassword,name=@ame,surname=@urname,email=@mail,phoneNumber=@honeNumber where Id="+dataGridView1.CurrentRow.Cells[0].Value+" ";
            cmd.Parameters.AddWithValue("@serName", textBox5.Text);
            cmd.Parameters.AddWithValue("@serPassword", textBox6.Text);
            cmd.Parameters.AddWithValue("@ame", textBox1.Text);
            cmd.Parameters.AddWithValue("@urname", textBox2.Text);
            cmd.Parameters.AddWithValue("@mail", textBox3.Text);
            cmd.Parameters.AddWithValue("@honeNumber",textBox4.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            updataDatabase();

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

               
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }
    }
}
