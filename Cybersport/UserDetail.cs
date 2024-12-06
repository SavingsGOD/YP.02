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
    public partial class UserDetail : Form
    {
        private string constr = data.conStr;
        private int userId;
        public UserDetail(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void UserDetail_Load(object sender, EventArgs e)
        {
            LoadUserDetails();
        }
        private void LoadUserDetails()
        {
            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Users WHERE UserID = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Заполнение полей формы полными данными
                        lblUsername.Text = reader["Username"].ToString();
                        lblEmail.Text = reader["Email"].ToString();
                        lblFIO.Text = reader["FIO"].ToString();
                        lblPhoneNumber.Text = reader["PhoneNumber"].ToString();
                        lblRole.Text = reader["Role"].ToString();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Users detailForm = new Users();
            this.Visible = false;
            detailForm.ShowDialog();
            this.Close();
        }
    }
}
