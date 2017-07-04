using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace YazLabTekDers
{
    public partial class loginForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=c:\users\furka\documents\visual studio 2017\Projects\YazLabTekDers\YazLabTekDers\Database1.mdf;Integrated Security=True");
        SqlCommand cmd;
        ArrayList kAdi = new ArrayList();
        ArrayList kSifre = new ArrayList();
        List<int> kBool = new List<int>();
        List<int> kID = new List<int>();

        public loginForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updataDatabase();
        }
        private void updataDatabase()
        {
            
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = " Select * From dbo.userTable";
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                kAdi.Add(dr["userName"]);
                kSifre.Add(dr["userPassword"]);
                kID.Add((int)dr["Id"]);
                int a = (int) dr["boolAdmin"];
                kBool.Add(a);
            }

            con.Close();


        }
        private void button1_Click(object sender, EventArgs e)
        {

            // BOS KONTROL 

            if(textBox1.Text==null||textBox2.Text==null)
            {
                MessageBox.Show("Lütfen Kullanıcı Adı ve Şifresini kontrol ediniz.", "HATALI GİRİŞ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Boolean giris = false;

            for (int i = 0; i < kAdi.Count; i++)
            {
                if (kAdi[i].Equals(textBox1.Text))
                {
                    if (kSifre[i].Equals(textBox2.Text))
                    {
                        giris = true;
                        if (kBool[i] == 1)
                        {
                            adminForm aForm = new adminForm();
                            aForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            userForm uForm = new userForm();
                            uForm.Tag = kID[i];
                            uForm.Show();
                            this.Hide();
                        }
                    }
                }
            }

            if (!giris)
            {
                MessageBox.Show("Lütfen Kullanıcı Adı ve Şifresini kontrol ediniz.", "HATALI GİRİŞ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



        }
    }
}
