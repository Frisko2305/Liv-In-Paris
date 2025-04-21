using System.Text.Json.Serialization;
using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class ChgInfo : Form
    {
        #region Attributs
        private Dictionary<string, string> userInfo;
        private string userType;
        private TableLayoutPanel layout;
        private TextBox T_Email, T_Tel, T_NumRue, T_Rue, T_CP, T_Ville, T_NomEnt, T_NomRef, T_TelRef, T_MDP;
        private Button BtnSave, BtnCancel, Chg_Photo;
        private PictureBox? profilepicture;

        // Les labels correspondants aux TextBox seront ajoutés directement avec des méthodes
        #endregion
        public ChgInfo(string userType, Dictionary<string, string> userInfo)
        {
            this.userType = userType;
            this.userInfo = userInfo;
            CreationForm();
        }

        private void CreationForm()
        {
            if(userType == "Particulier" || userType == "Entreprise")
            {
                this.Text = $"Modification des informations du profil de l'utilisateur {userInfo["Id_client"]}";
            }
            else
            {
                this.Text = $"Modification des informations du profil de l'utilisateur {userInfo["Id_cuisinier"]}";
            }
            this.Width = 500;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            // Ajout photo de profil
            profilepicture = new PictureBox{ Size = new Size(200,200), SizeMode = PictureBoxSizeMode.Zoom };

            // Boutons
            Chg_Photo = new Button
            {
                Text = "Changer de photo :",
                AutoSize = true, Anchor = AnchorStyles.None
            };
            Chg_Photo.Click += Chg_Photo_Click;

            BtnSave = new Button { Text = "Enregistrer"};
            BtnSave.Click += BtnSave_Click;

            BtnCancel = new Button { Text = "Annuler"};
            BtnCancel.Click += BtnCancel_Click;

            if(userType == "Particulier" || userType == "Cuisinier")
            {
                T_Email = CreateLabelEtTextBox("Email", userInfo.ContainsKey("Email") ? userInfo["Email"] : "");
                T_Tel = CreateLabelEtTextBox("Numéro de Téléphone", userInfo.ContainsKey("Part_NumTel") ? userInfo["Part_NumTel"] : "");
                T_NumRue = CreateLabelEtTextBox("Numéro de rue", userInfo.ContainsKey("Part_NumRue") ? userInfo["Part_NumRue"] : "");
                T_Rue = CreateLabelEtTextBox("Rue", userInfo.ContainsKey("Part_Rue") ? userInfo["Part_Rue"] : "");
                T_CP = CreateLabelEtTextBox("CP", userInfo.ContainsKey("Part_CP") ? userInfo["Part_CP"] : "");
                T_Ville = CreateLabelEtTextBox("Ville", userInfo.ContainsKey("Part_Ville") ? userInfo["Part_Ville"] : "");
                T_MDP = CreateLabelEtTextBox("Mot de passe", userInfo.ContainsKey("Mdp") ? userInfo["Mdp"] : "");

            }
            else    //Cas Entreprise
            {
                T_NomEnt = CreateLabelEtTextBox("Nom de l'entreprise", userInfo.ContainsKey("Ent_Nom") ? userInfo["Ent_Nom"] : "");
                T_NomRef = CreateLabelEtTextBox("Nom du référent", userInfo.ContainsKey("Ent_NomRef") ? userInfo["Ent_NomRef"] : "");
                T_TelRef = CreateLabelEtTextBox("Téléphone du référent", userInfo.ContainsKey("Ent_TelRef") ? userInfo["Ent_TelRef"] : "");
                T_NumRue = CreateLabelEtTextBox("Numéro de rue", userInfo.ContainsKey("Ent_NumRue") ? userInfo["Ent_NumRue"] : "");
                T_Rue = CreateLabelEtTextBox("Rue", userInfo.ContainsKey("Ent_Rue") ? userInfo["Ent_Rue"] : "");
                T_CP = CreateLabelEtTextBox("CP", userInfo.ContainsKey("Ent_CP") ? userInfo["Ent_CP"] : "");
                T_Ville = CreateLabelEtTextBox("Ville", userInfo.ContainsKey("Ent_Ville") ? userInfo["Ent_Ville"] : "");
                T_MDP = CreateLabelEtTextBox("Mot de passe", userInfo.ContainsKey("Mdp") ? userInfo["Mdp"] : "");
            }

            if(userInfo.ContainsKey("Photo_profil") && !string.IsNullOrEmpty(userInfo["Photo_profil"]))
            {
                try
                {
                    byte[] imagesBytes = Convert.FromBase64String(userInfo["Photo_profil"]);
                    using(var mem = new MemoryStream(imagesBytes))
                    {
                        profilepicture.Image = Image.FromStream(mem); //Charge l'image depuis byte[] vers la PictureBox
                    }
                }
                catch
                {
                    profilepicture.Image = null;    //Si pas d'image ou corrompue
                }
            }

            layout.Controls.Add(profilepicture);
            layout.Controls.Add(Chg_Photo);
            layout.Controls.Add(BtnSave, 0, layout.RowCount-1);
            layout.Controls.Add(BtnCancel, 1, layout.RowCount-1);     //On mets RowCount car on s'embete pas de calculer le nb de lignes après avoir ajouté tout les champs

            this.Controls.Add(layout);
        }

        #region Méthode Label/Bouton
        private TextBox CreateLabelEtTextBox(string label, string value)
        {
            Label lbl = new Label { Text = label, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            TextBox txt = new TextBox { Text = value, Dock = DockStyle.Fill };

            layout.Controls.Add(lbl);
            layout.Controls.Add(txt);

            return txt;     //On return le TextBox car il nous faudra accéder à son .Text pour les vérifications
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            Profil Profil_User = new Profil(userType, userInfo);    //On transmet les informations au Form Profil
            Profil_User.Show();

            this.Hide();
            Profil_User.FormClosed += (s,args) => this.Close();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // On vérifie d'abord si les champs sont bien remplis selon le profil
            if(userType == "Particulier" || userType == "Cuisinier")
            {
                if(T_Tel.Text.Length != 14)
                {
                    MessageBox.Show("Le numéro de Téléphone doit être saisi en format XX XX XX XX XX ni être vide.");
                    return;
                }
                userInfo["Part_NumTel"] = T_Tel.Text;

                if(T_Email.Text.Length > 100 || T_Email.Text == "" || T_Email.Text.Contains('@')  == false)
                {
                    MessageBox.Show("L'email ne doit pas dépasser 50 caractère ni être vide et doit contenir un '@' et un domaine");
                    return;
                }
                userInfo["Email"] = T_Email.Text;

                if(!int.TryParse(T_NumRue.Text, out _) || T_NumRue.Text == "")
                {
                    MessageBox.Show("Le numéro de la rue doit être un entier valide, sans prendre en compte BIS, TER, etc, ni être vide.");
                    return;
                }
                userInfo["Part_NumRue"] = T_NumRue.Text;

                if(T_Rue.Text.Length > 50 || T_Rue.Text == "")
                {
                    MessageBox.Show("Le nom de la rue ne doit pas dépasser 50 caractères ni être vide.");
                    return;
                }
                userInfo["Part_Rue"] = T_Rue.Text;

                if(!int.TryParse(T_CP.Text, out _) || T_CP.Text == "")
                {
                    MessageBox.Show("Le code postal doit être un entier valide de format XXXXX, ni être vide");
                    return;
                }
                userInfo["Part_CP"] = T_CP.Text;

                if(T_Ville.Text.Length > 50 || T_Ville.Text == "")
                {
                    MessageBox.Show("Le nom de la ville ne doit pas dépasser 50 caractères, ni être vide.");
                    return;
                }
                userInfo["Part_Ville"] = T_Ville.Text;

                if(T_MDP.Text.Length > 20 || T_MDP.Text == "")
                {
                    MessageBox.Show("Le mot de passe ne doit pas dépasser 20 caractères, ni être vide.");
                    return;
                }
                userInfo["Mdp"] = T_MDP.Text;

                // Verif SQL
                UpdateDatabase();

            }
            else    //Cas Entreprise
            {
                if(T_NomEnt.Text.Length > 50 || T_NomEnt.Text == "")
                {
                    MessageBox.Show("Le nom de l'entreprise ne doit pas dépasser 50 caractères, ni être vide.");
                    return;
                }
                userInfo["Ent_Nom"] = T_NomEnt.Text;

                if(T_NomRef.Text.Length > 50 || T_NomRef.Text == "")
                {
                    MessageBox.Show("Le nom du référent ne doit pas dépasser 50 caractères, ni être vide.");
                    return;
                }
                userInfo["Ent_NomRef"] = T_NomRef.Text;

                if(T_TelRef.Text.Length != 14 || T_TelRef.Text == "")
                {
                    MessageBox.Show("Le numéro de téléphone du référent doit être saisi au format XX XX XX XX XX, ni être vide.");
                    return;
                }
                userInfo["Ent_TelRef"] = T_TelRef.Text;

                if(!int.TryParse(T_NumRue.Text, out _) || T_NumRue.Text == "")
                {
                    MessageBox.Show("Le numéro de la rue doit être un entier valide, sans prendre en compte BIS, TER, etc, ni être vide.");
                    return;
                }
                userInfo["Ent_NumRue"] = T_NumRue.Text;

                if(T_Rue.Text.Length > 50 || T_Rue.Text == "")
                {
                    MessageBox.Show("La rue ne doit pas dépasser 50 caractères, ni être vide.");
                    return;
                }
                userInfo["Ent_Rue"] = T_Rue.Text;

                if(!int.TryParse(T_CP.Text, out _) || T_CP.Text == "")
                {
                    MessageBox.Show("Le code postal doit être saisi au format XXXXX, ni être vide.");
                    return;
                }
                userInfo["Ent_CP"] = T_CP.Text;

                if(T_Ville.Text.Length > 50 || T_Ville.Text == "")
                {
                    MessageBox.Show("Le nom de la ville ne doit pas dépasser 50 caractères, ni être vide.");
                    return;
                }
                userInfo["Ent_Ville"] = T_Ville.Text;

                if(T_MDP.Text.Length > 20 || T_MDP.Text == "")
                {
                    MessageBox.Show("Le mot de passe ne doit pas dépasser 20 caractères, ni être vide.");
                    return;
                }
                userInfo["Mdp"] = T_MDP.Text;

                // Verif SQL
                UpdateDatabase();
            }
        }

        private void Chg_Photo_Click(object? sender, EventArgs e)
        {
            using(OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                fileDialog.Title = "Sélectionnez une image";

                if(fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string PhotoPath = fileDialog.FileName;

                    try
                    {
                        profilepicture.Image = Image.FromFile(PhotoPath);

                        //Conversion de l'image en string adapté pour le dictionnaire userInfo
                        byte[] imageByte = File.ReadAllBytes(PhotoPath);
                        string base64image = Convert.ToBase64String(imageByte);

                        userInfo["Photo_profil"] = base64image;

                        MessageBox.Show("Mise à jour de la photo réalisée avec succès.");
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Erreur durant la mise à jour de la photo : {ex.Message}");
                    }
                }
            }
        }

        #endregion
        #region Mise à jour BDD
        private void UpdateDatabase()
        {
            bool IdentiteUpdated = false;
            bool ProfileUpdated = false;
            bool BothProfileUpdated = false;
            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root"))
                {
                    connection.Open();
                    string queryIdentite = "";
                    string queryProfil = "";
                    string queryClientToCuisinier = "";
                    string queryCuisinierToClient = "";

                    string queryCheckBothProfile = @"SELECT 1
                    FROM Client c
                    INNER JOIN Cuisinier cu ON c.Nom_client = cu.Nom_cuisinier AND c.Prenom_client = cu.Prenom_cuisinier
                    WHERE c.Id_client = @IdClient OR cu.Id_cuisinier = @IdCuisinier;";

                    bool hasBothProfiles = false;

                    // On vérifie si l'utilisateur a deux profils
                    using(MySqlCommand cmdCheck = new MySqlCommand(queryCheckBothProfile, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@IdClient", userInfo["Id_client"]);
                        cmdCheck.Parameters.AddWithValue("@IdCuisinier", userInfo["Id_cuisinier"]);
                        hasBothProfiles = cmdCheck.ExecuteScalar() != null; 
                        //Devient true si l'utilisateur a les deux profiles
                    }

                    switch(userType)
                    {
                        case "Particulier" :
                            queryIdentite = @"UPDATE Particulier
                            SET Num_tel = @NumTel, Email = @Email, Num_Rue = @NumRue, Rue = @Rue, CP = @CP, Ville = @Ville
                            WHERE Nom = @Nom AND Prenom = @Prenom;
                            ";
                            queryProfil = @"UPDATE Client
                            SET Mdp = @Mdp, Photo_profil = @Photo
                            WHERE Id_client = @Id;
                            ";

                            queryClientToCuisinier = @"UPDATE Cuisinier
                            SET Mdp = @Mdp
                            WHERE Id_cuisinier = @IdCuisinier;
                            ";
                        break;

                        case "Entreprise" :
                            queryIdentite = @"UPDATE Entreprise
                            SET Nom_entreprise = @NomEnt, Nom_referent = @NomRef, Num_tel_referent = @TelRef, Num_Rue = @NumRue, Rue = @Rue, CP = @CP, Ville = @Ville
                            WHERE SIRET = @SIRET;
                            ";

                            queryProfil = @"UPDATE Client
                            SET Mdp = @Mdp, Photo_profil = @Photo
                            WHERE Id_client = @Id;
                            ";
                        break;

                        case "Cuisinier" :
                            queryIdentite = @"
                            UPDATE Particulier
                            SET Num_tel = @NumTel, Email = @Email, Num_Rue = @NumRue, Rue = @Rue, CP = @CP, Ville = @Ville
                            WHERE Nom = @Nom AND Prenom = @Prenom;
                            ";

                            queryProfil = @"UPDATE Cuisinier
                            SET Mdp = @Mdp, Photo_profil = @Photo
                            WHERE Id_cuisinier = @Id;
                            ";

                            queryCuisinierToClient = @"UPDATE Client
                            SET Mdp = @Mdp
                            WHERE Id_client = @IdClient;
                            ";
                        break;

                        default :
                            MessageBox.Show("Erreur de type de profil");
                        break;
                    }
                    

                    using(MySqlCommand cmdIdentite = new MySqlCommand(queryIdentite, connection))
                    {
                        if(userType == "Particulier" || userType == "Cuisinier")
                        {
                            cmdIdentite.Parameters.AddWithValue("@Nom", userInfo["Nom"]);
                            cmdIdentite.Parameters.AddWithValue("@Prenom", userInfo["Prenom"]);
                            cmdIdentite.Parameters.AddWithValue("@NumTel", userInfo["Part_NumTel"]);
                            cmdIdentite.Parameters.AddWithValue("@Email", userInfo["Email"]);
                            cmdIdentite.Parameters.AddWithValue("@NumRue", Convert.ToInt32(userInfo["Part_NumRue"]));
                            cmdIdentite.Parameters.AddWithValue("@Rue", userInfo["Part_Rue"]);
                            cmdIdentite.Parameters.AddWithValue("@CP", Convert.ToInt32(userInfo["Part_CP"]));
                            cmdIdentite.Parameters.AddWithValue("@Ville", userInfo["Part_Ville"]);
                        }
                        else    //Cas Entreprise
                        {
                            cmdIdentite.Parameters.AddWithValue("@NomEnt", userInfo["Ent_Nom"]);
                            cmdIdentite.Parameters.AddWithValue("@NomRef", userInfo["Ent_NomRef"]);
                            cmdIdentite.Parameters.AddWithValue("@TelRef", userInfo["Ent_TelRef"]);
                            cmdIdentite.Parameters.AddWithValue("@SIRET", Convert.ToInt64(userInfo["SIRET"]));
                            cmdIdentite.Parameters.AddWithValue("@NumRue", Convert.ToInt32(userInfo["Ent_NumRue"]));
                            cmdIdentite.Parameters.AddWithValue("@Rue", userInfo["Ent_Rue"]);
                            cmdIdentite.Parameters.AddWithValue("@CP", Convert.ToInt32(userInfo["Ent_CP"]));
                            cmdIdentite.Parameters.AddWithValue("@Ville", userInfo["Ent_Ville"]);
                        }

                        IdentiteUpdated = cmdIdentite.ExecuteNonQuery() > 0;    //Si nb de lignes retournés > 0 --> Vrai
                        cmdIdentite.Dispose();
                    }
                    
                    using(MySqlCommand cmdProfil = new MySqlCommand(queryProfil, connection))
                    {
                        if(userType == "Particulier" || userType == "Entreprise")
                        {
                            cmdProfil.Parameters.AddWithValue("@Id", userInfo["Id_client"]);
                        }
                        else    //Cas Cuisinier
                        {
                            cmdProfil.Parameters.AddWithValue("@Id", userInfo["Id_cuisinier"]);
                        }

                        cmdProfil.Parameters.AddWithValue("@Mdp", userInfo["Mdp"]);
                        if(userInfo.ContainsKey("Photo_profil") && !string.IsNullOrEmpty(userInfo["Photo_profil"]))
                        {
                            byte[] photobyte = Convert.FromBase64String(userInfo["Photo_profil"]);
                            cmdProfil.Parameters.AddWithValue("@Photo", photobyte);
                        }
                        else
                        {
                            cmdProfil.Parameters.AddWithValue("@Photo", DBNull.Value);
                        }

                        ProfileUpdated = cmdProfil.ExecuteNonQuery() > 0;    //Si nb de lignes retournés > 0 --> Vrai
                        cmdProfil.Dispose();
                    }
                    if(hasBothProfiles)
                    {
                        switch(userType)
                        {
                            case "Particulier" :
                                using(MySqlCommand cmdClientToCuisinier = new MySqlCommand(queryClientToCuisinier, connection))
                                {
                                    cmdClientToCuisinier.Parameters.AddWithValue("@Mdp", userInfo["Mdp"]);
                                    cmdClientToCuisinier.Parameters.AddWithValue("@IdCuisinier", userInfo["Id_cuisinier"]);
                                    BothProfileUpdated = cmdClientToCuisinier.ExecuteNonQuery() > 0;
                                    cmdClientToCuisinier.Dispose();
                                }
                                

                            break;

                            case "Cuisinier" :
                                using(MySqlCommand cmdCuisinierToClient = new MySqlCommand(queryCuisinierToClient, connection))
                                {
                                    cmdCuisinierToClient.Parameters.AddWithValue("@Mdp", userInfo["Mdp"]);
                                    cmdCuisinierToClient.Parameters.AddWithValue("@IdClient", userInfo["Id_client"]);
                                    BothProfileUpdated = cmdCuisinierToClient.ExecuteNonQuery() > 0;
                                    cmdCuisinierToClient.Dispose();
                                }
                            break;

                            default :
                                MessageBox.Show("Erreur de profil");
                            break;
                        }
                    }
                    connection.Close();

                    if(IdentiteUpdated && ProfileUpdated)
                    {
                        if(BothProfileUpdated)
                        {
                            MessageBox.Show("Modification dans vos deux profils enregistrés avex succès");
                        }
                        else
                        {
                            MessageBox.Show("Modifications enregistrés avec succès.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'enregistrement des modifications, veuillez réessayer.");
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement des modifications : {ex.Message}");
            }
        }

        #endregion
    }
}

