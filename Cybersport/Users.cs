using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Cybersport
{
    public partial class Users : Form
    {
        string constr = data.conStr;
        decimal count;
        public Users()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }

        private void Users_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        string countDate(string Sele)
        {
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(Sele, conn);
            var count = "";
            if (mySqlCommand.ExecuteScalar() == null)
            {
                return count;
            }
            else
            {
                count = mySqlCommand.ExecuteScalar().ToString();
            }
            conn.Close();
            return count;
        }

        private void Users_Load(object sender, EventArgs e)
        {
            count = Convert.ToDecimal(countDate("SELECT COUNT(*) FROM Users"));
            label1.Text = $"Количество записей: {count}";

            dataGridView1.RowHeadersVisible = false;
            string connectionString = data.conStr;
            string query = @"
        SELECT 
            UserID AS 'ID', 
            Username AS 'Логин', 
            Password AS 'Пароль', 
            Email AS 'Email', 
            FIO AS 'ФИО', 
            PhoneNumber AS 'Телефон', 
            Role AS 'Роль' 
        FROM Users";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable usersTable = new DataTable();
                    adapter.Fill(usersTable);

                    dataGridView1.DataSource = usersTable;
                    dataGridView1.ClearSelection();

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns[0].Visible = false;
        }
    }
}
