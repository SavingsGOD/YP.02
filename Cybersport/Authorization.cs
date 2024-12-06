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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;

namespace Cybersport
{
    public partial class Authorization : Form
    {
        string adminName = localadmin.localName;
        private string captchaText;
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
        private void AdminLocal(string adminName)
        {
            MySqlConnection con = new MySqlConnection(conString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.ToString();
            string password = textBox2.Text.ToString();

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Проверка локального администратора
            if (IsLocalAdmin(login, password))
            {
                MessageBox.Show("Вы успешно авторизовались как локальный администратор");
                LocalAdminForm localAdminForm = new LocalAdminForm();
                this.Visible = false;
                localAdminForm.ShowDialog();
                this.Close();
                return; // Завершить выполнение метода
            }

            // Если это не локальный администратор, продолжаем проверку в базе данных
            MySqlConnection con = new MySqlConnection(conString);
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"Select * From Users Where Username = '{login}'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string hashPassword = GetHashPass(password);
                string storedHashPassword = dt.Rows[0].ItemArray.GetValue(2).ToString();

                if (hashPassword == storedHashPassword)
                {
                    // Получаем имя пользователя и роль только для обычных пользователей
                    string userName = dt.Rows[0].ItemArray.GetValue(4).ToString();
                    string role = dt.Rows[0].ItemArray.GetValue(6).ToString();
                    data.Login = login;
                    data.usrName = userName;
                    data.role = role;

                    MessageBox.Show("Вы успешно авторизовались");

                    switch (role)
                    {
                        case "Администратор":
                            Admin admin = new Admin();
                            this.Visible = false;
                            admin.ShowDialog();
                            break;
                        case "Менеджер":
                            Manager manager = new Manager();
                            this.Visible = false;
                            manager.ShowDialog();
                            break;
                        case "Участник":
                            Player player = new Player();
                            this.Visible = false;
                            player.ShowDialog();
                            break;
                        default:
                            MessageBox.Show("Некорректный пользователь");
                            break;
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный пароль", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Text = "";
                    Captha();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка авторизации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

            private bool IsLocalAdmin(string login, string password)
        {
            return login == localadmin.localName && password == localadmin.localPassword;
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
        private void Captha()
        {
            CaptchaToImage();
            pictureBox3.Enabled = false;
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Visible = true;
            button5.Visible = true;
            pictureBox3.Visible = true;
            button4.Visible = true;
            this.Height = 539;
            button1.Visible = false;
        }

        private void CaptchaToImage()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            captchaText = ""; for (int i = 0; i < 4; i++)
            {
                captchaText += chars[random.Next(chars.Length)];
            }
            Bitmap bmp = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.SmoothingMode = SmoothingMode.AntiAlias; graphics.Clear(Color.White);
            Font font = new Font("Comic Sans MS", 20, FontStyle.Bold);
            for (int i = 0; i < 4; i++)
            {
                PointF point = new PointF(i * 20, 0);
                graphics.TranslateTransform(10, 10);
                graphics.RotateTransform(random.Next(-10, 10));
                graphics.DrawString(captchaText[i].ToString(), font, Brushes.Black, point);
                graphics.ResetTransform();
            }
            for (int i = 0; i < 10; i++)
            {
                Pen pen = new Pen(Color.Red, random.Next(2, 5));
                int x1 = random.Next(pictureBox3.Width);
                int y1 = random.Next(pictureBox3.Height);
                int x2 = random.Next(pictureBox3.Width);
                int y2 = random.Next(pictureBox3.Height); graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            pictureBox3.Image = bmp;
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void Captha2()
        {
            CaptchaToImage();
            pictureBox3.Enabled = false;
            textBox3.Visible = true;
            button5.Visible = true;
            pictureBox3.Visible = true;
            button4.Visible = true;
            this.Height = 539;
            button1.Visible = false;
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            Captha2();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.ToString();
            string password = textBox2.Text.ToString();

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(textBox3.Text != captchaText)
            {
                MessageBox.Show("Капча введена неверно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Thread.Sleep(10000);
                Captha();
                textBox3.Clear();
                return;
            }
            // Проверка локального администратора
            if (IsLocalAdmin(login, password))
            {
                MessageBox.Show("Вы успешно авторизовались как локальный администратор");
                LocalAdminForm localAdminForm = new LocalAdminForm();
                this.Visible = false;
                localAdminForm.ShowDialog();
                this.Close();
                return; // Завершить выполнение метода
            }

            // Если это не локальный администратор, продолжаем проверку в базе данных
            MySqlConnection con = new MySqlConnection(conString);
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"Select * From Users Where Username = '{login}'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string hashPassword = GetHashPass(password);
                string storedHashPassword = dt.Rows[0].ItemArray.GetValue(2).ToString();

                if (hashPassword == storedHashPassword)
                {
                    /// Получаем имя пользователя и роль только для обычных пользователей
                    string userName = dt.Rows[0].ItemArray.GetValue(4).ToString();
                    string role = dt.Rows[0].ItemArray.GetValue(6).ToString();
                    data.Login = login;
                    data.usrName = userName;
                    data.role = role;

                    MessageBox.Show("Вы успешно авторизовались");

                    switch (role)
                    {
                        case "Администратор":
                            Admin admin = new Admin();
                            this.Visible = false;
                            admin.ShowDialog();
                            break;
                        case "Менеджер":
                            Manager manager = new Manager();
                            this.Visible = false;
                            manager.ShowDialog();
                            break;
                        case "Участник":
                            Player player = new Player();
                            this.Visible = false;
                            player.ShowDialog();
                            break;
                        default:
                            MessageBox.Show("Некорректный пользователь");
                            break;
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный пароль", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Text = "";
                    Captha();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка авторизации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
