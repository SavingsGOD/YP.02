using Cybersport.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
    
namespace Cybersport
{
    public partial class Registration : Form
    {
        public Registration()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();

        }
        string conString = data.conStr;
        private void button3_Click(object sender, EventArgs e)
        {
            Authorization authorization = new Authorization();
            this.Visible = false;
            authorization.ShowDialog();
            this.Close();
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
        bool IsValidPhoneCharacter(char c)
        {
            return
                   (c >= '0' && c <= '9');
        }
        bool IsValidEmailCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   
                   (c >= '0' && c <= '9');
        }
        bool IsValidFIOCharacter(char c)
        {
            return (c >= 'а' && c <= 'я') ||
                   (c >= 'А' && c <= 'Я') ||
                   (c == 32) ||
                   (c == '-');

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidFIOCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidPhoneCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidEmailCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //EnableDisableRegisterButton();
        }

        private void Registration_Load(object sender, EventArgs e)
        {

            comboBox1.Items.Add("@mail.ru");
            comboBox1.Items.Add("@inbox.ru");
            comboBox1.Items.Add("@bk.ru");
            comboBox1.Items.Add("@list.ru");
            comboBox1.Items.Add("@internet.ru");
            comboBox1.Items.Add("@yandex.ru");
            comboBox1.Items.Add("@xmail.ru");
            comboBox1.Items.Add("@gmail.com");
            comboBox1.Items.Add("@yahoo.com");
            comboBox1.Items.Add("@hotmail.com");
            comboBox1.Items.Add("@outlook.com");

            //this.button1.Enabled = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            this.maskedTextBox1.TextChanged += new System.EventHandler(this.maskedTextBox1_TextChanged);
            
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //EnableDisableRegisterButton();
            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                var words = textBox3.Text.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(words[i]))
                    {
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                    }
                }

                textBox3.Text = string.Join(" ", words);

                textBox3.SelectionStart = textBox3.Text.Length;

                textBox3.SelectionStart = textBox3.Text.Length;
                textBox3.SelectionLength = 0;
                textBox3.TextChanged -= textBox3_TextChanged;
                textBox3.Text = textBox3.Text;
                textBox3.TextChanged += textBox3_TextChanged;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string fullName = textBox3.Text.Trim();
            string email = textBox4.Text.Trim() + comboBox1.Text.Trim();
            string phone = maskedTextBox1.Text.Trim();
            string role = "Участник";

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) || maskedTextBox1.MaskFull == false)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }
        
            string hashedPassword = HashPassword(password);

            using (MySqlConnection connection = new MySqlConnection(conString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, Password, FIO, Email, PhoneNumber, Role) VALUES (@login, @password, @fullName, @Email, @Phone, @Role)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@fullName", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Role", role);
                    try
                    {
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Регистрация прошла успешно!");
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                            maskedTextBox1.Clear();

                            Authorization authorization = new Authorization();
                            this.Visible = false;
                            this.Close();
                            authorization.ShowDialog();

                        }
                        else
                        {
                            MessageBox.Show("Ошибка при регистрации. Попробуйте еще раз.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
            }
        }

        /*private void EnableDisableRegisterButton()
        {
            bool allFieldsFilled = !string.IsNullOrEmpty(textBox1.Text.Trim()) &&
                                   !string.IsNullOrEmpty(textBox2.Text.Trim()) &&
                                   !string.IsNullOrEmpty(textBox3.Text.Trim()) &&
                                   !string.IsNullOrEmpty(textBox4.Text.Trim()) &&
                                   maskedTextBox1.MaskFull;

            button1.Enabled = allFieldsFilled;
        }*/

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //EnableDisableRegisterButton();
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Registration_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
