using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Choix_Plat : Form
    {
        #region Attribut
        
        ComboBox TypePlat, Regime, NbPers;
        Button Retour, ValideFiltre, VoirPanier;
        TableLayoutPanel mainlayout;
        FlowLayoutPanel Platlayout;
        Dictionary<string, string> userInfo;
        List<Dictionary<string, string>> ListePlatFiltré, ListePlatChoisi ;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        public Choix_Plat(Dictionary<string,string> userInfo)
        {
            this.Text = "Choix du Plat";
            this.Size = new Size(750, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.userInfo = userInfo;

            ListePlatChoisi = new List<Dictionary<string, string>>();

            TypePlat = new ComboBox { Text = "Type de plat" };
            Regime = new ComboBox { Text = "Régime alimentaire" };
            NbPers = new ComboBox { Text = "Nombre de personnes" };

            
            TypePlat = new ComboBox { Text = "Type de plat"};
            Regime = new ComboBox { Text = "Régime alimentaire"};
            NbPers = new ComboBox { Text = "Nombre de personnes"};

            TypePlat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            TypePlat.AutoCompleteSource = AutoCompleteSource.ListItems;

            Regime.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Regime.AutoCompleteSource = AutoCompleteSource.ListItems;

            NbPers.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            NbPers.AutoCompleteSource = AutoCompleteSource.ListItems;

            TypePlat.Items.Insert(0, "-- Tous --");
            Regime.Items.Insert(0, "-- Tous --");
            NbPers.Items.Insert(0, "-- Tous --");

            TypePlat.SelectedIndex = 0;
            Regime.SelectedIndex = 0;
            NbPers.SelectedIndex = 0;

            AjoutComboBox("Type de plat" );
            AjoutComboBox("Régime alimentaire");
            AjoutComboBox("Nombre de personnes");

            RemplirComboBoxFiltre();

        
            ValideFiltre = new Button { Text = "Valider les filtres", AutoSize = true};
            
            ValideFiltre.Click += ValideFiltre_Click;

            Retour = new Button { Text = "Retour", AutoSize = true };
            Retour.Click += Retour_Click;

            VoirPanier = new Button { Text = "Voir votre panier", AutoSize = true };
            VoirPanier.Click += VoirPanier_Click;

            Platlayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Visible = false 
            };

            mainlayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                AutoSize = true
            };


            var filterPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };
            filterPanel.Controls.Add(TypePlat);
            filterPanel.Controls.Add(Regime);
            filterPanel.Controls.Add(NbPers);


            var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };
            buttonPanel.Controls.Add(ValideFiltre);
            buttonPanel.Controls.Add(Retour);


            mainlayout.Controls.Add(filterPanel);
            mainlayout.Controls.Add(buttonPanel);
            mainlayout.Controls.Add(VoirPanier);
            mainlayout.Controls.Add(Platlayout);

            this.Controls.Add(mainlayout);
        }

        private void AjoutComboBox(string placeholderText)
        {
            ComboBox box = new ComboBox();
            box.ForeColor = Color.Gray;

            box.Enter += (sender, e) =>
            {
                if(box.Text == placeholderText)
                {
                    box.Text = "";
                    box.ForeColor = Color.Black;
                }
            };

            box.Leave += (sender, e) =>
            {
                if(string.IsNullOrWhiteSpace(box.Text))
                {
                    box.Text = placeholderText;
                    box.ForeColor = Color.Gray;
                }
            };
        }

        private void Retour_Click(object? sender, EventArgs e)
        {
            Profil Form = new Profil("Particulier", userInfo);
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }

        private void VoirPanier_Click(object? sender, EventArgs e)
        {
            if (ListePlatChoisi.Count == null)
            {
                MessageBox.Show("Votre panier est vide.");
                return;
            }

            Commande commandeForm = new Commande(ListePlatChoisi, userInfo);
            commandeForm.Show();
            this.Hide();

            commandeForm.FormClosed += (s, args) => this.Show();
        }

        private void RemplirComboBoxFiltre()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root"))
                {
                    connection.Open();

                    RemplirComboBox(TypePlat, "SELECT DISTINCT Type_de_cuisine FROM Plat ORDER BY Type_de_cuisine", "Type_de_cuisine", connection);

                    RemplirComboBox(Regime, "SELECT DISTINCT Regime_alimentaire FROM Plat WHERE Regime_alimentaire IS NOT NULL ORDER BY Regime_alimentaire", "Regime_alimentaire", connection);

                    RemplirComboBox(NbPers, "SELECT DISTINCT Nb_personnes FROM Plat ORDER BY Nb_personnes", "Nb_personnes", connection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du remplissage des filtres : {ex.Message}");
            }
        }

        private void RemplirComboBox(ComboBox comboBox, string query, string columnName, MySqlConnection connection)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBox.Items.Clear();
                        comboBox.Items.Add($"-- {comboBox.Text} --"); 

                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader[columnName].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du remplissage de la ComboBox {comboBox.Name} : {ex.Message}");
            }
        }

        private void ValideFiltre_Click(object? sender, EventArgs e)
        {
            ListePlatFiltré = new List<Dictionary<string, string>>();
            try
            {
                using(MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root"))
                {
                    string query = @"
                    SELECT
                        p.Id_plat,
                        p.Nom_de_plat,
                        p.Stock,
                        p.Nb_personnes,
                        p.Prix,
                        p.Date_fabrication,
                        p.Date_peremption,
                        p.Type_de_cuisine,
                        p.Regime_alimentaire,
                        c.Nom_cuisinier,
                        c.Prenom_cuisinier
                    FROM Plat p
                    JOIN Propose pr ON p.Id_plat = pr.Id_plat
                    JOIN Cuisinier c ON pr.Id_cuisinier = c.Id_cuisinier
                    WHERE (@TypePlat IS NULL OR p.Type_de_cuisine = @TypePlat)
                        AND (@Regime IS NULL or p.Regime_alimentaire = @Regime)
                        AND (@NbPers IS NULL OR p.Nb_personnes >= @NbPers);";

                    connection.Open();

                    using(MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TypePlat", TypePlat.Text == "-- Tous --" ? (object)DBNull.Value : TypePlat.Text);
                        cmd.Parameters.AddWithValue("@Regime", Regime.Text == "-- Tous --" ? (object)DBNull.Value : Regime.Text);
                        cmd.Parameters.AddWithValue("@NbPers", NbPers.Text == "-- Tous --" ? (object)DBNull.Value : NbPers.Text);

                        using(MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                var plat = new Dictionary<string, string>
                                {
                                    { "Id", reader[reader.GetName(0)].ToString() },
                                    { "NomPlat", reader[reader.GetName(1)].ToString() },
                                    { "Stock", reader[reader.GetName(2)].ToString() },
                                    { "NbPers", reader[reader.GetName(3)].ToString() },
                                    { "Prix", reader[reader.GetName(4)].ToString() },
                                    { "DateF", reader[reader.GetName(5)].ToString() },
                                    { "DateP", reader[reader.GetName(6)].ToString() },
                                    { "TypePlat", reader[reader.GetName(7)].ToString() },
                                    { "Regime", reader[reader.GetName(8)].ToString() },
                                    { "Nom_cuisinier", reader[reader.GetName(9)].ToString() },
                                    { "Prenom_cuisinier", reader[reader.GetName(10)].ToString() }
                                };

                                ListePlatFiltré.Add(plat);
                            }
                            reader.Close();
                        }
                        cmd.Dispose();
                    }
                    connection.Close();
                }

                AjoutPlat();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des plats dans les serveurs : "+ex.Message);
            }
        }

        private void AjoutPlat()
        {
            Platlayout.Controls.Clear();

            foreach (var plat in ListePlatFiltré)
            {
                var platPanel = new Panel
                {
                    Width = 700,
                    Height = 120,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    Padding = new Padding(10)
                };

                var tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 5,
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

                var addButton = new Button
                {
                    Text = "Ajouter au panier",
                    AutoSize = true,
                    Tag = plat
                };

                addButton.Click += AjoutAuPanier;

                tableLayout.Controls.Add(addButton, 1, 4);

                platPanel.Controls.Add(tableLayout);
                Platlayout.Controls.Add(platPanel);
            }

            Platlayout.Visible = true;
        }

        private void AjoutAuPanier(object? sender, EventArgs e)
        {
            if (sender is Button addButton && addButton.Tag is Dictionary<string, string> plat)
            {
                if (!ListePlatChoisi.Any(p => p["Id"] == plat["Id"] && p["Nom_cuisinier"] == plat["Nom_cuisinier"] && p["Prenom_cuisinier"] == plat["Prenom_cuisinier"]))
                {
                    ListePlatChoisi.Add(plat);
                    MessageBox.Show($"Le plat '{plat["NomPlat"]}' de {plat["Prenom_cuisinier"]} {plat["Nom_cuisinier"]} a été ajouté au panier.");
                }
                else
                {
                    MessageBox.Show($"Le plat '{plat["NomPlat"]}' de {plat["Prenom_cuisinier"]} {plat["Nom_cuisinier"]} est déjà dans le panier.");
                }
            }
        }
    }
}