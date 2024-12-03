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
    }
}
