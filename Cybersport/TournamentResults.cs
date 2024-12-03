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
    public partial class TournamentResults : Form
    {
        private int tournamentId;

        public TournamentResults(int tournamentId)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
            this.tournamentId = tournamentId;
            LoadTournamentResults();
        }

        private void LoadTournamentResults()
        {
            dataGridView1.RowHeadersVisible = false;
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand command = new MySqlCommand(@"
    SELECT 
        mr.MatchID AS MatchID,
        t.TournamentName AS 'Название турнира',
        ta.TeamName AS 'Команда А',
        tb.TeamName AS 'Команда Б',
        mr.ScoreTeamA AS 'Очки команды А',
        mr.ScoreTeamB AS 'Очки команды Б',
        mr.MatchDate AS 'Дата турнира',
        CASE 
            WHEN mr.Winner = mr.TeamAID THEN ta.TeamName 
            WHEN mr.Winner = mr.TeamBID THEN tb.TeamName 
            ELSE 'No Winner' 
        END AS Победитель
    FROM 
        MatchResults mr
    INNER JOIN 
        Tournaments t ON mr.TournamentID = t.TournamentID
    INNER JOIN 
        Teams ta ON mr.TeamAID = ta.TeamID
    INNER JOIN 
        Teams tb ON mr.TeamBID = tb.TeamID
    WHERE 
        mr.TournamentID = @TournamentID", con);

                command.Parameters.AddWithValue("@TournamentID", tournamentId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt; 

                // Скрываем столбец MatchID
                dataGridView1.Columns["MatchID"].Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Tournaments tournaments = new Tournaments();
            tournaments.ShowDialog();
            this.Close();
        }

        private void TournamentResults_Load(object sender, EventArgs e)
        {
            
        }

        private void TournamentResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
