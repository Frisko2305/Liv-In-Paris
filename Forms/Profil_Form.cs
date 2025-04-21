using System.Collections;
using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Profil : Form
    {
        #region Attributs
        private Dictionary<string, string> userInfo;
        private string userType;
        PictureBox? Photo_profil;

        // Puisqu'il y a énormèment de boutons qui diffère selon le profil, on opte plutôt pour une méthode qui va les construire

        #endregion

        public Profil(string userType, Dictionary<string,string> userInfo)
        {
            this.userType = userType;
            this.userInfo = userInfo;
            LancementProfil();
        }

        private void LancementProfil()
        {
            this.Text = $"Profil -- {userType}.";
            this.Width = 1050;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;    //Centre la fenêtre centre écran

            Photo_profil = new PictureBox();
            Photo_profil.Width = 150;
            Photo_profil.Height = 150;
            Photo_profil.Top = 20;
            Photo_profil.Left = (this.Width - Photo_profil.Width)/2;
            Photo_profil.SizeMode = PictureBoxSizeMode.Zoom;
            Photo_profil.Anchor = AnchorStyles.None;

            if(userInfo.ContainsKey("Photo_profil"))
            {
                try
                {
                    byte[] imagesBytes = Convert.FromBase64String(userInfo["Photo_profil"]);
                    using(var mem = new MemoryStream(imagesBytes))
                    {
                        Photo_profil.Image = Image.FromStream(mem); //Charge l'image depuis byte[] vers la PictureBox
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Erreur lors de l'affichage de l'image : {e.Message}");
                }
            }

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 10,
                ColumnCount = 2,
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for(int i = 0 ; i < layout.RowCount ; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            switch(userType)
            {
                case "Particulier" :
                    AddLabels(layout, userInfo["Nom"], userInfo["Prenom"]);
                    AddButton(layout, "Passer en mode Cuisinier", 2, 2, ModeCuistot_Click);
                    AddButton(layout, "Changer les informations", 2, 3, ChangerInfos_Click);
                    AddButtonPair(layout, "Passer commande", "Donner un avis", 4, PasserCommande_Click, DonnerAvis_Click);
                    AddButtonPair(layout, "Historique des commandes", "Changer moyens de paiements", 5, Historique_Click, ChangerPaiement_Click);
                    AddButtonPair(layout, "Se déconnecter", "Supprimer mon compte", 6, Deconnexion_Click, SuppCompte_Click);
                break;

                case "Entreprise" :
                    AddLabels(layout, userInfo["Ent_Nom"], userInfo["Ent_NomRef"]);
                    AddButton(layout, "Changer les informations", 2, 2, ChangerInfos_Click);
                    AddButtonPair(layout, "Passer commande", "Donner un avis", 3, PasserCommande_Click, DonnerAvis_Click);
                    AddButtonPair(layout, "Historique des commandes", "Changer moyens de paiements", 4, Historique_Click, ChangerPaiement_Click);
                    AddButtonPair(layout, "Se déconnecter", "Supprimer mon compte", 5, Deconnexion_Click, SuppCompte_Click);
                break;

                case "Cuisinier" :   
                    AddLabels(layout, userInfo["Nom"], userInfo["Prenom"]);
                    AddButton(layout, "Passer en mode Client", 2, 2, ModeClient_Click);
                    AddButton(layout, "Changer les informations", 2, 3, ChangerInfos_Click);
                    AddButtonPair(layout, "Ajouter un Plat", "Retirer un Plat", 4, AjoutPlat_Click, RetirePlat_Click);
                    AddButtonPair(layout, "Commande en cours", "Livraisons effectuées", 5, CommandesEnCours_Click, LivraisonsEffectuées_Click);
                    AddButton(layout, "Lire les avis de vos plats", 2, 6, LireAvis_Click);
                    AddButtonPair(layout, "Se déconnecter", "Supprimer mon compte", 7, Deconnexion_Click, SuppCompte_Click);
                break;

                default :
                    MessageBox.Show("Type d'utilisateur inconnu");
                break;
            }

            layout.Controls.Add(Photo_profil);
            layout.SetColumnSpan(Photo_profil, 2);
            this.Controls.Add(layout);
        }

        #region Création Attributs

        //Méthode pour ajouter les deux identités (Nom/Prénom ou Nom Entreprise/Nom Référent)
        private void AddLabels(TableLayoutPanel layout, string text1, string text2)
        {
            Label label1 = new Label{ Text = text1, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, AutoSize = true, Margin = new Padding(10), Font = new Font("Impact", 30)};
            Label label2 = new Label{ Text = text2, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, AutoSize = true, Margin = new Padding(10), Font = new Font("Impact", 30)};
            layout.Controls.Add(label1, 0, 1);
            layout.Controls.Add(label2, 1, 1);
        }

        //Méthode pour ajouter un seul bouton, centré dans le layout au travers des 2 colonnes
        private void AddButton(TableLayoutPanel layout, string text, int colSpan, int row, EventHandler onClick)
        {
            Button button = new Button { Text = text, AutoSize = true, Anchor = AnchorStyles.None, Font = new Font("Arial Black", 20)};
            button.Click += onClick;
            layout.Controls.Add(button, 0, row);
            layout.SetColumnSpan(button, colSpan);
        }

        //Méthode pour ajouter deux boutons côtes à côtes sur une ligne dans le layout
        private void AddButtonPair(TableLayoutPanel layout, string text1, string text2, int row, EventHandler onClick1, EventHandler onClick2)
        {
            Button button1 = new Button{ Text = text1, AutoSize = true, Anchor = AnchorStyles.None, Font = new Font("Arial Black", 20)};
            button1.Click += onClick1;

            Button button2 = new Button{ Text = text2, AutoSize = true, Anchor = AnchorStyles.None, Font = new Font("Arial Black", 20)};
            button2.Click += onClick2;

            layout.Controls.Add(button1, 0, row);
            layout.Controls.Add(button2, 1, row);
        }

        #endregion
        #region Boutons Communs

        private void ChangerInfos_Click(object? sender, EventArgs e)
        {
            ChgInfo Form = new ChgInfo(userType, userInfo);
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }

        private void Deconnexion_Click(object? sender, EventArgs e)
        {
            PrésentationForm Form = new PrésentationForm();
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
// 
        }

        private void SuppCompte_Click(object? sender, EventArgs e)
        {
// 
        }

        #endregion
        #region Boutons Client

        private void ModeCuistot_Click(object? sender, EventArgs e)
        {
// 
        }

        private void ChangerPaiement_Click(object? sender, EventArgs e)
        {

        }

        private void PasserCommande_Click(object? sender, EventArgs e)
        {
            
        }

        private void DonnerAvis_Click(object? sender, EventArgs e)
        {
// 
        }

        #endregion
        #region Buttons Cuisinier

        private void LireAvis_Click(object? sender, EventArgs e)
        {
// 
        }

        private void Historique_Click(object? sender, EventArgs e)
        {

        }

        private void LivraisonsEffectuées_Click(object? sender, EventArgs e)
        {

        }

        private void CommandesEnCours_Click(object? sender, EventArgs e)
        {

        }

        private void AjoutPlat_Click(object? sender, EventArgs e)
        {
// 
        }

        private void RetirePlat_Click(object? sender, EventArgs e)
        {
// 
        }

        private void ModeClient_Click(object? sender, EventArgs e)
        {
// 
        }

        #endregion
    }
}

