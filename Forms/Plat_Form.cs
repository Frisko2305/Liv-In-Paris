using MySql.Data.MySqlClient;
namespace Liv_In_Paris
{
    public class Plat : Form
    {
        #region Attributs
        private Dictionary<string,string> userInfo;
        private TableLayoutPanel? layout;
        private TextBox? T_NomPlat, T_Stock, T_Nb_personnes, T_Prix, T_Date_Fab, T_Date_Perem, T_TypeCuisinie, T_RegimePlat;
        
        private Button? Retour, Ajout_Valide, Retire_Valide, AjoutPhoto;
        byte[]? imagePlat;

        #endregion
        public Plat(Dictionary<string, string> userInfo, bool Ajout)
        {
            this.userInfo = userInfo;
            if(Ajout)
            {
                InitializeComponent_AjoutPlat();
            }
            else
            {
                InitializeComponent_RetirerPlat();
            }
        }

        private void InitializeComponent_AjoutPlat()
        {
            this.Text = $"Ajouter un Plat par {userInfo["Prenom"]} {userInfo["Nom"]}";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 10
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            T_NomPlat = AjoutLabelEtTextBox("Nom du plat", 0);
            T_Stock = AjoutLabelEtTextBox("Stock", 1);
            T_Nb_personnes = AjoutLabelEtTextBox("Nombre de personnes", 2);
            T_Prix = AjoutLabelEtTextBox("Prix", 3);
            T_Date_Fab = AjoutLabelEtTextBox("Date de fabrication", 4);
            T_Date_Perem = AjoutLabelEtTextBox("Date de péremption", 5);
            T_TypeCuisinie = AjoutLabelEtTextBox("Type de cuisine", 6);
            T_RegimePlat = AjoutLabelEtTextBox("Régime alimentaire (vide si aucun)", 7);

            AjoutPhoto = new Button { Text = "Sélectionnez une photo de votre plat (optionnel)", Dock = DockStyle.Fill };
            AjoutPhoto.Click += AjoutPhoto_Click;

            Retour = new Button { Text = "Retour", Dock = DockStyle.Fill };
            Retour.Click += Retour_Click;

            Ajout_Valide = new Button { Text = "Valider", Dock = DockStyle.Fill };
            Ajout_Valide.Click += Ajout_Click;

            layout.Controls.Add(AjoutPhoto, 0, 8);
            layout.SetColumnSpan(AjoutPhoto, 2);
            layout.Controls.Add(Retour, 0, 9);
            layout.Controls.Add(Ajout_Valide, 1, 9);

            this.Controls.Add(layout);
        }

        private void InitializeComponent_RetirerPlat()
        {
            this.Text = $"Ajouter un Plat par {userInfo["Prenom"]} {userInfo["Nom"]}";
            this.Size = new Size(450, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            T_NomPlat = AjoutLabelEtTextBox("Nom du plat", 0);
            T_Date_Fab = AjoutLabelEtTextBox("Date de fabrication", 1);
            T_Date_Perem = AjoutLabelEtTextBox("Date de péremption", 2);

            Retour = new Button { Text = "Retour", Anchor = AnchorStyles.Top };
            Retour.Click += Retour_Click;

            Retire_Valide = new Button { Text = "Valider", Anchor = AnchorStyles.Top};
            Retire_Valide.Click += Retire_Click;

            layout.Controls.Add(Retour, 0, 3);
            layout.Controls.Add(Retire_Valide, 1, 3);

            this.Controls.Add(layout);
        }

        private TextBox AjoutLabelEtTextBox(string labelText, int row)
        {
            Label label = new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight };
            TextBox textBox = new TextBox { Dock = DockStyle.Fill };
            layout.Controls.Add(label, 0, row);
            layout.Controls.Add(textBox, 1, row);
            return textBox;
        }

        private void Retour_Click(object? sender, EventArgs e)
        {
            Profil Form = new Profil("Cuisinier", userInfo);
            Form.Show();
            
            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }

        private void AjoutPhoto_Click(object? sender, EventArgs e)
        {
            using(OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Images (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                fileDialog.Title = "Sélectionnez une image";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = fileDialog.FileName;
                    imagePlat = File.ReadAllBytes(path);
                    MessageBox.Show("Image sélectionnée : "+ fileDialog.FileName);
                }
            }
        }

        private void Retire_Click(object? sender, EventArgs e)
        {
            string resultFab = FormatDate(T_Date_Fab.Text);
            string resultPeremp = FormatDate(T_Date_Perem.Text);
            if(resultFab == string.Empty && resultPeremp == string.Empty)
            {
                return;
            }

            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root"))
                {
                    connection.Open();

                    string RechercheIdquery = @"DELETE FROM Plat WHERE Nom_de_plat = @Nom AND Date_fabrication = @DateF AND Date_peremption = @DateP;";
                    bool Reussi = false;

                    using(MySqlCommand cmd = new MySqlCommand(RechercheIdquery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nom", T_NomPlat.Text);
                        cmd.Parameters.AddWithValue("@DateF", resultFab);
                        cmd.Parameters.AddWithValue("@DateP", resultPeremp);
                        Reussi = cmd.ExecuteNonQuery() > 0;
                        cmd.Dispose();
                    }

                    if(Reussi)
                    {
                        MessageBox.Show("Le plat a été retiré avec succès. Retour vers le Profil");
                        Profil Form = new Profil("Cuisinier", userInfo);
                        Form.Show();

                        this.Hide();
                        Form.FormClosed += (s,args) => this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Le plat n'a pas pu être retiré, veuillez revoir vos informations. Retour vers le Profil");
                        Profil Form = new Profil("Cuisinier", userInfo);
                        Form.Show();

                        this.Hide();
                        Form.FormClosed += (s,args) => this.Close();
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du Plat : "+ ex.Message);
            }
        }

        private void Ajout_Click(object? sender, EventArgs e)
        {
            string resultFab = FormatDate(T_Date_Fab.Text);
            string resultPeremp = FormatDate(T_Date_Perem.Text);
            if(resultFab == string.Empty && resultPeremp == string.Empty)
            {
                return;
            }

            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root"))
                {
                    connection.Open();

                    bool PlatReussi = false;
                    bool ProposeReussi = false;
                    string AjoutPlatquery = @"INSERT INTO Plat(
                    Id_plat,
                    Nom_de_plat,
                    Stock,
                    Nb_personnes,
                    Prix,
                    Date_fabrication,
                    Date_peremption,
                    Type_de_cuisine,
                    Regime_alimentaire,
                    Photo)
                    VALUES(
                    @IdP,
                    @Nom,
                    @Stock,
                    @Nb,
                    @Prix,
                    @DateF,
                    @DateP,
                    @Type,
                    @Regime,
                    @Photo );";

                    string AjoutProposequery = @"INSERT INTO Propose(
                    Id_cuisinier,
                    Id_plat)
                    VALUES(
                    @IdC,
                    @IdP);";

                    string ID = GenIDPlat().ToString();

                    using(MySqlCommand AjoutPlatcmd = new MySqlCommand(AjoutPlatquery, connection))
                    {
                        AjoutPlatcmd.Parameters.AddWithValue("@IdP", ID);
                        AjoutPlatcmd.Parameters.AddWithValue("@Nom", T_NomPlat.Text);
                        AjoutPlatcmd.Parameters.AddWithValue("@Stock", T_Stock.Text);
                        AjoutPlatcmd.Parameters.AddWithValue("@Nb", T_Nb_personnes.Text);
                        AjoutPlatcmd.Parameters.AddWithValue("@Prix", T_Prix.Text);
                        AjoutPlatcmd.Parameters.AddWithValue("@DateF", resultFab);
                        AjoutPlatcmd.Parameters.AddWithValue("@DateP", resultPeremp);
                        AjoutPlatcmd.Parameters.AddWithValue("@Type", T_TypeCuisinie.Text);
                        AjoutPlatcmd.Parameters.AddWithValue("@Regime", T_RegimePlat.Text);
                        AjoutPlatcmd.Parameters.AddWithValue("@Photo", imagePlat);

                        PlatReussi = AjoutPlatcmd.ExecuteNonQuery() > 0;
                    }

                    using(MySqlCommand ProposePlatcmd = new MySqlCommand(AjoutProposequery, connection))
                    {
                        ProposePlatcmd.Parameters.AddWithValue("@IdP", ID);
                        ProposePlatcmd.Parameters.AddWithValue("@IdC", userInfo["Id_cuisinier"]);

                        ProposeReussi = ProposePlatcmd.ExecuteNonQuery() > 0;
                    }

                    if(PlatReussi && ProposeReussi)
                    {
                        MessageBox.Show($"Votre plat : {T_NomPlat.Text} a bien été ajouté aux serveurs, nous espérons que votre plat satisfera le plus de clients !");
                        Profil Form = new Profil("Cuisinier", userInfo);
                        Form.Show();

                        this.Hide();
                        Form.FormClosed += (s,args) => this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Votre plat : {T_NomPlat.Text} a bien été ajouté aux serveurs, veuillez revoir vos informations");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout du Plat : "+ex.Message);
            }
        }

        private int GenIDPlat()
        {
            Random random = new Random();
            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root"))
                {
                    connection.Open();
                    string query = @"SELECT COUNT(*) FROM Plat WHERE Id_plat = @id";
                    do
                    {
                        int ID = random.Next(1, 10000);
                        int result = 0;
                        using(MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@id", ID);
                            result = Convert.ToInt32(cmd.ExecuteScalar());
                            if(result == 0)
                            {
                                return ID;
                            }
                            cmd.Dispose();
                        }
                        connection.Close();
                    } while(true);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur lors de la génération d'identifiant pour le plat : "+ ex.Message);
                return -1;
            }
        }
        
        private string FormatDate(string text)
        {
            if(System.Text.RegularExpressions.Regex.IsMatch(text, @"^\d{2}/\d{2}/\d{4}"))
            {
                try
                {
                    DateTime date = DateTime.ParseExact(text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    return date.ToString("yyyy-MM-dd");
                }
                catch(FormatException)
                {
                    MessageBox.Show("La date saisie est invalide. Veuillez respectez le format JJ/MM/AAAA avant de continuer");
                }
            }
            else
            {
                MessageBox.Show("La date saisie ne respecte pas le format JJ/MM/AAAA avant de continuer");
                return string.Empty;
            }
            return string.Empty;
        }
    }
}