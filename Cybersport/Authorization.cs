using Cybersport.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Cybersport
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }
        string conString = data.conStr;
        public int m1 = 0;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            this.Visible = false;
            registration.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Player player = new Player();
            this.Visible = false;
            player.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            {
                string login = textBox1.Text.ToString();
                string hashPassword = string.Empty;
                string userName = string.Empty;
                string role = string.Empty;

                if (textBox1.Text == localadmin.localName && textBox2.Text == localadmin.localPassword)
                {
                    LocalAdminForm localAdminForm = new LocalAdminForm();
                    this.Visible = false;
                    localAdminForm.ShowDialog();
                    this.Close();
                }
                MySqlConnection con = new MySqlConnection(conString);
                con.Open();

                MySqlCommand cmd = new MySqlCommand($"Select * From Users Where Username = '{login}'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                hashPassword = GetHashPass(textBox2.Text.ToString());
                userName = dt.Rows[0].ItemArray.GetValue(4).ToString();

                data.Login = login;
                data.usrName = userName;

                    if (hashPassword == dt.Rows[0].ItemArray.GetValue(2).ToString())
                    {
                        role = dt.Rows[0].ItemArray.GetValue(6).ToString();
                        data.role = role;
                        MessageBox.Show("Вы успешно авторизовались");
                        if (role == "Администратор")
                        {
                            Admin admin = new Admin();
                            this.Visible = false;
                            admin.ShowDialog();
                            this.Close();
                        }
                        if (role == "Менеджер")
                        {
                            Manager manager = new Manager();
                            this.Visible = false;
                            manager.ShowDialog();
                            this.Close();
                        }
                        if (role == "Участник")
                        {
                            Player player = new Player();
                            this.Visible = false;
                            player.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Некоректный пользователь");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль");
                        textBox2.Text = "";

                    }
            }
            /*//catch (Exception)
            {
                MessageBox.Show("Ошибка авторизации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }
        public static string GetHashPass(string password)
        {
            byte[] bytesPass = Encoding.UTF8.GetBytes(password);

            SHA256Managed hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(bytesPass);
            string hashPasswd = string.Empty;

            foreach (byte x in hash)
            {
                hashPasswd += string.Format("{0:x2}", x);
            }
            hashstring.Dispose();

            return hashPasswd;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
         bool IsValidLoginCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c >= '0' && c <= '9');
        }
        bool IsValidPassCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   "!@#$%^&*()_=-+,./{}<>?".Contains(c) ||
                   (c >= '0' && c <= '9');
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidPassCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if (m1 == 0)
            {
                textBox2.PasswordChar = default;
                pictureBox1.Image = Properties.Resources.free_icon_hide_2767146; 
                m1 = 1;
            }
            else if (m1 == 1)
            {
                textBox2.PasswordChar = '*'; 
                pictureBox1.Image = Properties.Resources.free_icon_eye_158746; 
                m1 = 0;
            }
        }

            private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
