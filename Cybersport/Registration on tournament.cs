using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cybersport
{
    public partial class Registration_on_tournament : Form
    {
        public Registration_on_tournament()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tournaments tournaments = new Tournaments();
            this.Visible = false;
            tournaments.ShowDialog();
            this.Close();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
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
                   (c >= 'а' && c <= 'я') ||
                   (c >= 'А' && c <= 'Я') ||
                   (c >= '0' && c <= '9');
        }

        private void Registration_on_tournament_Load(object sender, EventArgs e)
        {
            label14.Text = data.Login ;
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = data.conStr;

                con.Open();

                MySqlCommand cmd = new MySqlCommand(@"SELECT Users.Username, Users.Role
                FROM Users
                WHERE Users.Role = 'Участник'

;", con);

                IDataReader dataReader = cmd.ExecuteReader();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetValue(0).ToString());
                    comboBox2.Items.Add(dataReader.GetValue(0).ToString());
                    comboBox3.Items.Add(dataReader.GetValue(0).ToString());
                    comboBox4.Items.Add(dataReader.GetValue(0).ToString());
                }
            }
        }
        public void SetTournamentDetails(string tournamentName, DateTime startDate, DateTime endDate, string captain)
        {
            label11.Text = tournamentName;
            label12.Text = startDate.ToShortDateString();
            label13.Text = endDate.ToShortDateString();
            label14.Text = captain;

        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            string teamName = textBox4.Text;

            // Обработка случая, когда имя команды пустое
            if (string.IsNullOrWhiteSpace(teamName))
            {
                MessageBox.Show("Пожалуйста, введите имя команды.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Проверка на существование команды
            if (IsTeamNameExists(teamName))
            {
                MessageBox.Show("Команда с таким именем уже существует. Пожалуйста, выберите другое имя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Проверка, что все ComboBox заполнены
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null || comboBox4.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите всех участников команды.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string captainUsername = label14.Text = data.Login; // Имя капитана
            int tournamentId = GetCurrentTournamentId(); // Метод для получения ID текущего турнира

            // Получаем ID капитана из базы данных
            int captainId = GetUserIdByUsername(captainUsername);

            // Список для хранения имен участников
            HashSet<string> selectedPlayers = new HashSet<string>();

            // Проверка на уникальность участников
            foreach (ComboBox comboBox in new ComboBox[] { comboBox1, comboBox2, comboBox3, comboBox4 })
            {
                if (comboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(comboBox.SelectedItem.ToString()))
                {
                    string playerName = comboBox.SelectedItem.ToString();

                    // Проверка на совпадение капитана с участниками
                    if (playerName.Equals(captainUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Капитан не может быть участником команды.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Проверка уникальности
                    if (!selectedPlayers.Add(playerName))
                    {
                        MessageBox.Show($"Участник {playerName} уже добавлен в команду.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // Добавление команды в базу данных
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                try
                {
                    con.Open();

                    // 1. Добавление команды в таблицу Teams
                    MySqlCommand addTeamCmd = new MySqlCommand("INSERT INTO Teams (TeamName, CaptainID) VALUES (@TeamName, @CaptainID);", con);
                    addTeamCmd.Parameters.AddWithValue("@TeamName", teamName);
                    addTeamCmd.Parameters.AddWithValue("@CaptainID", captainId);
                    addTeamCmd.ExecuteNonQuery();

                    // 2. Получаем ID добавленной команды
                    int teamId = (int)addTeamCmd.LastInsertedId;

                    // 3. Регистрация команды на турнире
                    MySqlCommand registerTeamCmd = new MySqlCommand("INSERT INTO TournamentRegistrations (TournamentID, TeamID, CaptainID) VALUES (@TournamentID, @TeamID, @CaptainID);", con);
                    registerTeamCmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                    registerTeamCmd.Parameters.AddWithValue("@TeamID", teamId);
                    registerTeamCmd.Parameters.AddWithValue("@CaptainID", captainId);
                    registerTeamCmd.ExecuteNonQuery();

                    MySqlCommand teamsCmd = new MySqlCommand("INSERT INTO TournamentsTeams (idTournaments, idTeam) VALUES (@TournamentID, @TeamID);", con);
                    teamsCmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                    teamsCmd.Parameters.AddWithValue("@TeamID", teamId);
                    teamsCmd.ExecuteNonQuery();

                    // 4. Добавление игроков в соответствующие таблицы
                    AddPlayersToTeam(con, teamId);


                    // Получаем RegistrationID последней добавленной записи
                    int registrationId = (int)registerTeamCmd.LastInsertedId;

                    // Добавление игроков в TournamentTeamPlayers
                    foreach (ComboBox comboBox in new ComboBox[] { comboBox1, comboBox2, comboBox3, comboBox4 })
                    {
                        if (comboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(comboBox.SelectedItem.ToString()))
                        {
                            {
                                MySqlCommand teamPlayerCmd = new MySqlCommand("INSERT INTO TournamentTeamPlayers (RegistrationID) VALUES (@RegistrationID);", con);
                                teamPlayerCmd.Parameters.AddWithValue("@RegistrationID", registrationId);
                                teamPlayerCmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Команда успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // Очистка полей после успешной регистрации
                        ClearFormFields();
                        Tournaments tournaments = new Tournaments();
                        this.Visible = false;
                        this.Close();
                        tournaments.ShowDialog();

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Произошла ошибка при добавлении команды в базу данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла непредвиденная ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool IsTeamNameExists(string teamName)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Teams WHERE TeamName = @TeamName;", con);
                cmd.Parameters.AddWithValue("@TeamName", teamName);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0; // Если count больше 0, значит команда с таким именем уже существует
            }
        }
            // Метод для очистки полей 
            private void ClearFormFields()
        {
            textBox4.Clear();
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;
            comboBox4.SelectedItem = null;
        }


        private int GetUserIdByUsername(string username)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT UserID FROM Users WHERE Username = @Username;", con);
                cmd.Parameters.AddWithValue("@Username", username);

                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Возвращаем -1, чтобы указать, что пользователь не найден
                }

                return Convert.ToInt32(result);
            }
        }

        // Метод для добавления игроков в команду
        private void AddPlayersToTeam(MySqlConnection con, int teamId)
        {
            foreach (ComboBox comboBox in new ComboBox[] { comboBox1, comboBox2, comboBox3, comboBox4 })
            {
                if (comboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(comboBox.SelectedItem.ToString()))
                {
                    int playerId = GetUserIdByUsername(comboBox.SelectedItem.ToString());
                    if (playerId != -1) // Проверка на существование пользователя
                    {
                        MySqlCommand addPlayerCmd = new MySqlCommand("INSERT INTO Participants (UserID, TeamID) VALUES (@UserID, @TeamID);", con);
                        addPlayerCmd.Parameters.AddWithValue("@UserID", playerId);
                        addPlayerCmd.Parameters.AddWithValue("@TeamID", teamId);
                        addPlayerCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // Метод для получения ID турнира
        private int GetCurrentTournamentId()
        {
            return 1; // Заменить реальным ID
        }

        private void Registration_on_tournament_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
