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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int time = Convert.ToInt32(textBox1.Text);

            Properties.Settings.Default.Time = Convert.ToString(time * 1000);
            Properties.Settings.Default.Save();
            MessageBox.Show("Время бездействия изменено", "Время", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Authorization authorization = new Authorization();
            this.Visible = false;
            authorization.ShowDialog();
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            int timebox = Convert.ToInt32(Properties.Settings.Default.Time) / 1000;
            textBox1.Text = Convert.ToString(timebox);
        }
    }
}
