using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cybersport
{
    public partial class Tournaments_and_add_tournaments : Form
    {
        public Tournaments_and_add_tournaments()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }
        string connect = data.conStr;

        private void Tournaments_and_add_tournaments_Load(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = connect;

                con.Open();

                MySqlCommand cmd = new MySqlCommand(@"select TournamentName AS Название, StartDate AS 'Дата начала', EndDate AS 'Дата окончания', GameType AS 'Жанр игр', Status AS Статус
                    from Tournaments
                ", con);

                cmd.ExecuteNonQuery();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Rows[0].Cells[0].Selected = false;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Manager manager = new Manager();
            this.Visible = false;
            manager.ShowDialog();
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void Tournaments_and_add_tournaments_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
