using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class SuppCompte : Form
    {
        #region Attribut
        List<string> comptesDispo;
        private Button? Valider;
        private Button? Retour;
        private TextBox? Saisie;
        private Label? labelComptes;
        private TableLayoutPanel? layout;
        private string userType;
        private Dictionary<string,string> userInfo;
        #endregion
        public SuppCompte(string userType, Dictionary<string,string> userInfo)
        {
            this.Text = $"Suppression Comptes de {userInfo["Prenom"]} {userInfo["Nom"]}";
            this.Size = new Size(450,150);
            this.userType = userType;
            this.userInfo = userInfo;
            this.StartPosition = FormStartPosition.CenterScreen;

            comptesDispo = new List<string>();
            if (userType == "Particulier")
            {
                if (!string.IsNullOrEmpty(userInfo["Id_cuisinier"]))
                {
                    comptesDispo.Add("Cuisinier");
                }
                if(!string.IsNullOrEmpty(userInfo["Id_client"]))
                {
                    comptesDispo.Add("Client");
                }
            }
            else if(userType == "Cuisinier")
            {
                if(!string.IsNullOrEmpty(userInfo["Id_client"]))
                {
                    comptesDispo.Add("Client");
                }
                if (!string.IsNullOrEmpty(userInfo["Id_cuisinier"]))
                {
                    comptesDispo.Add("Cuisinier");
                }
            }
            else if (userType == "Entreprise")
            {
                comptesDispo.Add("Entreprise");
            }

            if(comptesDispo.Count == 0)
            {
                MessageBox.Show("Aucun compte associé trouvé ;", "Information : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            layout = new TableLayoutPanel
            {
                RowCount = 3,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            labelComptes = new Label
            {
                Dock =  DockStyle.Fill,
                TextAlign  = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = true
            };
            string message = "Vous avez plusieurs comptes associés. Veuillez choisir lequel vous souhaitez supprimer :\n";
            for(int i = 0 ; i < comptesDispo.Count ; i++)
            {
                message += $"{i + 1}. {comptesDispo[i]}\n";
            }
            if (userType == "Particulier" && comptesDispo.Count > 1)
            {
                message += $"{comptesDispo.Count + 1}. Tous les comptes\n";
            }
            labelComptes.Text = message;

            Valider = new Button
            {
                Text = "Valider ?",
                Dock = DockStyle.Fill
            };
            Valider.Click += Valider_Click;

            Retour = new Button
            {
                Text = "Retour",
                Dock = DockStyle.Fill
            };
            Retour.Click += Retour_Click;

            Saisie = new TextBox
            {
                Dock = DockStyle.Fill
            };

            layout.Controls.Add(labelComptes, 0, 0);
            layout.SetRowSpan(labelComptes, 2);
            layout.Controls.Add(Valider, 1, 0);
            layout.Controls.Add(Retour, 1, 1);
            layout.Controls.Add(Saisie, 0, 2);
            layout.SetColumnSpan(Saisie, 2);

            this.Controls.Add(layout);
        }

        private bool SupprimerCompte(string compteType, Dictionary<string,string> userInfo)
        {
            string connectionstring = "SERVER=localhost;DATABASE=psi;UID=root;PWD=root";
            using(MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                bool queryReussi = false;

                try
                {
                    switch(compteType)
                    {
                        case "Particulier":
                            string querySuppParticulier = "DELETE FROM Particulier WHERE Nom = @Nom AND Prenom = @Prenom;";
                            cmd = new MySqlCommand(querySuppParticulier, connection);
                            cmd.Parameters.AddWithValue("@Nom", userInfo["Nom"]);
                            cmd.Parameters.AddWithValue("@Prenom", userInfo["Prenom"]);
                            queryReussi = cmd.ExecuteNonQuery() > 0;
                            cmd.Dispose();
                        break;

                        case "Client" :
                            string querySuppClient = "DELETE FROM Client WHERE Id_client = @Id;";
                            cmd = new MySqlCommand(querySuppClient, connection);
                            cmd.Parameters.AddWithValue("@Id", userInfo["Id_client"]);
                            queryReussi = cmd.ExecuteNonQuery() > 0;
                            cmd.Dispose();
                        break;

                        case "Cuisinier" :
                            string querySuppCuisinier = "DELETE FROM Cuisinier WHERE Id_cuisinier = @Id;";
                            cmd = new MySqlCommand(querySuppCuisinier, connection);
                            cmd.Parameters.AddWithValue("@Id", userInfo["Id_cuisinier"]);
                            queryReussi = cmd.ExecuteNonQuery() > 0;
                            cmd.Dispose();
                        break;

                        case "Entreprise" :
                            string querySuppEntreprise = "DELETE FROM Entreprise WHERE SIRET = @SIRET;";
                            cmd = new MySqlCommand(querySuppEntreprise, connection);
                            cmd.Parameters.AddWithValue("@SIRET", userInfo["SIRET"]);
                            queryReussi = cmd.ExecuteNonQuery() > 0;
                            cmd.Dispose();
                        break;

                        default :
                            MessageBox.Show("Erreur inattendu. Aucune action effectuée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppresion du compte {compteType} : "+ ex.Message);
                }

                return queryReussi;
            }
        }

        private void Valider_Click(object? sender, EventArgs e)
        {
            string saisie = Saisie.Text.Trim();

            if(int.TryParse(saisie, out int choixIndex) && choixIndex > 0 && choixIndex <= comptesDispo.Count + 1)
                {
                    if(choixIndex == comptesDispo.Count + 1)
                    {
                        if(userType == "Particulier")
                        {
                            if(SupprimerCompte("Particulier", userInfo))
                            {
                                MessageBox.Show("Suppression de tous les comptes réussie. Retour au menu principale.");
                                PrésentationForm Form = new PrésentationForm();
                                Form.Show();

                                this.Hide();
                                Form.FormClosed += (s,args) => this.Close();
                            }
                        }
                    }
                    else
                    {
                        if(SupprimerCompte(comptesDispo[choixIndex -1], userInfo))
                        {
                            MessageBox.Show("Suppression du compte réussie. Retour à la page de présentation.");
                            PrésentationForm Form = new PrésentationForm();
                            Form.Show();

                            this.Hide();
                            Form.FormClosed += (s,args) => this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Saisie invalide. Veuillez entrer un numéro valide parmis la liste proposé.");
                    return;
                }
        }

        private void Retour_Click(object? sender, EventArgs e)
        {
            Profil Form = new Profil(userType, userInfo);
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }
    }
}

