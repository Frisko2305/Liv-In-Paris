using OfficeOpenXml;
using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Insc_Form : Form
    {
        #region Attributs
        private Button Retour;
        private Label Choix_Part_Client_Cuisinier;
        private Label Hypertxt_Part, Hypertxt_Ent;     //Créer des 'liens' pour permettre de lire les conditions d'utilisations
        private CheckBox Part_Client, Part_Cuisinier;
        private CheckBox ConditionsPart, ConditionsEnt;
        private Button Choisir_Image_Part, Choisir_Image_Ent;
        private Button Insc_Part, Insc_Ent;
        private string? Image_path_Part, Image_path_Ent;
        private FlowLayoutPanel ParticulierFlow, EntrepriseFlow;
        private TableLayoutPanel ParticulierPanel, EntreprisePanel;
        private GroupBox ParticulierGroup, EntrepriseGroup;
        private TableLayoutPanel MainLayout;    //Permet de placer à gauche et droite de la page les Layout Part & Ent

        //Obligatoire de créer une variable pour chaque élément pour par la suite pouvoir les récupérer et créer le tuple associé
        private TextBox Nom_Part_Box, Prenom_Part_Box, Tel_Part_Box, Email_Part_Box, NumRue_Part_Box, Rue_Part_Box, CP_Part_Box, Ville_Part_Box, MDP_Part_Box, SIRET_Ent_Box, Nom_Ent_Box, NomRef_Ent_Box, TelRef_Ent_Box, NumRue_Ent_Box, Rue_Ent_Box, CP_Ent_Box, Ville_Ent_Box, MDP_Ent_Box;
        private ComboBox MetroPart, MetroEnt;
        private List<string> ListeMetro;

        #endregion

        public Insc_Form()
        {
            #region Initialisation Attributs
            
            this.Text = "Inscription to Liv In Paris !";
            this.Size = new Size(750,650);
            this.StartPosition = FormStartPosition.CenterScreen;    //Centre la fenêtre centre écran

            //On va charger l'entiereté des libelles des stations dans ListeMetro avec son chemin
            ListeMetro = ChargerListeMetro(Path.Combine(Directory.GetCurrentDirectory(), "MetroParis.xlsx"));

            // Create a TableLayoutPanel with 2 columns
            MainLayout = new TableLayoutPanel();
            MainLayout.Dock = DockStyle.Fill; // Fill the entire form
            MainLayout.ColumnCount = 2; // Two columns
            MainLayout.RowCount = 2; // One row
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Left column: 50% width
            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Right column: 50% width
            MainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Adapt visual when developping Part or Ent

            // Create the "Particulier" accordion
            ParticulierGroup = new GroupBox();
            ParticulierGroup.Text = "Particulier";
            ParticulierGroup.Dock = DockStyle.Top;
            ParticulierGroup.Dock = DockStyle.Fill;
            ParticulierGroup.AutoSize = true;
            ParticulierGroup.Click += (sender, e) => TogglePanelVisibility(ParticulierPanel);

            // Create the "Entreprise" accordion
            EntrepriseGroup = new GroupBox();
            EntrepriseGroup.Text = "Entreprise";
            EntrepriseGroup.Dock = DockStyle.Top;
            EntrepriseGroup.Dock = DockStyle.Fill;  //Remplis le Form
            EntrepriseGroup.AutoSize = true;
            EntrepriseGroup.Click += (sender, e) => TogglePanelVisibility(EntreprisePanel);

            // Create the TableLayoutPanel for "Particulier"
            ParticulierPanel = new TableLayoutPanel();
            ParticulierPanel.Dock = DockStyle.Fill;
            ParticulierPanel.RowCount = 13;
            ParticulierPanel.ColumnCount = 2;
            ParticulierPanel.AutoSize = true;
            ParticulierPanel.Visible = false; // Initially hidden

            // Create the TableLayoutPanel for "Entreprise"
            EntreprisePanel = new TableLayoutPanel();
            EntreprisePanel.Dock = DockStyle.Fill;
            EntreprisePanel.RowCount = 12;
            EntreprisePanel.ColumnCount = 2;
            EntreprisePanel.AutoSize = true;
            EntreprisePanel.Visible = false; // Initially hidden

            // The last row with all checkboxes is added separately
            ParticulierFlow = new FlowLayoutPanel();
            ParticulierFlow.Dock = DockStyle.Fill;
            ParticulierFlow.FlowDirection = FlowDirection.LeftToRight;
            ParticulierFlow.AutoSize = true;

            // The last row with the checkbox is added separately
            EntrepriseFlow = new FlowLayoutPanel();
            EntrepriseFlow.Dock = DockStyle.Fill;
            EntrepriseFlow.FlowDirection = FlowDirection.LeftToRight;
            EntrepriseFlow.AutoSize = true;

            // Ajout des boutons de base

            Retour = new Button{Text = "Retour", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right};
            Retour.Click += Retour_Click;

            Hypertxt_Part = new Label{Text = "Lire les conditions d'utilisations.", ForeColor = Color.Blue, AutoSize = true, Cursor = Cursors.Hand};
            Hypertxt_Part.Click += Hypertxt_Part_Click;

            Hypertxt_Ent = new Label{Text = "Lire les conditions d'utilisations.", ForeColor = Color.Blue, AutoSize = true, Cursor = Cursors.Hand};
            Hypertxt_Ent.Click += Hypertxt_Ent_Click;

            Choisir_Image_Part = new Button{Text = "Choisir une photo de profil", AutoSize = true, Anchor = AnchorStyles.Right};
            Choisir_Image_Part.Click += Choisir_Image_Click_Part;

            Choisir_Image_Ent = new Button{Text = "Choisir une photo de profil", AutoSize = true, Anchor = AnchorStyles.Right};
            Choisir_Image_Ent.Click += Choisir_Image_Click_Ent;

            //Creation of every TextBox/Labels

            //TextBox Particulier

            Nom_Part_Box = AjoutLabelEtTextBoxParticulier("Nom :", 0);
            Prenom_Part_Box = AjoutLabelEtTextBoxParticulier("Prenom :", 1);
            Tel_Part_Box = AjoutLabelEtTextBoxParticulier("Téléphone :", 2);
            Email_Part_Box = AjoutLabelEtTextBoxParticulier("Email :", 3);
            NumRue_Part_Box  = AjoutLabelEtTextBoxParticulier("Numéro de rue :", 4);
            Rue_Part_Box = AjoutLabelEtTextBoxParticulier("Rue :", 5);
            CP_Part_Box = AjoutLabelEtTextBoxParticulier("Code Postal :", 6);
            Ville_Part_Box = AjoutLabelEtTextBoxParticulier("Ville :", 7);
            MetroPart = AjoutLabelEtComboBoxParticulier("Station de métro la plus proche : ", ListeMetro, 8);
            MDP_Part_Box = AjoutLabelEtTextBoxParticulier("Mot de passe :", 9);

            //TextBox Entreprise

            SIRET_Ent_Box = AjoutLabelEtTextBoxEntreprise("N° SIRET :", 0);
            Nom_Ent_Box = AjoutLabelEtTextBoxEntreprise("Nom de l'entreprise :", 1);
            NomRef_Ent_Box = AjoutLabelEtTextBoxEntreprise("Nom du référent :", 2);
            TelRef_Ent_Box = AjoutLabelEtTextBoxEntreprise("Numéro de téléphone du référent :", 3);
            NumRue_Ent_Box = AjoutLabelEtTextBoxEntreprise("Numéro de rue :", 4);
            Rue_Ent_Box = AjoutLabelEtTextBoxEntreprise("Rue :", 5);
            CP_Ent_Box = AjoutLabelEtTextBoxEntreprise("Code Postal :", 6);
            Ville_Ent_Box = AjoutLabelEtTextBoxEntreprise("Ville :", 7);
            MetroEnt = AjoutLabelEtComboBoxEntreprise("Station de métro la plus proche : ", ListeMetro, 8);
            MDP_Ent_Box = AjoutLabelEtTextBoxEntreprise("Mot de passe :", 9);


            ConditionsPart = new CheckBox()
            {
                Text = "J'accepte les termes et conditions d'utilisation.",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            };

            ConditionsEnt = new CheckBox()
            {
                Text = "J'accepte les termes et conditions d'utilisation.",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            };
            
            Insc_Part = new Button {Text = "Valider ?", AutoSize = true, Width = 100};
            Insc_Part.Click += Insc_Part_Click;

            Choix_Part_Client_Cuisinier = new Label()
            {
                Text = "Vous souhaitez :"
            };
        
            Part_Client = new CheckBox()
            {
                Text = "Acheter des plats.",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            };

            Part_Cuisinier = new CheckBox()
            {
                Text = "Vendre des plats.",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            };



            Insc_Ent = new Button {Text = "Validé ?", AutoSize = true, Width = 100};
            Insc_Ent.Click += Insc_Ent_Click;

            #endregion

            #region Ajout et Création
            //ajout du checkbox au flowPanel
            ParticulierFlow.Controls.Add(ConditionsPart);
            ParticulierFlow.Controls.Add(Hypertxt_Part);
            ParticulierFlow.Controls.Add(Insc_Part);
            ParticulierFlow.Controls.Add(Choisir_Image_Part);

            //ajout du checkbox au flowPanel
            EntrepriseFlow.Controls.Add(ConditionsEnt);
            EntrepriseFlow.Controls.Add(Hypertxt_Ent);
            EntrepriseFlow.Controls.Add(Insc_Ent);
            EntrepriseFlow.Controls.Add(Choisir_Image_Ent);

            // Add fields to "Particulier" 

            //Ajout du FlowPanel au TablPanel en configurant son étendu
            ParticulierPanel.Controls.Add(ParticulierFlow, 0, 10);
            ParticulierPanel.SetColumnSpan(ParticulierFlow, ParticulierPanel.ColumnCount); //le FlowPanel s'étend sur le nb de colonnes de ParticulierPanel
            ParticulierPanel.Controls.Add(Choix_Part_Client_Cuisinier,  0, 11);
            ParticulierPanel.SetColumnSpan(Choix_Part_Client_Cuisinier, ParticulierPanel.ColumnCount);
            ParticulierPanel.Controls.Add(Part_Client, 0, 12);
            ParticulierPanel.Controls.Add(Part_Cuisinier, 0, 13);

            // Add the TableLayoutPanel to the "Particulier" GroupBox
            ParticulierGroup.Controls.Add(ParticulierPanel);

            //Ajout du FlowPanel au TablPanel en configurant son étendu
            EntreprisePanel.Controls.Add(EntrepriseFlow, 0, 10);
            EntreprisePanel.SetColumnSpan(EntrepriseFlow, EntreprisePanel.ColumnCount); //le FlowPanel s'étend sur le nb de colonnes de ParticulierPanel

            // Add the TableLayoutPanel to the "Entreprise" GroupBox
            EntrepriseGroup.Controls.Add(EntreprisePanel);

            //Add the GroupBoxes and Retour to the MainLayout 
            MainLayout.Controls.Add(Retour, 0, 0);
            MainLayout.SetColumnSpan(Retour, 2);
            MainLayout.Controls.Add(ParticulierGroup, 1, 0);
            MainLayout.Controls.Add(EntrepriseGroup, 2, 0);

            // Add the Panel to the form
            this.Controls.Add(MainLayout);
            #endregion
        }

        #region Méthode Bouton
        private void Retour_Click(object? sender, EventArgs e)
        {
            PrésentationForm Form = new PrésentationForm();
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }

        private void Hypertxt_Part_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Vous ne croyez tout de même pas qu'on a réellement des conditions d'utilisations à vous faire lire ?! \uD83E\uDD23 \uD83E\uDD23 \uD83E\uDD23");
        }

        private void Hypertxt_Ent_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Vous croyez tout de même pas qu'on a réellement des conditions d'utilisations à vous faire lire ?! \uD83E\uDD23 \uD83E\uDD23 \uD83E\uDD23");
        }

        private void TogglePanelVisibility(TableLayoutPanel panel)
        {
            // Toggle the visibility of the panel
            panel.Visible = !panel.Visible;
        }

        private void Insc_Part_Click(object? sender, EventArgs e)
        {
            //Check CheckBox Condition
            if(!ConditionsPart.Checked)
            {
                MessageBox.Show("Vous devez accpeter les termes et conditions d'utilisations pour continuer.");
                return;
            }

            //Check si on a bien coché case Client et/ou Cuisiniers
            if(Part_Client.Checked == false && Part_Cuisinier.Checked == false)
            {
                MessageBox.Show("Vous devez sélectionner au moins un profil : En tant que Client et commander des plats et/ou en tant que Cuisinier et vendre des plats.");
                return;
            }

            //Check des types des inputs saisis pour requete SQL + association à variable
            if(Nom_Part_Box.Text.Length > 50 || Nom_Part_Box.Text == "")
            {
                MessageBox.Show("Le Nom ne doit pas dépasser 50 caractères ni être vide.");
                return;
            }
            string Nom_PartString = Nom_Part_Box.Text;

            if(Prenom_Part_Box.Text.Length > 50 || Prenom_Part_Box.Text == "")
            {
                MessageBox.Show("Le Prénom ne doit pas dépasser 50 caractères ni être vide.");
                return;
            }
            string Prenom_PartString = Prenom_Part_Box.Text;

            if(Tel_Part_Box.Text.Length != 14)
            {
                MessageBox.Show("Le numéro de Téléphone doit être saisi en format XX XX XX XX XX ni être vide.");
                return;
            }
            string Tel_PartString = Tel_Part_Box.Text;

            if(Email_Part_Box.Text.Length > 100 || Email_Part_Box.Text == "" || Email_Part_Box.Text.Contains('@')  == false)
            {
                MessageBox.Show("L'email ne doit pas dépasser 50 caractère ni être vide et doit contenir un '@' et un domaine");
                return;
            }
            string Email_PartString = Email_Part_Box.Text;

            if(!int.TryParse(NumRue_Part_Box.Text, out _) || NumRue_Part_Box.Text == "")
            {
                MessageBox.Show("Le numéro de la rue doit être un entier valide, sans prendre en compte BIS, TER, etc, ni être vide.");
                return;
            }
            int NumRue_PartInt = Convert.ToInt32(NumRue_Part_Box.Text);

            if(Rue_Part_Box.Text.Length > 50 || Rue_Part_Box.Text == "")
            {
                MessageBox.Show("Le nom de la rue ne doit pas dépasser 50 caractères ni être vide.");
                return;
            }
            string Rue_PartString = Rue_Part_Box.Text;

            if(!int.TryParse(CP_Part_Box.Text, out _) || CP_Part_Box.Text == "")
            {
                MessageBox.Show("Le code postal doit être un entier valide de format XXXXX, ni être vide");
                return;
            }
            int CP_PartInt = Convert.ToInt32(CP_Part_Box.Text);

            if(Ville_Part_Box.Text.Length > 50 || Ville_Part_Box.Text == "")
            {
                MessageBox.Show("Le nom de la ville ne doit pas dépasser 50 caractères, ni être vide.");
                return;
            }
            string Ville_PartString = Ville_Part_Box.Text;

            if(MDP_Part_Box.Text.Length > 20 || MDP_Part_Box.Text == "")
            {
                MessageBox.Show("Le mot de passe ne doit pas dépasser 20 caractères, ni être vide.");
                return;
            }
            string MDP_PartString = MDP_Part_Box.Text;

            // Il ne peut pas y avoir d'erreur sur le metro
            string Metro = MetroPart.Text;

            int[]? Retour_Inscription_Particulier = InscValide_Part(Nom_PartString, Prenom_PartString, Tel_PartString, Email_PartString, NumRue_PartInt, Rue_PartString, CP_PartInt, Ville_PartString, MDP_PartString, Metro, Image_path_Part);
            //SQL
            if(Retour_Inscription_Particulier != null && Part_Client.Checked && !Part_Cuisinier.Checked)
            {
                MessageBox.Show($"Inscription réussi.\nBienvenue sur Liv In Paris.\nVoici votre Identifiant Client : {Retour_Inscription_Particulier[0]}"); //L'indice 0 du tableau est celui du Client
            }
            else if(Retour_Inscription_Particulier != null && !Part_Client.Checked && Part_Cuisinier.Checked)
            {
                MessageBox.Show($"Inscription réussi.\nBienvenue sur Liv In Paris.\nVoici votre Identifiant Cuisinier : {Retour_Inscription_Particulier[1]}"); //L'indice 1 du tableau est celui du Client
            }
            else if(Retour_Inscription_Particulier != null && Part_Client.Checked && Part_Cuisinier.Checked)
            {
                MessageBox.Show($"Inscription réussi.\nBienvenue sur Liv In Paris.\nVoici votre Identifiant Client : {Retour_Inscription_Particulier[0]}\nVoici votre Identifiant Cuisinier : {Retour_Inscription_Particulier[1]}"); //L'indice 1 du tableau est celui du Client
            }
            else
            {
                MessageBox.Show("Inscription échoué, revoir vos informations.");  //Checker l'ID de sorte à ce qu'il n'existe pas déjà dans BDD (ask copilot)
            }
        }
    
        private void Insc_Ent_Click(object? sender, EventArgs e)
        {
            //Check CheckBox Condition
            if(!ConditionsEnt.Checked)
            {
                MessageBox.Show("Vous devez accpeter les termes et conditions d'utilisations pour continuer.");
            }

            //Check des types des inputs saisis pour requete SQL + association à variables
            if(!long.TryParse(SIRET_Ent_Box.Text, out _) || SIRET_Ent_Box.Text == "" || SIRET_Ent_Box.Text.Length != 14)
            {
                MessageBox.Show("Le numéro de SIRET doit être un entier valide de 14 chiffres, ni être vide.");
                return;
            }
            long SIRET_EntLong = Convert.ToInt64(SIRET_Ent_Box.Text);

            if(Nom_Ent_Box.Text.Length > 50 || Nom_Ent_Box.Text == "")
            {
                MessageBox.Show("Le nom de l'entreprise ne doit pas dépasser 50 caractères, ni être vide.");
                return;
            }
            string Nom_EntString = Nom_Ent_Box.Text;

            if(NomRef_Ent_Box.Text.Length > 50 || NomRef_Ent_Box.Text == "")
            {
                MessageBox.Show("Le nom du référent ne doit pas dépasser 50 caractères, ni être vide.");
                return;
            }
            string NomRef_EntString = NomRef_Ent_Box.Text;

            if(TelRef_Ent_Box.Text.Length != 14 || TelRef_Ent_Box.Text == "")
            {
                MessageBox.Show("Le numéro de téléphone du référent doit être saisi au format XX XX XX XX XX, ni être vide.");
                return;
            }
            string TelRef_EntString = TelRef_Ent_Box.Text;

            if(!int.TryParse(NumRue_Ent_Box.Text, out _) || NumRue_Ent_Box.Text == "")
            {
                MessageBox.Show("Le numéro de la rue doit être un entier valide, sans prendre en compte BIS, TER, etc, ni être vide.");
                return;
            }
            int NumRue_EntInt = Convert.ToInt32(NumRue_Ent_Box.Text);

            if(Rue_Ent_Box.Text.Length > 50 || Rue_Ent_Box.Text == "")
            {
                MessageBox.Show("La rue ne doit pas dépasser 50 caractères, ni être vide.");
                return;
            }
            string Rue_EntString = Rue_Ent_Box.Text;

            if(!int.TryParse(CP_Ent_Box.Text, out _) || CP_Ent_Box.Text == "")
            {
                MessageBox.Show("Le code postal doit être saisi au format XXXXX, ni être vide.");
                return;
            }
            int CP_EntInt = Convert.ToInt32(CP_Ent_Box.Text);

            if(Ville_Ent_Box.Text.Length > 50 || Ville_Ent_Box.Text == "")
            {
                MessageBox.Show("Le nom de la ville ne doit pas dépasser 50 caractères, ni être vide.");
                return;
            }
            string Ville_EntString = Ville_Ent_Box.Text;

            if(MDP_Ent_Box.Text.Length > 20 || MDP_Ent_Box.Text == "")
            {
                MessageBox.Show("Le mot de passe ne doit pas dépasser 20 caractères, ni être vide.");
                return;
            }
            string MDP_EntString = MDP_Ent_Box.Text;

            // Il ne peut pas y avoir d'erreur sur la saisie du métro
            string Metro = MetroEnt.Text;

            int[]? Retour_Inscription_Ent = InscValide_Ent(SIRET_EntLong, Nom_EntString, NomRef_EntString, TelRef_EntString, NumRue_EntInt, Rue_EntString, CP_EntInt, Ville_EntString, MDP_EntString, Metro, Image_path_Ent);

            //SQL
            if(Retour_Inscription_Ent != null)
            {
                MessageBox.Show($"Inscription réussi.\nBienvenue sur Liv In Paris.\nVoici votre Identifiant Client : {Retour_Inscription_Ent[0]}");
            }
            else
            {
                MessageBox.Show("Erreur sur l'inscription, revoir vos informations.");
            }
        }

        #endregion

        #region Méthode Image
        private void Choisir_Image_Click_Part(object? sender, EventArgs e)
        {
            try
            {
                using(OpenFileDialog fileDialog = new OpenFileDialog())
                {
                    fileDialog.Filter = "Images (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                    fileDialog.Title = "Sélectionnez une image";

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        Image_path_Part = fileDialog.FileName;                        
                        MessageBox.Show("Image sélectionnée : "+ Image_path_Part);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du sélecteur de fichiers : {ex.Message}");
            }
        }

        private void Choisir_Image_Click_Ent(object? sender, EventArgs e)
        {
            using(OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Images (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                fileDialog.Title = "Sélectionnez une image";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image_path_Ent = fileDialog.FileName;
                    MessageBox.Show("Image sélectionnée : "+ Image_path_Ent);
                }
            }
        }

        #endregion

        #region Méthode Validation        
        private int[]? InscValide_Part(string nom, string prenom, string tel, string email, int Nrue, string rue, int CP, string ville, string mdp, string metro, string? image)     //Type int[]? pour qu'on puisse afficher les ID à l'utilisateur lors d'un futur login
        {
            //On génère d'abord un ID qui n'existe pas déjà dans Client et/ou Cuisiniers selon le choix avec GenIDSQL pour ne pas encombrer le réel but de la méthode Inscription
            int ID_Client = GenIDSQL(0); //ID de 4 chiffres de long uniques et conformes à ce que SQL attend
            int ID_Cuisinier = GenIDSQL(1);
            int[]? list_ID = {ID_Client, ID_Cuisinier};

            // Maintenant on insert le tout dans la BDD
            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root"))
                {
                    connection.Open();

                    string InsertPart = @"INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville, Metro) VALUES (@nom, @prenom, @tel, @email,  @Nrue, @rue, @CP, @Ville, @Metro);";
                    using(MySqlCommand cmdPart = new MySqlCommand(InsertPart, connection))
                    {
                        cmdPart.Parameters.AddWithValue("@nom", nom);
                        cmdPart.Parameters.AddWithValue("@prenom", prenom);
                        cmdPart.Parameters.AddWithValue("@tel", tel);
                        cmdPart.Parameters.AddWithValue("@email", email);
                        cmdPart.Parameters.AddWithValue("@Nrue", Nrue);
                        cmdPart.Parameters.AddWithValue("@rue", rue);
                        cmdPart.Parameters.AddWithValue("@CP", CP);
                        cmdPart.Parameters.AddWithValue("@Ville", ville);
                        cmdPart.Parameters.AddWithValue("@Metro", metro);

                        cmdPart.ExecuteNonQuery();
                    }

                    // Dinstinguons le cas Cuisiner et/ou Client
                    if(Part_Client.Checked && !Part_Cuisinier.Checked) //On veut être uniquement Client
                    {
                        string InsertClient = @"INSERT INTO Client (Id_client, Mdp, Nom_client, Prenom_client, SIRET_entreprise, Photo_profil) VALUES (@id, @mdp, @nomClient, @prenomClient, @SIRET, @photo);";
                        using(MySqlCommand cmdPart = new MySqlCommand(InsertClient, connection))
                        {
                            cmdPart.Parameters.AddWithValue("@id", ID_Client);
                            cmdPart.Parameters.AddWithValue("@mdp", mdp);
                            cmdPart.Parameters.AddWithValue("@nomClient", nom);
                            cmdPart.Parameters.AddWithValue("@prenomClient", prenom);
                            cmdPart.Parameters.AddWithValue("@SIRET", null);            //Arbitraire car dans les contraintes d'ajout dans la BDD, si Client est un particulier, alors le paramètre SIRET est nul
                            // Paramètre photo

                            if(!string.IsNullOrEmpty(image))
                            {
                                byte[] imageBytes = ConvertImageToBytes(image);
                                cmdPart.Parameters.AddWithValue("@photo", imageBytes);
                            }
                            else
                            {
                                cmdPart.Parameters.AddWithValue("@photo", null);
                            }
                            cmdPart.ExecuteNonQuery();
                        }
                    }
                    else if(Part_Cuisinier.Checked && !Part_Client.Checked) //On veut être uniquement Cuisinier
                    {
                        string InsertCuisinier = @"INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier, Photo_profil) VALUES (@id, @mdp, @nom, @prenom, @photo);";
                        using(MySqlCommand cmdPart = new MySqlCommand(InsertCuisinier, connection))
                        {
                            cmdPart.Parameters.AddWithValue("@id", ID_Cuisinier);
                            cmdPart.Parameters.AddWithValue("@mdp", mdp);
                            cmdPart.Parameters.AddWithValue("@nom", nom);
                            cmdPart.Parameters.AddWithValue("@prenom", prenom);
                            // Paramètre photo

                            if(!string.IsNullOrEmpty(image))
                            {
                                byte[] imageBytes = ConvertImageToBytes(image);
                                cmdPart.Parameters.AddWithValue("@photo", imageBytes);
                            }
                            else
                            {
                                cmdPart.Parameters.AddWithValue("@photo", null);
                            }
                            cmdPart.ExecuteNonQuery();
                        }
                    }
                    else    //Cas Cuisinier ET Client car les deux = faux est impossible car vérifié dans Insc_Part_Click
                    {
                        string InsertCuisinier = @"INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier, Photo_profil) VALUES (@id, @mdp, @nom, @prenom, @photo);";
                        using(MySqlCommand cmdPart = new MySqlCommand(InsertCuisinier, connection))
                        {
                            cmdPart.Parameters.AddWithValue("@id", ID_Cuisinier);
                            cmdPart.Parameters.AddWithValue("@mdp", mdp);
                            cmdPart.Parameters.AddWithValue("@nom", nom);
                            cmdPart.Parameters.AddWithValue("@prenom", prenom);
                            // Paramètre photo

                            if(!string.IsNullOrEmpty(image))
                            {
                                byte[] imageBytes = ConvertImageToBytes(image);
                                cmdPart.Parameters.AddWithValue("@photo", imageBytes);
                            }
                            else
                            {
                                cmdPart.Parameters.AddWithValue("@photo", null);
                            }
                            cmdPart.ExecuteNonQuery();
                        }

                        string InsertClient = @"INSERT INTO Client (Id_client, Mdp, Nom_client, Prenom_client, SIRET_entreprise, Photo_profil) VALUES (@id, @mdp, @nomClient, @prenomClient, @SIRET, @photo);";
                        using(MySqlCommand cmdPart = new MySqlCommand(InsertClient, connection))
                        {
                            cmdPart.Parameters.AddWithValue("@id", ID_Client);
                            cmdPart.Parameters.AddWithValue("@mdp", mdp);
                            cmdPart.Parameters.AddWithValue("@nomClient", nom);
                            cmdPart.Parameters.AddWithValue("@prenomClient", prenom);
                            cmdPart.Parameters.AddWithValue("@SIRET", null);
                            // Paramètre photo

                            if(!string.IsNullOrEmpty(image))
                            {
                                byte[] imageBytes = ConvertImageToBytes(image);
                                cmdPart.Parameters.AddWithValue("@photo", imageBytes);
                            }
                            else
                            {
                                cmdPart.Parameters.AddWithValue("@photo", null);
                            }
                            cmdPart.ExecuteNonQuery();
                        }
                    }
                    return list_ID;    //Inscription réussi
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'inscription : {ex.Message}");
                return null;
            }
        }


        private int[]? InscValide_Ent(long siret, string nom_Ent, string nomRef, string telRef, int Nrue, string rue, int CP, string ville, string mdp, string metro, string? image)
        {
            int ID_Ent = GenIDSQL(0);   //obligé 0 car ne peut être qu'un Client
            int[]? list_ID = {ID_Ent};
            // Maintenant on insert le tout dans la BDD
            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root"))
                {
                    connection.Open();

                    string InsertEnt = @"INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville, Metro) VALUES (@SIRET, @NomE, @NomRef, @telRef, @NRue, @Rue, @CP, @ville, @metro);";
                    using(MySqlCommand cmdEnt = new MySqlCommand(InsertEnt, connection))
                    {
                        cmdEnt.Parameters.AddWithValue("@SIRET", siret);
                        cmdEnt.Parameters.AddWithValue("@NomE", nom_Ent);
                        cmdEnt.Parameters.AddWithValue("@NomRef", nomRef);
                        cmdEnt.Parameters.AddWithValue("@telRef", telRef);
                        cmdEnt.Parameters.AddWithValue("@NRue", Nrue);
                        cmdEnt.Parameters.AddWithValue("@Rue", rue);
                        cmdEnt.Parameters.AddWithValue("@CP", CP);
                        cmdEnt.Parameters.AddWithValue("@ville", ville);
                        cmdEnt.Parameters.AddWithValue("@metro", metro);

                        cmdEnt.ExecuteNonQuery();
                    }

                    //Et maintenant dans Client
                    string InsertClient = @"INSERT INTO Client (Id_client, Mdp, Nom_client, Prenom_client, SIRET_entreprise, Photo_profil) VALUES (@id, @mdp, @nomClient, @prenomClient, @SIRET, @photo);";
                    using(MySqlCommand cmdEnt = new MySqlCommand(InsertClient, connection))
                    {
                        cmdEnt.Parameters.AddWithValue("@id", ID_Ent);
                        cmdEnt.Parameters.AddWithValue("@mdp", mdp);
                        cmdEnt.Parameters.AddWithValue("@nomClient", null);
                        cmdEnt.Parameters.AddWithValue("@prenomClient", null);     //Même argument que pour les Particulier devenant Client
                        cmdEnt.Parameters.AddWithValue("@SIRET", siret);
                        // Paramètre photo
                    
                        if(!string.IsNullOrEmpty(image))
                        {
                            byte[] imageBytes = ConvertImageToBytes(image);
                            cmdEnt.Parameters.AddWithValue("@photo", imageBytes);
                        }
                        else
                        {
                            cmdEnt.Parameters.AddWithValue("@photo", null);
                        }

                        cmdEnt.ExecuteNonQuery();
                    }
                    return list_ID;    //Inscription réussi
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'inscription : {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Méthode Tiers

        private TextBox AjoutLabelEtTextBoxParticulier(string label, int row)
        {
            Label Label = new Label() {Text = label, AutoSize = true, Anchor = AnchorStyles.Left};
            TextBox textBox = new TextBox() {Width = ParticulierPanel.Width - 35, AutoSize = true, Anchor = AnchorStyles.Right};
            ParticulierPanel.Controls.Add(Label, 0, row);
            ParticulierPanel.Controls.Add(textBox, 1, row);
            return textBox;
        }

        private TextBox AjoutLabelEtTextBoxEntreprise(string label, int row)
        {
            Label Label = new Label {Text = label, AutoSize = true, Anchor = AnchorStyles.Left};
            TextBox textBox = new TextBox {Width = EntreprisePanel.Width - 45, AutoSize = true, Anchor = AnchorStyles.Right};
            EntreprisePanel.Controls.Add(Label, 0, row);
            EntreprisePanel.Controls.Add(textBox, 1, row);
            return textBox;
        }

        private ComboBox AjoutLabelEtComboBoxParticulier(string label, List<string> liste, int row)
        {
            Label Label = new Label {Text = label, AutoSize = true, Anchor = AnchorStyles.Left};
            ComboBox ComboBox = new ComboBox();
            ComboBox.Items.AddRange(liste.ToArray());
            ComboBox.Width = ParticulierPanel.Width - 35;
            ComboBox.AutoSize = true;
            ComboBox.Anchor = AnchorStyles.Left;
            ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(liste.ToArray());
            ComboBox.AutoCompleteCustomSource = autoComplete;
            
            ParticulierPanel.Controls.Add(Label, 0, row);
            ParticulierPanel.Controls.Add(ComboBox, 1, row);
            return ComboBox;
        }

        private ComboBox AjoutLabelEtComboBoxEntreprise(string label, List<string> liste, int row)
        {
            Label Label = new Label {Text = label, AutoSize = true, Anchor = AnchorStyles.Left};
            ComboBox ComboBox = new ComboBox();
            ComboBox.Items.AddRange(liste.ToArray());
            ComboBox.Width = EntreprisePanel.Width - 45;
            ComboBox.AutoSize = true;
            ComboBox.Anchor = AnchorStyles.Left;
            ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            
            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(liste.ToArray());
            ComboBox.AutoCompleteCustomSource = autoComplete;

            ParticulierPanel.Controls.Add(Label, 0, row);
            ParticulierPanel.Controls.Add(ComboBox, 1, row);
            EntreprisePanel.Controls.Add(Label, 0, row);
            EntreprisePanel.Controls.Add(ComboBox, 1, row);
            return ComboBox;
        }
        private int GenIDSQL(int choix)     //0 pour Client (Part/Ent) et 1 pour Cuisinier
        {
            int ID = 0;
            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root"))
                {
                    connection.Open();

                    Random random = new Random();
                    do
                    {
                        ID = random.Next(1, 10000);
                        string query = "";
                        switch(choix)
                        {
                            case 0 :
                                query = @"SELECT COUNT(*) FROM Client WHERE Id_client = @id";
                            break;

                            case 1 :
                                query = @"SELECT COUNT(*) FROM Cuisinier WHERE Id_cuisinier = @id";
                            break;

                            default : break ; //deux choix possible pour choix donc pas besoin
                        }
                        using(MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@id", ID);
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            if (count == 0) //Veut dire que l'ID est bien unique
                            {
                                return ID;
                            }
                        }
                    } while (true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur durant de l'inscription : {ex.Message}");
                return -1;   
                //Arbitraire
            }
        }


        private List<string> ChargerListeMetro(string cheminFichier)
        {
            // Enregistrer le fournisseur d'encodage pour éviter l'erreur IBM437
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
            HashSet<string> stationSet= new HashSet<string>();   //Afin de ne pas avoir de doublons, on le convertira en List à la fin

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

        private byte[] ConvertImageToBytes(string imagePath)
        {
            return File.ReadAllBytes(imagePath);
        }

        #endregion
    }
}
