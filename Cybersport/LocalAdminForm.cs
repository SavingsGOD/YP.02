using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Cybersport
{
    public partial class LocalAdminForm : Form
    {
        string connect = data.conStr;

        public LocalAdminForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "CSV files (*.csv)|*.csv";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connect))
                    {
                        mySqlConnection.Open();
                        string filePath = openFileDialog1.FileName;
                        string tableName = comboBox1.SelectedItem.ToString();

                        int importRows = ImportCSV(filePath, tableName, mySqlConnection);
                        if (importRows > 0)
                        {
                            MessageBox.Show($"Успешно импортировано {importRows}  записей в таблицу {tableName}!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        int ImportCSV(string csvFilePath, string tableName, MySqlConnection connection)
        {
            int res = 0;
            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                string headerLine = reader.ReadLine();
                string[] headers = headerLine.Split(';');

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');

                    if (values.Length != headers.Length)
                    {
                        MessageBox.Show("The number of values does not match the number of headers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue; 
                    }
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = MySqlHelper.EscapeString(values[i]); 
                        values[i] = $"'{values[i]}'"; 
                    }

                    string query = $"INSERT INTO {tableName} ({string.Join(",", headers)}) VALUES ({string.Join(",", values)})";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        res += command.ExecuteNonQuery();
                    }
                }
            }
            return res;
        }

        private void LocalAdminForm_Load(object sender, EventArgs e)
        {
            button8.Enabled = false;
            label2.Text = "Локальный администратор";
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SHOW TABLES", con);

                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        comboBox1.Items.Add(dataReader.GetValue(0).ToString());
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите восстановить БД?", "Сообщение пользователю", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                MySqlConnection mySqlConnection = new MySqlConnection(connect);
                mySqlConnection.Open();

                string pathFile = Directory.GetCurrentDirectory() + @"\..\..\structure.sql";
                string textFile = File.ReadAllText(pathFile);
                MySqlCommand mySqlCommand = new MySqlCommand(textFile, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

                mySqlConnection.Close();

                MessageBox.Show("Структура базы данных успешно восстановлена!", "Сообщение пользователю", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
    }

        private void button4_Click(object sender, EventArgs e)
        {
            Authorization authorization = new Authorization();
            this.Visible = false;
            authorization.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void LocalAdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button8.Enabled = true;
        }
    }
}
