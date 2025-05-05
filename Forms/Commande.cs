using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Commande : Form
    {
        private List<Dictionary<string, string>> ListePlatChoisi;
        private Dictionary<string, string> userInfo;

        public Commande(List<Dictionary<string, string>> listePlatChoisi, Dictionary<string, string> userInfo)
        {
            this.ListePlatChoisi = listePlatChoisi;
            this.userInfo = userInfo;

            this.Text = "Votre Commande";
            this.Size = new Size(750, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                AutoSize = true
            };

            var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };

            var retourButton = new Button { Text = "Retour", AutoSize = true };
            retourButton.Click += Retour_Click;

            var confirmerButton = new Button { Text = "Confirmer la commande", AutoSize = true };
            confirmerButton.Click += ConfirmerCommande_Click;

            buttonPanel.Controls.Add(retourButton);
            buttonPanel.Controls.Add(confirmerButton);

            var platLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            foreach (var plat in ListePlatChoisi)
            {
                var platPanel = new Panel
                {
                    Width = 700,
                    Height = 150,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    Padding = new Padding(10)
                };

                var tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 6,
                    AutoSize = true
                };

                tableLayout.Controls.Add(new Label { Text = "Nom :", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 0);
                tableLayout.Controls.Add(new Label { Text = plat["NomPlat"], AutoSize = true }, 1, 0);

                tableLayout.Controls.Add(new Label { Text = "Prix :", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 1);
                tableLayout.Controls.Add(new Label { Text = $"{plat["Prix"]} €", AutoSize = true }, 1, 1);

                tableLayout.Controls.Add(new Label { Text = "Stock :", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 2);
                tableLayout.Controls.Add(new Label { Text = plat["Stock"], AutoSize = true }, 1, 2);

                tableLayout.Controls.Add(new Label { Text = "Cuisinier :", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 3);
                tableLayout.Controls.Add(new Label { Text = $"{plat["Prenom_cuisinier"]} {plat["Nom_cuisinier"]}", AutoSize = true }, 1, 3);

                tableLayout.Controls.Add(new Label { Text = "Date de fabrication :", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 4);
                tableLayout.Controls.Add(new Label { Text = plat["DateF"], AutoSize = true }, 1, 4);

                tableLayout.Controls.Add(new Label { Text = "Date de péremption :", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 5);
                tableLayout.Controls.Add(new Label { Text = plat["DateP"], AutoSize = true }, 1, 5);

                platPanel.Controls.Add(tableLayout);
                platLayout.Controls.Add(platPanel);
            }

            mainLayout.Controls.Add(buttonPanel);
            mainLayout.Controls.Add(platLayout);

            this.Controls.Add(mainLayout);
        }
        
        private void Retour_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfirmerCommande_Click(object? sender, EventArgs e)
        {
            try
            {
                Graphe<double> graphe = new Graphe<double>();
                Program.PeuplementTable(graphe);

                // Nettoyer les données de recherche
                string metroUtilisateur = userInfo["Metro"].Trim().ToLower();
                string metroCuisinier = ListePlatChoisi[0]["Metro"].Trim().ToLower();

                // Rechercher les nœuds correspondants
                var noeudUtilisateur = graphe.Noeuds_Pte.FirstOrDefault(n => n.Libelle.Trim().ToLower() == metroUtilisateur);
                if (noeudUtilisateur == null)
                {
                    MessageBox.Show($"La station de métro '{userInfo["Metro"]}' de l'utilisateur est introuvable dans le graphe.");
                    return;
                }

                var noeudCuisinier = graphe.Noeuds_Pte.FirstOrDefault(n => n.Libelle.Trim().ToLower() == metroCuisinier);
                if (noeudCuisinier == null)
                {
                    MessageBox.Show($"La station de métro '{ListePlatChoisi[0]["Metro"]}' du cuisinier est introuvable dans le graphe.");
                    return;
                }

                int[,] distances = Program.FloydWarshall(graphe);

                int indexUtilisateur = graphe.Noeuds_Pte.IndexOf(noeudUtilisateur);
                int indexCuisinier = graphe.Noeuds_Pte.IndexOf(noeudCuisinier);

                if (distances[indexUtilisateur, indexCuisinier] == int.MaxValue)
                {
                    MessageBox.Show("Il n'y a pas de chemin entre les deux stations.");
                    return;
                }

                int distance = distances[indexUtilisateur, indexCuisinier];

                DateTime previsionArrivee = DateTime.Now.AddMinutes(distance);

                double prixTotal = ListePlatChoisi.Sum(plat => double.Parse(plat["Prix"]));

                try
                {
                    bool Reussi = false;

                    using (var connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root"))
                    {
                        connection.Open();

                        string query = @"
                            INSERT INTO Commande (Prix, Statut, Prevision_arrivee, Id_client)
                            VALUES (@Prix, @Statut, @Prevision_arrivee, @Id_client)";

                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Prix", prixTotal);
                            command.Parameters.AddWithValue("@Statut", "En préparation");
                            command.Parameters.AddWithValue("@Prevision_arrivee", previsionArrivee);
                            command.Parameters.AddWithValue("@Id_client", userInfo["Id_client"]);

                            Reussi = command.ExecuteNonQuery() > 0;
                        }

                        if(Reussi)
                        {   
                            long idCommande;
                            using (var command = new MySqlCommand("SELECT LAST_INSERT_ID();", connection))
                            {
                            idCommande = (long)command.ExecuteScalar();
                            }

                            foreach (var plat in ListePlatChoisi)
                            {
                                string queryPlat = @"
                                    INSERT INTO Commande_Plat (Id_commande, Id_plat)
                                    VALUES (@Id_commande, @Id_plat)";

                                using (var commandPlat = new MySqlCommand(queryPlat, connection))
                                {
                                    commandPlat.Parameters.AddWithValue("@Id_commande", idCommande);
                                    commandPlat.Parameters.AddWithValue("@Id_plat", plat["Id"]);
                                    commandPlat.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("Commande confirmée avec tous les plats associés !");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'insertion de la commande : {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des nœuds : {ex.Message}");
            }

            
        }
    }
}