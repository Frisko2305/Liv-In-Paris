using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Insc_Form : Form
    {
        private GroupBox ParticulierGroup;
        private GroupBox EntrepriseGroup;
        private TableLayoutPanel ParticulierPanel;
        private TableLayoutPanel EntreprisePanel;
        private FlowLayoutPanel ParticulierFlow;
        private FlowLayoutPanel EntrepriseFlow;
        private CheckBox ConditionsPart;
        private CheckBox ConditionsEnt;
        private Button Insc_Part;
        private Button Insc_Ent;
        private TableLayoutPanel MainLayout;    //Permet de placer à gauche et droite de la page les Layout Part & Ent
        private Button Retour;
        private Label Hypertxt_Part;
        private Label Hypertxt_Ent;     //Créer des 'liens' pour permettre de lire les conditions d'utilisations
        private Label Choix_Part_Client_Cuisinier;
        private CheckBox Part_Client;
        private CheckBox Part_Cuisinier;

        //Obligatoire de créer une variable pour chaque élément pour par la suite pouvoir les récupérer et créer le tuple associé
        private TextBox Nom_Part_Box;
        private TextBox Prenom_Part_Box;
        private TextBox Tel_Part_Box;
        private TextBox Email_Part_Box;
        private TextBox NumRue_Part_Box;
        private TextBox Rue_Part_Box;
        private TextBox CP_Part_Box;
        private TextBox Ville_Part_Box;
        private TextBox MDP_Part_Box;
        private TextBox SIRET_Ent_Box;
        private TextBox Nom_Ent_Box;
        private TextBox NomRef_Ent_Box;
        private TextBox TelRef_Ent_Box;
        private TextBox NumRue_Ent_Box;
        private TextBox Rue_Ent_Box;
        private TextBox CP_Ent_Box;
        private TextBox Ville_Ent_Box;
        private TextBox MDP_Ent_Box;

        private Label Nom_Part_Label;
        private Label Prenom_Part_Label;
        private Label Tel_Part_Label;
        private Label Email_Part_Label;
        private Label NumRue_Part_Label;
        private Label Rue_Part_Label;
        private Label CP_Part_Label;
        private Label Ville_Part_Label;
        private Label MDP_Part_Label;
        private Label SIRET_Ent_Label;
        private Label Nom_Ent_Label;
        private Label NomRef_Ent_Label;
        private Label TelRef_Ent_Label;
        private Label NumRue_Ent_Label;
        private Label Rue_Ent_Label;
        private Label CP_Ent_Label;
        private Label Ville_Ent_Label;
        private Label MDP_Ent_Label;

        public Insc_Form()
        {
            this.Text = "Inscription to Liv In Paris !";
            this.Width = 750;
            this.Height = 650;
            this.StartPosition = FormStartPosition.CenterScreen;    //Centre la fenêtre centre écran

            Retour = new Button();
            Retour.Text = "Retour";
            Retour.AutoSize = true;
            Retour.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Retour.Click += Retour_Click;

            Hypertxt_Part = new Label();
            Hypertxt_Part.Text = "Lire les conditions d'utilisations.";
            Hypertxt_Part.ForeColor = Color.Cyan;
            Hypertxt_Part.AutoSize = true;
            Hypertxt_Part.Cursor = Cursors.Hand;
            Hypertxt_Part.Click += Hypertxt_Part_Click;

            Hypertxt_Ent = new Label();
            Hypertxt_Ent.Text = "Lire les conditions d'utilisations.";
            Hypertxt_Ent.ForeColor = Color.Cyan;
            Hypertxt_Ent.AutoSize = true;
            Hypertxt_Ent.Cursor = Cursors.Hand;
            Hypertxt_Ent.Click += Hypertxt_Ent_Click;

            //Creation of every TextBox/Labels
            //Label Particulier

            Nom_Part_Label = new Label() {Text = "Nom :", AutoSize = true, Anchor = AnchorStyles.Left};
            Prenom_Part_Label = new Label() {Text = "Prenom :", AutoSize = true, Anchor = AnchorStyles.Left};
            Tel_Part_Label = new Label() {Text = "Téléphone :", AutoSize = true, Anchor = AnchorStyles.Left};
            Email_Part_Label = new Label() {Text = "Email :", AutoSize = true, Anchor = AnchorStyles.Left};
            NumRue_Part_Label = new Label() {Text = "Numéro de rue :", AutoSize = true, Anchor = AnchorStyles.Left};
            Rue_Part_Label = new Label() {Text = "Rue :", AutoSize = true, Anchor = AnchorStyles.Left};
            CP_Part_Label = new Label() {Text = "Code Postal :", AutoSize = true, Anchor = AnchorStyles.Left};
            Ville_Part_Label = new Label() {Text = "Ville :", AutoSize = true, Anchor = AnchorStyles.Left};
            MDP_Part_Label = new Label() {Text = "Mot de passe :", AutoSize = true, Anchor = AnchorStyles.Left};

            //TextBox Particulier

            Nom_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};
            Prenom_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};
            Tel_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};
            Email_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};
            NumRue_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};
            Rue_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};
            CP_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};
            Ville_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};
            MDP_Part_Box = new TextBox() {Width = 200, AutoSize = true, Anchor = AnchorStyles.Right};

            //Label Entreprise

            SIRET_Ent_Label = new Label() {Text = "N° SIRET :", AutoSize = true, Anchor = AnchorStyles.Left};
            Nom_Ent_Label = new Label() {Text = "Nom de l'entreprise :", AutoSize = true, Anchor = AnchorStyles.Left};
            NomRef_Ent_Label = new Label() {Text = "Nom du référent :", AutoSize = true, Anchor = AnchorStyles.Left};
            TelRef_Ent_Label = new Label() {Text = "Numéro de téléphone du référent :", AutoSize = true, Anchor = AnchorStyles.Left};
            NumRue_Ent_Label = new Label() {Text = "Numéro de rue :", AutoSize = true, Anchor = AnchorStyles.Left};
            Rue_Ent_Label = new Label() {Text = "Rue :", AutoSize = true, Anchor = AnchorStyles.Left};
            CP_Ent_Label = new Label() {Text = "Code Postal :", AutoSize = true, Anchor = AnchorStyles.Left};
            Ville_Ent_Label = new Label() {Text = "Ville :", AutoSize = true, Anchor = AnchorStyles.Left};
            MDP_Ent_Label = new Label() {Text = "Mot de passe :", AutoSize = true, Anchor = AnchorStyles.Left};

            //TextBox Entreprise

            SIRET_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};
            Nom_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};
            NomRef_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};
            TelRef_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};
            NumRue_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};
            Rue_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};
            CP_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};
            Ville_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};
            MDP_Ent_Box = new TextBox() {Width = 150, Anchor = AnchorStyles.Right};

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
            ParticulierPanel.RowCount = 12;
            ParticulierPanel.ColumnCount = 2;
            ParticulierPanel.AutoSize = true;
            ParticulierPanel.Visible = false; // Initially hidden

            // Add fields to "Particulier" 
            ParticulierPanel.Controls.Add(Nom_Part_Label, 0, 0);
            ParticulierPanel.Controls.Add(Nom_Part_Box, 1, 0);
            ParticulierPanel.Controls.Add(Prenom_Part_Label, 0, 1);
            ParticulierPanel.Controls.Add(Prenom_Part_Box, 1, 1);
            ParticulierPanel.Controls.Add(Tel_Part_Label, 0, 2);
            ParticulierPanel.Controls.Add(Tel_Part_Box, 1, 2);
            ParticulierPanel.Controls.Add(Email_Part_Label, 0, 3);
            ParticulierPanel.Controls.Add(Email_Part_Box, 1, 3);
            ParticulierPanel.Controls.Add(NumRue_Part_Label, 0, 4);
            ParticulierPanel.Controls.Add(NumRue_Part_Box, 1, 4);
            ParticulierPanel.Controls.Add(Rue_Part_Label, 0, 5);
            ParticulierPanel.Controls.Add(Rue_Part_Box, 1, 5);
            ParticulierPanel.Controls.Add(CP_Part_Label, 0, 6);
            ParticulierPanel.Controls.Add(CP_Part_Box, 1, 6);
            ParticulierPanel.Controls.Add(Ville_Part_Label, 0, 7);
            ParticulierPanel.Controls.Add(Ville_Part_Box, 1, 7);
            ParticulierPanel.Controls.Add(MDP_Part_Label, 0, 8);
            ParticulierPanel.Controls.Add(MDP_Part_Box, 1, 8);

            // The last row with all checkboxes is added separately
            ParticulierFlow = new FlowLayoutPanel();
            ParticulierFlow.Dock = DockStyle.Fill;
            ParticulierFlow.FlowDirection = FlowDirection.LeftToRight;
            ParticulierFlow.AutoSize = true;

            ConditionsPart = new CheckBox()
            {
                Text = "J'accepte les termes et conditions d'utilisation.",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            };

            Insc_Part = new Button();
            Insc_Part.Text = "Valider ?";
            Insc_Part.AutoSize = true;
            Insc_Part.Width = 100;
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
            //ajout du checkbox au flowPanel
            ParticulierFlow.Controls.Add(ConditionsPart);
            ParticulierFlow.Controls.Add(Hypertxt_Part);
            ParticulierFlow.Controls.Add(Insc_Part);

            //Ajout du FlowPanel au TablPanel en configurant son étendu
            ParticulierPanel.Controls.Add(ParticulierFlow, 0, 9);
            ParticulierPanel.SetColumnSpan(ParticulierFlow, ParticulierPanel.ColumnCount); //le FlowPanel s'étend sur le nb de colonnes de ParticulierPanel
            ParticulierPanel.Controls.Add(Choix_Part_Client_Cuisinier,  0, 10);
            ParticulierPanel.SetColumnSpan(Choix_Part_Client_Cuisinier, ParticulierPanel.ColumnCount);
            ParticulierPanel.Controls.Add(Part_Client, 0, 11);
            ParticulierPanel.Controls.Add(Part_Cuisinier, 0, 12);

            // Add the TableLayoutPanel to the "Particulier" GroupBox
            ParticulierGroup.Controls.Add(ParticulierPanel);


            // Create the TableLayoutPanel for "Entreprise"
            EntreprisePanel = new TableLayoutPanel();
            EntreprisePanel.Dock = DockStyle.Fill;
            EntreprisePanel.RowCount = 11;
            EntreprisePanel.ColumnCount = 2;
            EntreprisePanel.AutoSize = true;
            EntreprisePanel.Visible = false; // Initially hidden

            // Add fields to "Entreprise"
            EntreprisePanel.Controls.Add(SIRET_Ent_Label, 0, 0);
            EntreprisePanel.Controls.Add(SIRET_Ent_Box, 1, 0);
            EntreprisePanel.Controls.Add(Nom_Ent_Label, 0, 1);
            EntreprisePanel.Controls.Add(Nom_Ent_Box, 1, 1);
            EntreprisePanel.Controls.Add(NomRef_Ent_Label, 0, 2);
            EntreprisePanel.Controls.Add(NomRef_Ent_Box, 1, 2);
            EntreprisePanel.Controls.Add(TelRef_Ent_Label, 0, 3);
            EntreprisePanel.Controls.Add(TelRef_Ent_Box, 1, 3);
            EntreprisePanel.Controls.Add(NumRue_Ent_Label, 0, 4);
            EntreprisePanel.Controls.Add(NumRue_Ent_Box, 1, 4);
            EntreprisePanel.Controls.Add(Rue_Ent_Label, 0, 5);
            EntreprisePanel.Controls.Add(Rue_Ent_Box, 1, 5);
            EntreprisePanel.Controls.Add(CP_Ent_Label, 0, 6);
            EntreprisePanel.Controls.Add(CP_Ent_Box, 1, 6);
            EntreprisePanel.Controls.Add(Ville_Ent_Label, 0, 7);
            EntreprisePanel.Controls.Add(Ville_Ent_Box, 1, 7);
            EntreprisePanel.Controls.Add(MDP_Ent_Label, 0, 8);
            EntreprisePanel.Controls.Add(MDP_Ent_Box, 1, 8);

            // The last row with the checkbox is added separately
            EntrepriseFlow = new FlowLayoutPanel();
            EntrepriseFlow.Dock = DockStyle.Fill;
            EntrepriseFlow.FlowDirection = FlowDirection.LeftToRight;
            EntrepriseFlow.AutoSize = true;

            ConditionsEnt = new CheckBox()
            {
                Text = "J'accepte les termes et conditions d'utilisation.",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            };

            Insc_Ent = new Button();
            Insc_Ent.Text = "Validé ?";
            Insc_Ent.AutoSize = true;
            Insc_Ent.Width = 100;
            Insc_Ent.Click += Insc_Ent_Click;

            //ajout du checkbox au flowPanel
            EntrepriseFlow.Controls.Add(ConditionsEnt);
            EntrepriseFlow.Controls.Add(Hypertxt_Ent);
            EntrepriseFlow.Controls.Add(Insc_Ent);
        
            //Ajout du FlowPanel au TablPanel en configurant son étendu
            EntreprisePanel.Controls.Add(EntrepriseFlow, 0, 9);
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
        }

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
            }

            //Check si on a bien coché case Client et/ou Cuisiniers
            if(Part_Client.Checked == false && Part_Cuisinier.Checked == false)
            {
                MessageBox.Show("Vous devez sélectionner au moins un profil : En tant que Client et commander des plats et/ou en tant que Cuisinier et vendre des plats.");
            }

            //Check des types des inputs saisis pour requete SQL + association à variable
            if(Nom_Part_Box.Text.Length > 50 || Nom_Part_Box.Text == "")
            {
                MessageBox.Show("Le Nom ne doit pas dépasser 50 caractères ni être vide.");
            }
            string Nom_PartString = Nom_Part_Box.Text;

            if(Prenom_Part_Box.Text.Length > 50 || Prenom_Part_Box.Text == "")
            {
                MessageBox.Show("Le Prénom ne doit pas dépasser 50 caractères ni être vide.");
            }
            string Prenom_PartString = Prenom_Part_Box.Text;

            if(Tel_Part_Box.Text.Length != 14)
            {
                MessageBox.Show("Le numéro de Téléphone doit être saisi en format XX XX XX XX XX ni être vide.");
            }
            string Tel_PartString = Tel_Part_Box.Text;

            if(Email_Part_Box.Text.Length > 100 || Email_Part_Box.Text == "" || Email_Part_Box.Text.Contains('@')  == false)
            {
                MessageBox.Show("L'email ne doit pas dépasser 50 caractère ni être vide et doit contenir un '@' et un domaine");
            }
            string Email_PartString = Email_Part_Box.Text;

            if(!int.TryParse(NumRue_Part_Box.Text, out _) || NumRue_Part_Box.Text == "")
            {
                MessageBox.Show("Le numéro de la rue doit être un entier valide, sans prendre en compte BIS, TER, etc, ni être vide.");
            }
            int NumRue_PartInt = Convert.ToInt32(NumRue_Part_Box.Text);

            if(Rue_Part_Box.Text.Length > 50 || Rue_Part_Box.Text == "")
            {
                MessageBox.Show("Le nom de la rue ne doit pas dépasser 50 caractères ni être vide.");
            }
            string Rue_PartString = Rue_Part_Box.Text;

            if(!int.TryParse(CP_Part_Box.Text, out _) || CP_Part_Box.Text == "")
            {
                MessageBox.Show("Le code postal doit être un entier valide de format XXXXX, ni être vide");
            }
            int CP_PartInt = Convert.ToInt32(CP_Part_Box.Text);

            if(Ville_Part_Box.Text.Length > 50 || Ville_Part_Box.Text == "")
            {
                MessageBox.Show("Le nom de la ville ne doit pas dépasser 50 caractères, ni être vide.");
            }
            string Ville_PartString = Ville_Part_Box.Text;

            if(MDP_Part_Box.Text.Length > 20 || MDP_Part_Box.Text == "")
            {
                MessageBox.Show("Le mot de passe ne doit pas dépasser 20 caractères, ni être vide.");
            }
            string MDP_PartString = MDP_Part_Box.Text;

            int[]? Retour_Inscription_Particulier = InscValide_Part(Nom_PartString, Prenom_PartString, Tel_PartString, Email_PartString, NumRue_PartInt, Rue_PartString, CP_PartInt, Ville_PartString, MDP_PartString);
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
            if(!long.TryParse(SIRET_Ent_Box.Text, out _) || SIRET_Ent_Box.Text == "")
            {
                MessageBox.Show("Le numéro de SIRET doit être un entier valide sous format XXXXXXXXX, ni être vide.");
            }
            long SIRET_EntLong = Convert.ToInt64(SIRET_Ent_Box.Text);

            if(Nom_Ent_Box.Text.Length > 50 || Nom_Ent_Box.Text == "")
            {
                MessageBox.Show("Le nom de l'entreprise ne doit pas dépasser 50 caractères, ni être vide.");
            }
            string Nom_EntString = Nom_Ent_Box.Text;

            if(NomRef_Ent_Box.Text.Length > 50 || NomRef_Ent_Box.Text == "")
            {
                MessageBox.Show("Le nom du référent ne doit pas dépasser 50 caractères, ni être vide.");
            }
            string NomRef_EntString = NomRef_Ent_Box.Text;

            if(TelRef_Ent_Box.Text.Length != 14 || TelRef_Ent_Box.Text == "")
            {
                MessageBox.Show("Le numéro de téléphone du référent doit être saisi au format XX XX XX XX XX, ni être vide.");
            }
            string TelRef_EntString = TelRef_Ent_Box.Text;

            if(!int.TryParse(NumRue_Ent_Box.Text, out _) || NumRue_Ent_Box.Text == "")
            {
                MessageBox.Show("Le numéro de la rue doit être un entier valide, sans prendre en compte BIS, TER, etc, ni être vide.");
            }
            int NumRue_EntInt = Convert.ToInt32(NumRue_Ent_Box.Text);

            if(Rue_Ent_Box.Text.Length > 50 || Rue_Ent_Box.Text == "")
            {
                MessageBox.Show("La rue ne doit pas dépasser 50 caractères, ni être vide.");
            }
            string Rue_EntString = Rue_Ent_Box.Text;

            if(!int.TryParse(CP_Ent_Box.Text, out _) || CP_Ent_Box.Text == "")
            {
                MessageBox.Show("Le code postal doit être saisi au format XXXXX, ni être vide.");
            }
            int CP_EntInt = Convert.ToInt32(CP_Ent_Box.Text);

            if(Ville_Ent_Box.Text.Length > 50 || Ville_Ent_Box.Text == "")
            {
                MessageBox.Show("Le nom de la ville ne doit pas dépasser 50 caractères, ni être vide.");
            }
            string Ville_EntString = Ville_Ent_Box.Text;

            if(MDP_Ent_Box.Text.Length > 20 || MDP_Ent_Box.Text == "")
            {
                MessageBox.Show("Le mot de passe ne doit pas dépasser 20 caractères, ni être vide.");
            }
            string MDP_EntString = MDP_Ent_Box.Text;

            int[]? Retour_Inscription_Ent = InscValide_Ent(SIRET_EntLong, Nom_EntString, NomRef_EntString, TelRef_EntString, NumRue_EntInt, Rue_EntString, CP_EntInt, Ville_EntString, MDP_EntString);

            //SQL
            if(Retour_Inscription_Ent != null)
            {
                MessageBox.Show($"Inscription réussi.\nBienvenue sur Liv In Paris.\nVoici votre Identifiant Client : {Retour_Inscription_Ent[0]}");
            }
            else
            {
                MessageBox.Show("Erreur sur l'inscription, revoir vos informations.");   //Checker l'ID de sorte à ce qu'il n'existe pas déjà dans BDD (ask copilot)
            }
        }

        private int[]? InscValide_Part(string nom, string prenom, string tel, string email, int Nrue, string rue, int CP, string ville, string mdp)     //Type int[]? pour qu'on puisse afficher les ID à l'utilisateur lors d'un futur login
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

                    string InsertPart = @"INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES (@nom, @prenom, @tel, @email,  @Nrue, @rue, @CP, @Ville);";
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

                        cmdPart.ExecuteNonQuery();
                    }

                    // Dinstinguons le cas Cuisiner et/ou Client
                    if(Part_Client.Checked && !Part_Cuisinier.Checked) //On veut être uniquement Client
                    {
                        string InsertClient = @"INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (@id, @mdp, @nomClient, @prenomClient, @SIRET);";
                        using(MySqlCommand cmdPart = new MySqlCommand(InsertClient, connection))
                        {
                            cmdPart.Parameters.AddWithValue("@id", ID_Client);
                            cmdPart.Parameters.AddWithValue("@mdp", mdp);
                            cmdPart.Parameters.AddWithValue("@nomClient", nom);
                            cmdPart.Parameters.AddWithValue("@prenomClient", prenom);
                            cmdPart.Parameters.AddWithValue("@SIRET", null);            //Arbitraire car dans les contraintes d'ajout dans la BDD, si Client est un particulier, alors le paramètre SIRET est nul

                            cmdPart.ExecuteNonQuery();
                        }
                    }
                    else if(Part_Cuisinier.Checked && !Part_Client.Checked) //On veut être uniquement Cuisinier
                    {
                        string InsertCuisinier = @"INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (@id, @mdp, @nom, @prenom);";
                        using(MySqlCommand cmdPart = new MySqlCommand(InsertCuisinier, connection))
                        {
                            cmdPart.Parameters.AddWithValue("@id", ID_Cuisinier);
                            cmdPart.Parameters.AddWithValue("@mdp", mdp);
                            cmdPart.Parameters.AddWithValue("@nom", nom);
                            cmdPart.Parameters.AddWithValue("@prenom", prenom);

                            cmdPart.ExecuteNonQuery();
                        }
                    }
                    else    //Cas Cuisinier ET Client car les deux = faux est impossible car vérifié dans Insc_Part_Click
                    {
                        string InsertCuisinier = @"INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (@id, @mdp, @nom, @prenom);";
                        using(MySqlCommand cmdPart = new MySqlCommand(InsertCuisinier, connection))
                        {
                            cmdPart.Parameters.AddWithValue("@id", ID_Cuisinier);
                            cmdPart.Parameters.AddWithValue("@mdp", mdp);
                            cmdPart.Parameters.AddWithValue("@nom", nom);
                            cmdPart.Parameters.AddWithValue("@prenom", prenom);

                            cmdPart.ExecuteNonQuery();
                        }

                        string InsertClient = @"INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (@id, @mdp, @nomClient, @prenomClient, @SIRET);";
                        using(MySqlCommand cmdPart = new MySqlCommand(InsertClient, connection))
                        {
                            cmdPart.Parameters.AddWithValue("@id", ID_Client);
                            cmdPart.Parameters.AddWithValue("@mdp", mdp);
                            cmdPart.Parameters.AddWithValue("@nomClient", nom);
                            cmdPart.Parameters.AddWithValue("@prenomClient", prenom);
                            cmdPart.Parameters.AddWithValue("@SIRET", null);

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

        private int[]? InscValide_Ent(long siret, string nom_Ent, string nomRef, string telRef, int Nrue, string rue, int CP, string ville, string mdp)
        {
            int ID_Ent = GenIDSQL(0);   //obligé 0 car ne peut être qu'un Client
            int[]? list_ID = {ID_Ent};
            // Maintenant on insert le tout dans la BDD
            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root"))
                {
                    connection.Open();

                    string InsertEnt = @"INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (@SIRET, @NomE, @NomRef, @telRef, @NRue, @Rue, @CP, @ville);";
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

                        cmdEnt.ExecuteNonQuery();
                    }

                    //Et maintenant dans Client
                    string InsertClient = @"INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (@id, @mdp, @nomClient, @prenomClient, @SIRET);";
                    using(MySqlCommand cmdPart = new MySqlCommand(InsertClient, connection))
                    {
                        cmdPart.Parameters.AddWithValue("@id", ID_Ent);
                        cmdPart.Parameters.AddWithValue("@mdp", mdp);
                        cmdPart.Parameters.AddWithValue("@nomClient", null);
                        cmdPart.Parameters.AddWithValue("@prenomClient", null);     //Même argument que pour les Particulier devenant Client
                        cmdPart.Parameters.AddWithValue("@SIRET", siret);

                        cmdPart.ExecuteNonQuery();
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
                        ID = random.Next(1000, 10000);
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
                //Arbitraire car le while permet d'éviter une erreur, la seule erreur que nous pourrions avoir
                //est si l'intitulé de la connection est fausse, et même la on retombera sur nos pieds
                ///car quand l'inscription poursuivra, l'ID étant négatif il va être refusé
            }
        }
    }
}
