using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Login_Form : Form
    {
        private Label ID;
        private Label PWD;
        private TextBox Id_box;
        private TextBox Pwd_box;
        private TableLayoutPanel? layout;
        private Button Login;
        private Button Retour;

        public Login_Form()
        {
            this.Text = "Login";
            this.Width = 350;
            this.Height = 175;
            this.StartPosition = FormStartPosition.CenterScreen;    //Centre la fenêtre centre écran

            layout = new TableLayoutPanel();           //avec un layout
            layout.RowCount = 2; //ID then PWD with 1 for space in-between
            layout.ColumnCount = 2;  //Label then Field
            layout.Dock = DockStyle.Fill;       //Remplis les cases pour pouvoir y mettre les choses
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));  //Autosize pour ID
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));  //Autosize pour PWD

            ID = new Label();
            ID.Text = "ID";
            ID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            ID.AutoSize = true;

            PWD = new Label();
            PWD.Text = "Password";
            PWD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            PWD.AutoSize = true; 

            Id_box = new TextBox();
            Id_box.Width = 150;

            Pwd_box = new TextBox();
            Pwd_box.Width = 150;

            Login = new Button();
            Login.Text = "Login";
            Login.AutoSize = true;
            Login.Width = 100;
            Login.Location = new Point((this.ClientSize.Width /2) , 75);
            Login.Click += button_Click;

            Retour = new Button();
            Retour.Text = "Retour";
            Retour.AutoSize = true;
            Retour.Width = 100;
            Retour.Location = new Point((Retour.Width) /2, 75);
            Retour.Click += Retour_Click;  

            layout.Controls.Add(ID,0,0);
            layout.Controls.Add(Id_box,0,1);
            layout.Controls.Add(PWD,1,0);
            layout.Controls.Add(Pwd_box,1,1);

            this.Controls.Add(Login);
            this.Controls.Add(Retour);
            this.Controls.Add(layout);
        }

        private void button_Click(object? sender, EventArgs e)
        {
            string userId = Id_box.Text;
            string userpwd = Pwd_box.Text;
            
            var(userType, userInfo) = LoginValide(userId, userpwd);

            if(userType != "Invalide" && userInfo != null)  //Qu'importe qui que ce soit tant que que c'est pas null
            {
                MessageBox.Show("Login réussi");
                Profil Profil_User = new Profil(userType, userInfo);    //On transmet les informations au Form Profil
                Profil_User.Show();

                this.Hide();
                Profil_User.FormClosed += (s,args) => this.Close();
            }
            else
            {
                MessageBox.Show("Identifiant erroné");
            }
        }

        private void Retour_Click(object? sender, EventArgs e)
        {
            PrésentationForm Form = new PrésentationForm();
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }

        private (string userType, Dictionary<string, string>? UserInfo) LoginValide(string userId, string userpwd)
        {
            string connectionString = "SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root";
            string query =
                @"
                SELECT c.Id_client, c.Mdp, c.Nom_particulier, c.Prenom_particulier, c.SIRET_entreprise,
                p.Num_tel AS Particulier_Num_tel, p.email AS Particulier_Email,
                e.Nom_entreprise AS Entreprise_Nom, e.Nom_referent AS Entreprise_Referent,
                cu.Id_cuisinier AS Cuisinier_Id,
                CASE
                    WHEN cu.Id_cuisinier IS NOT NULL THEN cu.Photo_profil
                    ELSE c.Photo_profil
                END AS Photo_profil
                FROM client c
                LEFT JOIN particulier p ON c.Nom_particulier = p.Nom AND c.Prenom_particulier = p.Prenom
                LEFT JOIN entreprise e ON c.SIRET_entreprise = e.SIRET
                LEFT JOIN cuisinier cu ON c.Id_client = cu.Id_cuisinier
                WHERE c.Id_client = @Id AND c.MDP = @pwd;";

            try
            {
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", userId);
                    cmd.Parameters.AddWithValue("@pwd", userpwd);

                    using(MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            var userInfo = new Dictionary<string, string>
                            {
                                { "Id", reader["Id_client"]?.ToString() ?? "" },
                                { "Nom", reader["Nom_particulier"]?.ToString() ?? "" },
                                { "Prenom", reader["Prenom_particulier"]?.ToString() ?? "" },
                                { "SIRET", reader["SIRET_entreprise"]?.ToString() ?? "" },
                                { "Particulier_Num_tel", reader["Particulier_Num_tel"]?.ToString() ?? "" },
                                { "Particulier_Email", reader["Particulier_Email"]?.ToString() ?? "" },
                                { "Entreprise_Nom", reader["Entreprise_Nom"]?.ToString() ?? "" },
                                { "Entreprise_Referent", reader["Entreprise_Referent"]?.ToString() ?? "" }
                            };

                            // On récupère l'image du profil s'il existe
                            if(!reader.IsDBNull(reader.GetOrdinal("Photo_profil")))
                            {
                                byte[] Photo = (byte[])reader["Photo_profil"];
                                userInfo.Add("Photo_profil", Convert.ToBase64String(Photo));
                            }

                            if(!string.IsNullOrEmpty(userInfo["Nom"]) && !string.IsNullOrEmpty(userInfo["Prenom"]))     //Si les colonnes de string du nom et prenom ne sont pas vides
                            {
                                return ("Particulier", userInfo);
                            }
                            else if (!string.IsNullOrEmpty(userInfo["SIRET"]))
                            {
                                return ("Entreprise", userInfo);
                            }
                            else
                            {
                                return ("Cuisinier", userInfo);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Erreur de connexion à la base de donnée"+e.Message);
            }
            return ("Invalide", null);
        }
    }
}