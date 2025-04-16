using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Profil : Form
    {
        #region Attributs
        private Dictionary<string, string> infos;
        private string userType;
        PictureBox Photo_profil;

        #endregion

        public Profil(string userType, Dictionary<string,string> infos)
        {
            this.userType = userType;
            this.infos = infos;

            LancementProfil();
        }

        private void LancementProfil()
        {
            this.Text = $"Profil -- {userType}.";
            this.Width = 750;
            this.Height = 1000;
            this.StartPosition = FormStartPosition.CenterScreen;    //Centre la fenêtre centre écran

            Photo_profil = new PictureBox();
            Photo_profil.Width = 150;
            Photo_profil.Height = 150;
            Photo_profil.Top = 20;
            Photo_profil.Left = (this.Width - Photo_profil.Width)/2;
            Photo_profil.SizeMode = PictureBoxSizeMode.Zoom;

            if(infos.ContainsKey("Photo_profil"))
            {
                try
                {
                    byte[] imagesBytes = Convert.FromBase64String(infos["Photo_profil"]);
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
            else
            {
                MessageBox.Show("Aucune image trouvé");
            }

            this.Controls.Add(Photo_profil);

            MessageBox.Show("Votre Profil arrivera bientôt dans la prochaine MAJ !!");
        }
    }
}

