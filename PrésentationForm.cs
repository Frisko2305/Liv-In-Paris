using System.Drawing.Text;
namespace Liv_In_Paris
{
    public class PrésentationForm : Form
    {
        private Button Connection;
        private Button Inscription;
        private Button Carte;
        private Label Service;
        private PictureBox Picture;

        public PrésentationForm()
        {
            this.Text = "Liv-In Paris";
            this.Width = 500;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;    //Centre la fenêtre centre écran

            Connection = new Button();
            Connection.Text = "Se connecter";
            Connection.Font = new Font("Times New Roman", 12, FontStyle.Regular);
            Connection.AutoSize = true;
            Connection.Location = new Point(300,200);
            Connection.Click += Connection_Click;

            Inscription = new Button();
            Inscription.Text = "S'inscrire";
            Inscription.Font = new Font("Times New Roman", 12, FontStyle.Regular);
            Inscription.AutoSize = true;
            Inscription.Location = new Point(Connection.Location.X ,300);
            Inscription.Click += Inscription_Click;

            Carte = new Button();
            Carte.Text = "Carte des stations de Paris";
            Carte.Font = new Font("Times New Roman", 12, FontStyle.Regular);
            Carte.AutoSize = true;
            Carte.Location = new Point((this.ClientSize.Width/2) - Carte.Width, 400);
            Carte.Click += Carte_Click;


            Service = new Label();
            Service.Text = "Bienvenue sur Liv In Paris";
            Service.Font = new Font("CASTELLAR", 15, FontStyle.Bold);
            Service.AutoSize = true;
            Service.Location = new Point((this.ClientSize.Width - Service.PreferredSize.Width) /2, 30);

            //Ici ajouter pour l'image avec PictureBox Image
            Picture = new PictureBox();
            Picture.Image = Image.FromFile("Image_Présentation.png");   //filepath avec file à la racine du projet
            Picture.SizeMode = PictureBoxSizeMode.StretchImage;
            Picture.Size = new Size(240, 270);
            Picture.Location = new Point(20,115); 

            Controls.Add(Connection);
            Controls.Add(Inscription);
            Controls.Add(Service);
            Controls.Add(Picture);
            Controls.Add(Carte);
        }

        private void Connection_Click(object sender, EventArgs e)
        {
            Login_Form Form = new Login_Form();
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }

        private void Inscription_Click(object sender, EventArgs e)
        {
            Insc_Form Form = new Insc_Form();
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }

        private void Carte_Click(object? sender, EventArgs e)
        {
            // Carte_Form Form = new Carte_Form();
            // Form.Show();
            MessageBox.Show("En cours de développement. A voir à la prochaine mise à jour !!");
            // this.Hide();
            // Form.FormClosed += (s,args) => this.Close();
        }
    }
}

