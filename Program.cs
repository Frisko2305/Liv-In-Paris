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

            /// Pour vérifier la connexité du graphe, on a qu'a vérifié si la liste des noeuds visités (en DFS ou BFS) et de la même longueur que la liste des noeuds du graphe

            // Effectuer un parcours en largeur (BFS)
            Console.WriteLine("\nParcours en largeur (BFS) à partir du nœud " + noeudDepart.Numero + ":");
            int connexite = graphe.ParcoursBFS(noeudDepart);

            // Effectuer un parcours en profondeur (DFS)
            Console.WriteLine("\nParcours en profondeur (DFS) à partir du nœud " + noeudDepart.Numero + ":");
            graphe.ParcoursDFS(noeudDepart);

            if(connexite == graphe.Noeuds_Pte.Count)
            {
                Console.WriteLine("Ce graphe est connexe !");
            }
        }
    }
}
