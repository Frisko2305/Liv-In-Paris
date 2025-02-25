using System.Windows.Forms;


namespace Graphe_maVersion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Création du graphe
            Graphe graphe = new Graphe();

            // Chargement des données depuis le fichier
            graphe.ChargerDepuisFichier();

            // Positionner les nœuds en cercle
            graphe.PlacerNoeudsEnCercle(800, 800);

            // Démarrer l'affichage du graphe
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GraphVisualizer.AfficherGraphe(graphe);
        }
    }
}
