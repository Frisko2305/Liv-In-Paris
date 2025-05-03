using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Login_Form : Form
    {
        #region Attributs
        private Label ID, PWD;
        private TextBox Id_box, Pwd_box;
        private Button Login, Retour;
        private TableLayoutPanel? layout;

        #endregion

        public Login_Form()
        {
            #region Initialisation Attributs

            this.Text = "Login";
            this.Size = new Size(350,175);
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

            #endregion

            #region Ajout et Création

            layout.Controls.Add(ID,0,0);
            layout.Controls.Add(Id_box,0,1);
            layout.Controls.Add(PWD,1,0);
            layout.Controls.Add(Pwd_box,1,1);

            this.Controls.Add(Login);
            this.Controls.Add(Retour);
            this.Controls.Add(layout);

            #endregion
        }

        #region Méthode Boutons
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
                SELECT
                    c.Id_client AS Id_Client,
                    cu.Id_cuisinier AS Id_Cuisinier,
                    CASE
                        WHEN cu.Id_cuisinier IS NOT NULL AND c.Id_client IS NULL THEN cu.Mdp
                        WHEN cu.Id_cuisinier IS NULL AND c.Id_client IS NOT NULL THEN c.Mdp
                        ELSE c.Mdp
                    END AS Mdp,

                    CASE
                        WHEN cu.Id_cuisinier IS NOT NULL THEN cu.Nom_cuisinier
                        ELSE c.Nom_client
                    END AS Nom,
                    CASE
                        WHEN cu.Id_cuisinier IS NOT NULL THEN cu.Prenom_cuisinier
                        ELSE c.Prenom_client
                    END AS Prenom,
                    
                    c.SIRET_entreprise AS SIRET,
                    e.Nom_entreprise AS Nom_entreprise,
                    e.Nom_referent AS Nom_referent,
                    e.Num_tel_referent AS Num_tel_referent,
                    p.Email AS Email,
                    p.Num_tel AS Particulier_Num_tel,
                    p.Num_Rue AS Particulier_Num_Rue,
                    p.Rue AS Particulier_Rue,
                    p.CP AS Particulier_CP,
                    p.Ville AS Particulier_Ville,
                    e.Num_Rue AS Entreprise_Num_Rue,
                    e.Rue AS Entreprise_Rue,
                    e.CP AS Entreprise_CP,
                    e.Ville AS Entreprise_Ville,
                    
                    CASE
                        WHEN cu.Id_cuisinier IS NOT NULL THEN cu.Photo_profil
                        ELSE c.Photo_profil
                    END AS Photo_profil
                    
                    FROM client c
                    LEFT JOIN particulier p ON c.Nom_client = p.Nom AND c.Prenom_client = p.Prenom
                    LEFT JOIN entreprise e ON c.SIRET_entreprise = e.SIRET
                    LEFT JOIN cuisinier cu ON c.Nom_client = cu.Nom_cuisinier AND c.Prenom_client = cu.Prenom_cuisinier
                    
                    WHERE
                        (@Id = c.Id_client AND @pwd = c.Mdp)
                        OR (@Id = cu.Id_cuisinier AND @pwd = cu.Mdp)";
            // A noter : si'l trouve un Id de cuisinier, il prendra celui la au lieu du client même si on saisi un id client
            // Il faut alors adapter la logique lors du return
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
                                { "Id_client", reader["Id_Client"]?.ToString() ?? "" },
                                { "Id_cuisinier", reader["Id_Cuisinier"]?.ToString() ?? "" },
                                { "Nom", reader["Nom"]?.ToString() ?? "" },
                                { "Prenom", reader["Prenom"]?.ToString() ?? "" },
                                { "SIRET", reader["SIRET"]?.ToString() ?? "" },
                                { "Email", reader["Email"]?.ToString() ?? "" },
                                { "Mdp", reader["Mdp"]?.ToString() ?? "" },

                                // Infos des Particuliers
                                { "Part_NumTel", reader["Particulier_Num_tel"]?.ToString() ?? "" },
                                { "Part_NumRue", reader["Particulier_Num_Rue"]?.ToString() ?? "" },
                                { "Part_Rue", reader["Particulier_Rue"]?.ToString() ?? "" },
                                { "Part_CP", reader["Particulier_CP"]?.ToString() ?? "" },
                                { "Part_Ville", reader["Particulier_Ville"]?.ToString() ?? "" },

                                // Infos des Entreprises
                                { "Ent_Nom", reader["Nom_entreprise"]?.ToString() ?? "" },
                                { "Ent_NumRue", reader["Entreprise_Num_Rue"]?.ToString() ?? "" },
                                { "Ent_Rue", reader["Entreprise_Rue"]?.ToString() ?? "" },
                                { "Ent_CP", reader["Entreprise_CP"]?.ToString() ?? "" },
                                { "Ent_Ville", reader["Entreprise_Ville"]?.ToString() ?? "" },
                                { "Ent_NomRef", reader["Nom_referent"]?.ToString() ?? "" },
                                { "Ent_TelRef", reader["Num_tel_referent"]?.ToString() ?? "" }
                            };

                            // On récupère l'image du profil s'il existe
                            if(!reader.IsDBNull(reader.GetOrdinal("Photo_profil")))
                            {
                                byte[] Photo = (byte[])reader["Photo_profil"];
                                userInfo.Add("Photo_profil", Convert.ToBase64String(Photo));
                            }

                            if(!string.IsNullOrEmpty(userInfo["Id_cuisinier"]) && userId == userInfo["Id_cuisinier"])       //Cas Cuisinier 
                            {
                                return ("Cuisinier", userInfo);
                            }
                            else if (!string.IsNullOrEmpty(userInfo["Id_client"]) && userId == userInfo["Id_client"])       //Cas Client (Part ou Ent)
                            {
                                if(!string.IsNullOrEmpty(userInfo["SIRET"]))    //Cas Ent
                                {
                                    return ("Entreprise", userInfo);
                                }
                                else
                                {
                                    return ("Particulier", userInfo);
                                }
                            }
                            else
                            {
                                return("Invalide", null);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Erreur de connexion à la base de donnée : "+e.Message);
            }
            return ("Invalide", null);
        }

        #endregion
    }
}