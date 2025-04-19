namespace Liv_In_Paris
{
    public class ChgInfo : Form
    {
        #region Attributs
        private Dictionary<string, string> userInfo;
        private string userType;
        private TableLayoutPanel layout;
        private TextBox T_Nom, T_Prenom, T_Email, T_Tel, T_NumRue, T_Rue, T_CP, T_Ville, T_NomEnt, T_NomRef, T_TelRef, T_MDP;
        private Button BtnSave, BtnCancel, Chg_Photo;
        private PictureBox profilepicture;

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
            this.Text = $"Modification des informations du profil de l'utilisateur {userInfo["Id"]}";
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
                T_Nom = CreateLabelEtTextBox("Nom", userInfo.ContainsKey("Nom") ? userInfo["Nom"] : "");
                T_Prenom = CreateLabelEtTextBox("Prénom", userInfo.ContainsKey("Prenom") ? userInfo["Prenom"] : "");
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
                if(T_Nom.Text.Length > 50 || T_Nom.Text == "")
                {
                    MessageBox.Show("Le Nom ne doit pas dépasser 50 caractères ni être vide.");
                    return;
                }
                userInfo["Nom"] = T_Nom.Text;

                if(T_Prenom.Text.Length > 50 || T_Prenom.Text == "")
                {
                    MessageBox.Show("Le Prénom ne doit pas dépasser 50 caractères ni être vide.");
                    return;
                }
                userInfo["Prenom"] = T_Prenom.Text;

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
    }
}

