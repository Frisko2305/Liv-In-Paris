using MySql.Data.MySqlClient;

namespace Liv_In_Paris
{
    public class Login_Form : Form
    {
        private Label ID;
        private Label PWD;
        private TextBox Id_box;
        private TextBox Pwd_box;
        private TableLayoutPanel? layout;
        private Button Login;
        private Button Retour;

        public Login_Form()
        {
            this.Text = "Login";
            this.Width = 350;
            this.Height = 175;
            this.StartPosition = FormStartPosition.CenterScreen;    //Centre la fenêtre centre écran

            layout = new TableLayoutPanel();           //avec un layout
            layout.RowCount = 2; //ID then PWD with 1 for space in-between
            layout.ColumnCount = 2;  //Label then Field
            layout.Dock = DockStyle.Fill;       //Remplis les cases pour pouvoir y mettre les choses
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));  //Autosize pour ID
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));  //Autosize pour PWD

            ID = new Label();
            ID.Text = "ID";
            ID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            ID.AutoSize = true;

            PWD = new Label();
            PWD.Text = "Password";
            PWD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            PWD.AutoSize = true; 

            Id_box = new TextBox();
            Id_box.Width = 150;

            Pwd_box = new TextBox();
            Pwd_box.Width = 150;

            Login = new Button();
            Login.Text = "Login";
            Login.AutoSize = true;
            Login.Width = 100;
            Login.Location = new Point((this.ClientSize.Width /2) , 75);
            Login.Click += button_Click;

            Retour = new Button();
            Retour.Text = "Retour";
            Retour.AutoSize = true;
            Retour.Width = 100;
            Retour.Location = new Point((Retour.Width) /2, 75);
            Retour.Click += Retour_Click;  

            layout.Controls.Add(ID,0,0);
            layout.Controls.Add(Id_box,0,1);
            layout.Controls.Add(PWD,1,0);
            layout.Controls.Add(Pwd_box,1,1);

            this.Controls.Add(Login);
            this.Controls.Add(Retour);
            this.Controls.Add(layout);
        }

        private void button_Click(object? sender, EventArgs e)
        {
            string userId = Id_box.Text;
            string userpwd = Pwd_box.Text;

            if(LoginValide(userId,userpwd) == 0)    //<-- Client Particulier
            {
                MessageBox.Show("Login réussi");
                Profil_Client_Part Profil_Client = new Profil_Client_Part(userId);        //On prend en paramètre de création du Form l'Id saisi pour prélever les informations nécessaires
                Profil_Client.Show();

                this.Hide();
                Profil_Client.FormClosed += (s,args) => this.Close();
            }
            else if(LoginValide(userId,userpwd) == 1)   //<-- Client Entreprise
            {
                MessageBox.Show("Login réussi");
                Profil_Client_Ent Profil_Client = new Profil_Client_Ent(userId);        //On prend en paramètre de création du Form l'Id saisi pour prélever les informations nécessaires
                Profil_Client.Show();

                this.Hide();
                Profil_Client.FormClosed += (s,args) => this.Close();
            }
            else if(LoginValide(userId,userpwd) == 2)   //<-- Cuisinier
            {
                Profil_Cuisinier Profil_Cuisinier = new Profil_Cuisinier(userId);
                Profil_Cuisinier.Show();

                this.Hide();
                Profil_Cuisinier.FormClosed += (s,args) => this.Close();
            }
            else
            {
                MessageBox.Show("Identifiant erroné");
            }
        }

        private void Retour_Click(object? sender, EventArgs e)
        {
            PrésentationForm Form = new PrésentationForm();
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }

        private int LoginValide(string userId, string userpwd)
        {
            string connectionString = "SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root";
            string queryClient = $"SELECT Nom_particulier, Prenom_particulier, NULL AS SIRET_entreprise FROM client WHERE Id_client = @Id AND Mdp = @pwd;";
            string queryCuisinier = $"SELECT COUNT(*) FROM cuisinier WHERE Id_cuisinier = @Id AND Mdp = @pwd;";
            string queryEntreprise = $"SELECT NULL AS Nom_particulier, NULL AS Prenom_particulier, SIRET_entreprise FROM entreprise WHERE Id_client = @Id AND Mdp = @pwd;";
            string nom = "";    //Nom du Particulier trouvé dans la BDD
            string prenom = "";    //Preom du Particulier trouvé dans la BDD
            string siret = "";    //SIRET de l'Entreprise trouvé dans la BDD

            try
            {
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand commandClientPart = new MySqlCommand(queryClient, connection);     //Si c'est un Client Particulier
                    commandClientPart.Parameters.AddWithValue("@Id", userId);
                    commandClientPart.Parameters.AddWithValue("@pwd", userpwd);
                    
                    using(MySqlDataReader reader = commandClientPart.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            nom = reader["Nom_particulier"] as string ?? "";
                            prenom = reader["Prenom_particulier"] as string ?? "";
                        }
                    }
                    commandClientPart.Dispose();

                    MySqlCommand commandClientEnt = new MySqlCommand(queryClient, connection);     //Si c'est un Client Entreprise
                    commandClientEnt.Parameters.AddWithValue("@Id", userId);
                    commandClientEnt.Parameters.AddWithValue("@pwd", userpwd);
                    
                    using(MySqlDataReader reader = commandClientPart.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            siret = reader["SIRET_entreprise"] as string ?? "";
                        }
                    }
                    commandClientPart.Dispose();

                    MySqlCommand commandCuisinier = new MySqlCommand(queryCuisinier, connection);     //Va regarder dans la table Client et stocker le résultat dans resultClient
                    commandCuisinier.Parameters.AddWithValue("@Id", userId);
                    commandCuisinier.Parameters.AddWithValue("@pwd", userpwd);
                    int resultCuisinier = Convert.ToInt32(commandCuisinier.ExecuteScalar());
                    commandCuisinier.Dispose();

                    if (!string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(prenom)) // C'est un Client Particulier
                    {
                        return 0;
                    }
                    else if (!string.IsNullOrEmpty(siret)) // C'est un Client Entreprise
                    {
                        return 1;
                    }
                    else if (resultCuisinier > 0) // C'est un Cuisinier
                    {
                        return 2;
                    }
                    else
                    {
                        return -1; // Identifiant erroné
                    }
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Erreur de connexion à la base de donnée"+e.Message);
                return -1;
            }
        }
    }
}