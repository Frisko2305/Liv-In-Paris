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

            TableLayoutPanel layout = new TableLayoutPanel();           //avec un layout
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

            if(LoginValide(userId,userpwd))
            {
                MessageBox.Show("Login réussi");
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

        private bool LoginValide(string userId, string userpwd)
        {
            string connectionString = "SERVER=localhost;PORT=3306;" + "DATABASE= psi;" + "UID=root;PASSWORD=root";
            string queryClient = $"SELECT COUNT(*) FROM client WHERE Id_client = @Id AND Mdp = @pwd;";
            string queryCuisinier = $"SELECT COUNT(*) FROM cuisinier WHERE Id_cuisinier = @Id AND Mdp = @pwd;";

            try
            {
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand commandClient = new MySqlCommand(queryClient, connection);     //Va regarder dans la table Client et stocker le résultat dans resultClient
                    commandClient.Parameters.AddWithValue("@Id", userId);
                    commandClient.Parameters.AddWithValue("@pwd", userpwd);
                    int resultClient = Convert.ToInt32(commandClient.ExecuteScalar());
                    commandClient.Dispose();

                    MySqlCommand commandCuisinier = new MySqlCommand(queryCuisinier, connection);     //Va regarder dans la table Client et stocker le résultat dans resultClient
                    commandCuisinier.Parameters.AddWithValue("@Id", userId);
                    commandCuisinier.Parameters.AddWithValue("@pwd", userpwd);
                    int resultCuisinier = Convert.ToInt32(commandCuisinier.ExecuteScalar());
                    commandClient.Dispose();

                    if(resultClient > 0 || resultCuisinier > 0)     //S'il le trouve dans une des deux tables, alors le Login est réussi
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Erreur de Connexion à la base de donnée"+e.Message);
            
                return false;
            }
        }
    }
}