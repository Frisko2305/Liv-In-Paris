using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Profil_Client_Part : Form
    {
        private string[]? infos;        

        public Profil_Client_Part(string userId)
        {
            
        }

        private string[]? Avoir_Infos(string userId)
        {
            string connectionString = "SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root";
            string query = $"SELECT * FROM client WHERE Id_client = @Id";
            string[] result = new string[5];
            try
            {
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", userId);

                    return result;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Erreur d'exécution : "+ e.Message);
                return null;
            }
        }
    }
}

