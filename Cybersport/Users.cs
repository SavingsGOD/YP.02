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
using System.Net;

namespace Cybersport
{
    public partial class Users : Form
    {
        string constr = data.conStr;
        private int currentPage = 1; // Current page number
        private int pageSize = 20;    // Number of records per page
        private int totalRecords;      // Total number of records

        public Users()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }
        private void GetDate()
        {
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = constr;

                con.Open();

                MySqlCommand cmd = new MySqlCommand($"select * from `Users`;", con);
                cmd.ExecuteNonQuery();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
                totalRecords = dataGridView1.RowCount;
            }

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
        public void LoadTeamsForTournaments2(string searchTerm = "")
        {

        }
        private void Users_Load(object sender, EventArgs e)
        {
            GetDate();
            LoadUsers();
            
        }

        private void LoadUsers(string searchTerm = "")
        {
            string connectionString = data.conStr;
            string query = @"
    SELECT 
        UserID AS 'ID', 
        Username AS 'Логин', 
        CONCAT(LEFT(Email, 2), REPEAT('*', LENGTH(Email) - 2 - LENGTH(SUBSTRING_INDEX(Email, '@', -1)) - 1), '@', SUBSTRING_INDEX(Email, '@', -1)) AS 'Email', -- Оставить первые 2 символа перед '@' и заменить остальные на '*'
        CONCAT(SUBSTRING_INDEX(FIO, ' ', 1), ' ', 
               UPPER(LEFT(SUBSTRING_INDEX(FIO, ' ', 2), 1)), '. ', 
               UPPER(LEFT(SUBSTRING_INDEX(FIO, ' ', -1), 1)), '.') AS 'ФИО', 
        CONCAT(SUBSTRING(PhoneNumber, 1, LENGTH(PhoneNumber) - 4), '****') AS 'Телефон', 
        Role AS 'Роль' 
    FROM Users";

            // Добавление условия поиска
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query += " WHERE Username LIKE @searchTerm"; // Search condition
            }

            query += " LIMIT @pageSize OFFSET @offset"; // Add limit and offset

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    }
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@offset", (currentPage - 1) * pageSize); // Calculate offset

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable usersTable = new DataTable();
                    adapter.Fill(usersTable);

                    dataGridView1.DataSource = usersTable;
                    dataGridView1.ClearSelection();

                    // Обновление общего количества записей
                    UpdateTotalRecords(searchTerm);

                    // Обновление отображения пагинации
                    UpdatePaginationDisplay();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }






        private void UpdateTotalRecords(string searchTerm)
        {
            string countQuery = "SELECT COUNT(*) FROM Users";
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                countQuery += " WHERE Username LIKE @searchTerm";
            }

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand countCommand = new MySqlCommand(countQuery, conn);
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        countCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    }

                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при подсчете записей: {ex.Message}");
                }
            }
        }

        private void UpdatePaginationDisplay()
        {
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Update labels or buttons for pagination
            number_of_pages.Text = $"Общее количество записей: {totalRecords} | Записи на текущей странице: {dataGridView1.RowCount}";

            // Enable or disable page buttons based on current page


            // Change button colors based on current page
            SetPageButtonColors(currentPage, totalPages);
        }

        private void SetPageButtonColors(int currentPage, int totalPages)
        {
            Label[] pageButtons = { page1, page2, page3, page4, page5 };

            for (int i = 0; i < pageButtons.Length; i++)
            {
                if (i + 1 == currentPage)
                {
                    pageButtons[i].BackColor = Color.Blue; // Set current page to blue
                    pageButtons[i].ForeColor = Color.White; // Set text color for current page
                }
                else
                {
                    pageButtons[i].BackColor = Color.Gray; // Set other pages to gray
                    pageButtons[i].ForeColor = Color.Black; // Set text color for other pages
                }
            }
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            currentPage = 1; // Сброс страницы при новом поиске
            string searchTerm = search.Text; // Получаем текст из текстового поля поиска
            LoadUsers(searchTerm); // Загружаем пользователей с учетом текста поиска
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.RowHeadersVisible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
            }
            else
            {
                // Если мы на первой странице, возвращаемся на последнюю страницу
                int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                currentPage = totalPages;
            }
            LoadUsers(search.Text);
        }

        private void pointer_arrow1_Click(object sender, EventArgs e) // Left arrow
        {
            if (currentPage > 1)
            {
                currentPage--;
            }
            else
            {
                // Если мы на первой странице, возвращаемся на последнюю страницу
                int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                currentPage = totalPages;
            }
            LoadUsers(search.Text);
        }

        private void pointer_arrow2_Click(object sender, EventArgs e) // Right arrow
        {
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (currentPage < totalPages)
            {
                currentPage++;
            }
            else
            {
                // Если мы на последней странице, возвращаемся на первую страницу
                currentPage = 1;
            }
            LoadUsers(search.Text);
        }


            private void number_of_pages_Click(object sender, EventArgs e)
        {

        }
        private void page1_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadUsers(search.Text);
        }

        private void page2_Click(object sender, EventArgs e)
        {
            currentPage = 2;
            LoadUsers(search.Text);
        }

        private void page3_Click(object sender, EventArgs e)
        {
            currentPage = 3;
            LoadUsers(search.Text);
        }

        private void page4_Click(object sender, EventArgs e)
        {
            currentPage = 4; // Adjust this as necessary based on total pages
            LoadUsers(search.Text);
        }

        private void page5_Click(object sender, EventArgs e)
        {
            currentPage = 5; // Adjust this as necessary based on total pages
            LoadUsers(search.Text);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // убедитесь, что строка выбрана
            {
                int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
                UserDetail detailForm = new UserDetail(userId);
                this.Visible = false;
                detailForm.ShowDialog();
                this.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int r = e.RowIndex;
                dataGridView1.Rows[r].Selected = true;
            }
        }
    }
}


