using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Lab3
{
        public partial class Form1 : Form
        {
            public string connectionString = ConfigurationManager.ConnectionStrings["F1String"].ConnectionString;

            public Form1()
            {
                InitializeComponent();
            }

            private void Form1_Load(object sender, EventArgs e)
            {
                Reload();
            }

            private void Reload()
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sqlText = "select * from Teams";
                SqlCommand command = new SqlCommand(sqlText, connection);
                SqlDataReader reader = command.ExecuteReader();

                lstTeams.Items.Clear();

                int i = 0;
                while (reader.Read())
                {
                    lstTeams.Items.Add(reader["TeamID"].ToString());
                    lstTeams.Items[i].SubItems.Add(reader["TeamName"].ToString());
                    i++;
                }
                connection.Close();
            }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            connect.Open();

            try
            {

                int IdTeam = Int32.Parse(txtID.Text);
                string NameTeam = txtName.Text;

                string sqlUpdate = "update Teams set TeamName=@TeamName where TeamID = @TeamID";
                SqlCommand command1 = new SqlCommand(sqlUpdate, connect);
                command1.Parameters.AddWithValue("@TeamID", IdTeam);
                command1.Parameters.AddWithValue("@TeamName", NameTeam);


                int count = command1.ExecuteNonQuery();
                if (count < 1)
                {

                    string sqlText = "insert into Teams (TeamName) values (@TeamName)";
                    SqlCommand cmd = new SqlCommand(sqlText, connect);
                    cmd.Parameters.AddWithValue("@TeamName", NameTeam);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { }
            connect.Close();
            Reload();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            connect.Open();

            try
            {
                int IdTeam = Int32.Parse(lstTeams.SelectedItems[0].Text);

                string sqlText = "delete from Teams where TeamID = @TeamID";
                SqlCommand cmd = new SqlCommand(sqlText, connect);
                cmd.Parameters.AddWithValue("@TeamID", IdTeam);

                cmd.ExecuteNonQuery();
            }
            catch (Exception) { }
            connect.Close();
            Reload();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            connect.Open();

            try
            {
                int IdTeam = Int32.Parse(lstTeams.SelectedItems[0].Text);

                string sqlText = "select * from Teams where teamID = @TeamID";
                SqlCommand cmd = new SqlCommand(sqlText, connect);
                cmd.Parameters.AddWithValue("@TeamID", IdTeam);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                txtID.Text = reader["TeamID"].ToString();
                reader["TeamName"].ToString();
            }
            catch (Exception) { }
            connect.Close();
            Reload();
        }
    }
}
