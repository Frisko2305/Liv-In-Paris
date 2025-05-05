using OfficeOpenXml;
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
        private ComboBox C_Metro;
        private Button? BtnSave, BtnCancel, Chg_Photo;
        private PictureBox? profilepicture;
        private List<string> ListeMetro;


        #endregion
        public ChgInfo(string userType, Dictionary<string, string> userInfo)
        {
            this.userType = userType;
            this.userInfo = userInfo;

            ListeMetro = ChargerListeMetro(Path.Combine(Directory.GetCurrentDirectory(), "MetroParis.xlsx"));
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
            this.Size = new Size(500,600);
            this.StartPosition = FormStartPosition.CenterScreen;

            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            profilepicture = new PictureBox{ Size = new Size(200,200), SizeMode = PictureBoxSizeMode.Zoom };

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
                T_Email = AjoutLabelEtTextBox("Email", userInfo.ContainsKey("Email") ? userInfo["Email"] : "");
                T_Tel = AjoutLabelEtTextBox("Numéro de Téléphone", userInfo.ContainsKey("Part_NumTel") ? userInfo["Part_NumTel"] : "");
                T_NumRue = AjoutLabelEtTextBox("Numéro de rue", userInfo.ContainsKey("Part_NumRue") ? userInfo["Part_NumRue"] : "");
                T_Rue = AjoutLabelEtTextBox("Rue", userInfo.ContainsKey("Part_Rue") ? userInfo["Part_Rue"] : "");
                T_CP = AjoutLabelEtTextBox("CP", userInfo.ContainsKey("Part_CP") ? userInfo["Part_CP"] : "");
                T_Ville = AjoutLabelEtTextBox("Ville", userInfo.ContainsKey("Part_Ville") ? userInfo["Part_Ville"] : "");
                T_MDP = AjoutLabelEtTextBox("Mot de passe", userInfo.ContainsKey("Mdp") ? userInfo["Mdp"] : "");
                C_Metro = AjoutLabelEtComboBox("Station de métro la plus proche", userInfo.ContainsKey("Metro") ? userInfo["Metro"] : "", ListeMetro);

            }
            else
            {
                T_NomEnt = AjoutLabelEtTextBox("Nom de l'entreprise", userInfo.ContainsKey("Ent_Nom") ? userInfo["Ent_Nom"] : "");
                T_NomRef = AjoutLabelEtTextBox("Nom du référent", userInfo.ContainsKey("Ent_NomRef") ? userInfo["Ent_NomRef"] : "");
                T_TelRef = AjoutLabelEtTextBox("Téléphone du référent", userInfo.ContainsKey("Ent_TelRef") ? userInfo["Ent_TelRef"] : "");
                T_NumRue = AjoutLabelEtTextBox("Numéro de rue", userInfo.ContainsKey("Ent_NumRue") ? userInfo["Ent_NumRue"] : "");
                T_Rue = AjoutLabelEtTextBox("Rue", userInfo.ContainsKey("Ent_Rue") ? userInfo["Ent_Rue"] : "");
                T_CP = AjoutLabelEtTextBox("CP", userInfo.ContainsKey("Ent_CP") ? userInfo["Ent_CP"] : "");
                T_Ville = AjoutLabelEtTextBox("Ville", userInfo.ContainsKey("Ent_Ville") ? userInfo["Ent_Ville"] : "");
                T_MDP = AjoutLabelEtTextBox("Mot de passe", userInfo.ContainsKey("Mdp") ? userInfo["Mdp"] : "");
                C_Metro = AjoutLabelEtComboBox("Station de métro la plus proche", userInfo.ContainsKey("Metro") ? userInfo["Metro"] : "", ListeMetro);
            }

            if(userInfo.ContainsKey("Photo_profil") && !string.IsNullOrEmpty(userInfo["Photo_profil"]))
            {
                try
                {
                    byte[] imagesBytes = Convert.FromBase64String(userInfo["Photo_profil"]);
                    using(var mem = new MemoryStream(imagesBytes))
                    {
                        profilepicture.Image = Image.FromStream(mem);
                    }
                }
                catch
                {
                    profilepicture.Image = null;
                }
            }

            layout.Controls.Add(profilepicture);
            layout.Controls.Add(Chg_Photo);
            layout.Controls.Add(BtnSave, 0, layout.RowCount-1);
            layout.Controls.Add(BtnCancel, 1, layout.RowCount-1);

            this.Controls.Add(layout);
        }

        #region Méthode Label/Bouton
        private TextBox AjoutLabelEtTextBox(string label, string value)
        {
            Label lbl = new Label { Text = label, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            TextBox txt = new TextBox { Text = value, Dock = DockStyle.Fill };

            layout.Controls.Add(lbl);
            layout.Controls.Add(txt);

            return txt;
        }

        private ComboBox AjoutLabelEtComboBox(string label, string defaut, List<string> liste)
        {
            Label Label = new Label {Text = label, AutoSize = true, Anchor = AnchorStyles.Left};
            ComboBox ComboBox = new ComboBox();
            ComboBox.Items.AddRange(liste.ToArray());
            ComboBox.Text = defaut;
            ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            ComboBox.Width = 200;
            ComboBox.AutoSize = true;
            ComboBox.Anchor = AnchorStyles.Left;
            ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(liste.ToArray());
            ComboBox.AutoCompleteCustomSource = autoComplete;
            
            layout.Controls.Add(Label);
            layout.Controls.Add(ComboBox);
            return ComboBox;
        }

        private List<string> ChargerListeMetro(string cheminFichier)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
            HashSet<string> stationSet= new HashSet<string>();

            try
            {
                using(var package = new ExcelPackage(new FileInfo(cheminFichier)))
                {
                    var feuille = package.Workbook.Worksheets["Noeuds"];
                    if(feuille == null)
                    {
                        throw new Exception("La feuille 'Noeuds' est introuvable dans le fichier Excel");
                    }

                    int rowCount = feuille.Dimension.Rows;

                    for(int i = 2 ; i <= rowCount ; i++)
                    {
                        string station = feuille.Cells[i, 3].Text;
                        if(!string.IsNullOrWhiteSpace(station))
                        {
                            stationSet.Add(station);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Erreru lors du chargement des Stations : {ex.Message}");
            }
            return stationSet.ToList();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            Profil Profil_User = new Profil(userType, userInfo);
            Profil_User.Show();

            this.Hide();
            Profil_User.FormClosed += (s,args) => this.Close();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
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

                userInfo["Metro"] = C_Metro.Text;

                UpdateDatabase();

            }
            else
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

                userInfo["Metro"] = C_Metro.Text;

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

                    using(MySqlCommand cmdCheck = new MySqlCommand(queryCheckBothProfile, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@IdClient", userInfo["Id_client"]);
                        cmdCheck.Parameters.AddWithValue("@IdCuisinier", userInfo["Id_cuisinier"]);
                        hasBothProfiles = cmdCheck.ExecuteScalar() != null; 

                    }

                    switch(userType)
                    {
                        case "Particulier" :
                            queryIdentite = @"UPDATE Particulier
                            SET Num_tel = @NumTel, Email = @Email, Num_Rue = @NumRue, Rue = @Rue, CP = @CP, Ville = @Ville, Metro = @metro
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
                            SET Nom_entreprise = @NomEnt, Nom_referent = @NomRef, Num_tel_referent = @TelRef, Num_Rue = @NumRue, Rue = @Rue, CP = @CP, Ville = @Ville, Metro = @Metro
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
                            SET Num_tel = @NumTel, Email = @Email, Num_Rue = @NumRue, Rue = @Rue, CP = @CP, Ville = @Ville, Metro = @Metro
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
                            cmdIdentite.Parameters.AddWithValue("@Metro", userInfo["Metro"]);
                        }
                        else
                        {
                            cmdIdentite.Parameters.AddWithValue("@NomEnt", userInfo["Ent_Nom"]);
                            cmdIdentite.Parameters.AddWithValue("@NomRef", userInfo["Ent_NomRef"]);
                            cmdIdentite.Parameters.AddWithValue("@TelRef", userInfo["Ent_TelRef"]);
                            cmdIdentite.Parameters.AddWithValue("@SIRET", Convert.ToInt64(userInfo["SIRET"]));
                            cmdIdentite.Parameters.AddWithValue("@NumRue", Convert.ToInt32(userInfo["Ent_NumRue"]));
                            cmdIdentite.Parameters.AddWithValue("@Rue", userInfo["Ent_Rue"]);
                            cmdIdentite.Parameters.AddWithValue("@CP", Convert.ToInt32(userInfo["Ent_CP"]));
                            cmdIdentite.Parameters.AddWithValue("@Ville", userInfo["Ent_Ville"]);
                            cmdIdentite.Parameters.AddWithValue("@Metro", userInfo["Metro"]);
                        }

                        IdentiteUpdated = cmdIdentite.ExecuteNonQuery() > 0;
                        cmdIdentite.Dispose();
                    }
                    
                    using(MySqlCommand cmdProfil = new MySqlCommand(queryProfil, connection))
                    {
                        if(userType == "Particulier" || userType == "Entreprise")
                        {
                            cmdProfil.Parameters.AddWithValue("@Id", userInfo["Id_client"]);
                        }
                        else
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

                        ProfileUpdated = cmdProfil.ExecuteNonQuery() > 0;
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

                            Profil Profil_User = new Profil(userType, userInfo);
                            Profil_User.Show();

                            this.Hide();
                            Profil_User.FormClosed += (s,args) => this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Modifications enregistrés avec succès.");
                            
                            Profil Profil_User = new Profil(userType, userInfo);
                            Profil_User.Show();

                            this.Hide();
                            Profil_User.FormClosed += (s,args) => this.Close();
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

