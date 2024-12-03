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

namespace Cybersport
{
    public partial class Tournaments : Form
    {
        private ContextMenuStrip contextMenuStrip;


        public Tournaments()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации

            this.FormClosing += new FormClosingEventHandler(Tournaments_FormClosing);
            InitializeComponent();
            contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem viewResultsItem = new ToolStripMenuItem("Просмотреть результаты турнира");
            viewResultsItem.Click += ViewResultsItem_Click;
            contextMenuStrip.Items.Add(viewResultsItem);
            dataGridView1.MouseDown += dataGridView1_MouseDown;
        }
        string connect = data.conStr;

        private void button4_Click(object sender, EventArgs e)
        {
            Player player = new Player();
            this.Visible = false;
            player.ShowDialog();
            this.Close();
        }

        private void ViewResultsItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                string tournamentName = dataGridView1.Rows[selectedRowIndex].Cells["Название"].Value.ToString();
                int tournamentId = GetTournamentID(tournamentName);

                string searchFilter = textBox1.Text.Trim();
                string genreFilter = (comboBox2.SelectedIndex != -1) ? comboBox2.SelectedItem.ToString() : string.Empty;
                string sortOrder = (comboBox1.SelectedIndex != -1) ? comboBox1.SelectedItem.ToString() : string.Empty;

                this.Visible = false;
                TournamentResults tournamentResults = new TournamentResults(tournamentId);
                tournamentResults.ShowDialog();
                this.Close();
            }
        }

        private int GetTournamentID(string tournamentName)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand command = new MySqlCommand("SELECT TournamentID FROM Tournaments WHERE TournamentName = @TournamentName", con);
                command.Parameters.AddWithValue("@TournamentName", tournamentName);

                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration_on_tournament registration_On_Tournament = new Registration_on_tournament();
            this.Visible = false;
            registration_On_Tournament.ShowDialog();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        public void Tournaments_Load(object sender, EventArgs e)
        {
            comboBox1.KeyPress += new KeyPressEventHandler(OnKeyPress);
            comboBox2.KeyPress += new KeyPressEventHandler(OnKeyPress);

            comboBox1.Items.Add("По возрастанию");
            comboBox1.Items.Add("По убыванию");

            comboBox2.Items.Add("Сбросить фильтрацию");
            comboBox2.Items.Add("MOBA");
            comboBox2.Items.Add("Action");
            comboBox2.Items.Add("Shooter");
            comboBox2.Items.Add("Adventure");
            comboBox2.Items.Add("MMORPG");



            dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = connect;

                con.Open();

                MySqlCommand cmd = new MySqlCommand(@"
        SELECT 
            Tournaments.TournamentName AS Название, 
            Tournaments.StartDate AS 'Дата начала', 
            Tournaments.EndDate AS 'Дата окончания',
            Tournaments.GameType AS 'Жанр игр', 
            CASE 
                WHEN Tournaments.StartDate > NOW() THEN 'Предстоящий'
                WHEN Tournaments.EndDate < NOW() THEN 'Завершённый'
                ELSE 'Текущий'
            END AS 'Статус'
        FROM 
            Tournaments
    ", con);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Rows[0].Cells[0].Selected = false;

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                LoadFilteredData();
            }
        }

        void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Player player = new Player();
            this.Visible = false;
            player.ShowDialog();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }
        string FillDataGridView(string search = "", string genreFilter = "")
        {
            string query = @"
    SELECT 
        Tournaments.TournamentName AS Название,
        Tournaments.StartDate AS 'Дата начала',
        Tournaments.EndDate AS 'Дата окончания',
        Tournaments.GameType AS 'Жанр игр',
        CASE 
            WHEN Tournaments.StartDate > NOW() THEN 'Предстоящий'
            WHEN Tournaments.EndDate < NOW() THEN 'Завершённый'
            ELSE 'Текущий'
        END AS 'Статус'
    FROM 
        Tournaments";

            List<string> conditions = new List<string>();

            // Добавляем условие для поиска по названию
            if (!string.IsNullOrEmpty(search))
            {
                conditions.Add($"Tournaments.TournamentName LIKE '%{search}%'");
            }

            // Добавляем условие для фильтрации по жанру
            if (!string.IsNullOrEmpty(genreFilter))
            {
                conditions.Add($"Tournaments.GameType = '{genreFilter}'");
            }

            // Если есть условия, объединяем их в WHERE
            if (conditions.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditions);
            }

            // Добавляем сортировку, если выбрана
            if (comboBox1.SelectedIndex != -1)
            {
                string sortOrder = comboBox1.SelectedItem.ToString() == "По возрастанию" ? "ASC" : "DESC";
                query += " ORDER BY Tournaments.StartDate " + sortOrder; // Сортируем по дате турнира
            }

            return query;
        }
        void LoadData(string query)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand command = new MySqlCommand(query, con);
                DataTable dt = new DataTable();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }

                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;

                // Поведение выбора строк в DataGridView
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Selected = false; // Убираем выделение с первой строки
                }
            }
        }
        void load(string search)
        {
            MySqlConnection con = new MySqlConnection(data.conStr);
            con.Open();
            MySqlCommand command = new MySqlCommand(search, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            command.ExecuteNonQuery();
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;

            dataGridView1.AllowUserToAddRows = false;

            con.Close();
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
                   (c == 32) ||
                   (c >= '0' && c <= '9');
        }
        private void UpdateButtonState()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                string tournamentStatus = selectedRow.Cells["Статус"].Value.ToString();

                if (tournamentStatus == "Предстоящий")
                {
                    button2.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                }
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                string tournamentStatus = selectedRow.Cells["Статус"].Value.ToString();

                if (tournamentStatus == "Предстоящий")
                {
                    string tournamentName = selectedRow.Cells["Название"].Value.ToString();
                    DateTime startDate = Convert.ToDateTime(selectedRow.Cells["Дата начала"].Value);
                    DateTime endDate = Convert.ToDateTime(selectedRow.Cells["Дата окончания"].Value);
                    string gameType = selectedRow.Cells["Жанр игр"].Value.ToString();

                    Registration_on_tournament confirmationForm = new Registration_on_tournament();
                    confirmationForm.SetTournamentDetails(tournamentName, startDate, endDate, gameType);
                    this.Visible = false;
                    confirmationForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Вы не можете подать заявку на турнир, так как его статус - " + tournamentStatus + ".");
                }
            }
            else
            {

                MessageBox.Show("Пожалуйста, выберите турнир для регистрации.");

            }
        }

        private void LoadFilteredData()
        {
            string search = textBox1.Text.Trim();
            string genreFilter = string.Empty;

            if (comboBox2.SelectedIndex != -1 && comboBox2.SelectedItem.ToString() != "Сбросить фильтрацию")
            {
                genreFilter = comboBox2.SelectedItem.ToString();
            }

            string query = FillDataGridView(search, genreFilter);
            LoadData(query);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Статус")
            {
                switch (e.Value?.ToString())
                {
                    case "Предстоящий":
                        e.CellStyle.BackColor = Color.LightGreen;
                        break;
                    case "Завершённый":
                        e.CellStyle.BackColor = Color.LightGray;
                        break;
                    case "Текущий":
                        e.CellStyle.BackColor = Color.LightBlue;
                        break;
                    default:
                        break;
                }
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем текст из текстбокса перед удалением
            string search = textBox1.Text.Trim();
            string genreFilter = string.Empty;

            if (comboBox2.SelectedItem != null)
            {
                // Если выбрано "Сбросить фильтрацию"
                if (comboBox2.SelectedItem.ToString() == "Сбросить фильтрацию")
                {
                    // Сбрасываем фильтры
                    textBox1.Text = string.Empty; // очистка текстбокса
                    comboBox2.SelectedIndex = -1; // отмена выделения жанра

                    // обновляем данные без фильтрации
                    LoadFilteredData();
                }
                else
                {
                    // Устанавливаем фильтры по жанру
                    genreFilter = comboBox2.SelectedItem.ToString();
                    LoadFilteredData();
                }

            }
        }
            private void comboBox2_DropDown(object sender, EventArgs e)
            {
                if (comboBox2.Items.Contains("Фильтрация по жанру"))
                    comboBox2.Items.Remove("Фильтрация по жанру");
            }

        private void dataGridView1_ContextMenuStripChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = dataGridView1.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    dataGridView1.ClearSelection(); 
                    dataGridView1.Rows[hitTestInfo.RowIndex].Selected = true; 
                    contextMenuStrip.Show(dataGridView1, e.Location); 
                }
            }
        }

        private void Tournaments_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
