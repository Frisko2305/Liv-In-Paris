using System.Windows.Forms;


namespace Liv_In_Paris
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

            Console.WriteLine("Nœuds du graphe :");
            foreach (var noeud in graphe.Noeuds_Pte)
            {
                Console.Write(noeud.Numero + " ");
            }
            
            Console.WriteLine();
            
            // Choisir un nœud de départ pour les parcours
            // Supposons que nous voulons commencer par le premier nœud
            
            Noeuds noeudDepart = graphe.Noeuds_Pte[33];

            // Effectuer un parcours en largeur (BFS)
            Console.WriteLine("\nParcours en largeur (BFS) à partir du nœud " + noeudDepart.Numero + ":");
            graphe.ParcoursBFS(noeudDepart);

            // Effectuer un parcours en profondeur (DFS)
            Console.WriteLine("\nParcours en profondeur (DFS) à partir du nœud " + noeudDepart.Numero + ":");
            graphe.ParcoursDFS(noeudDepart);

            // Attendre que l'utilisateur appuie sur une touche avant de fermer
            Console.WriteLine("Appuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}
