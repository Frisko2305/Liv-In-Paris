using OfficeOpenXml;

namespace Liv_In_Paris
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            /// Création du graphe
            
            Graphe<double> graphe = new Graphe<double>();
            List<NoeudsStation<double>> noeuds = graphe.Noeuds_Pte;
            List<LienStation<double>> liens = graphe.Liens_Pte;

            PeuplementTable(graphe);

            /// Démarrer l'affichage du graphe
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GraphVisualizer<double>.AfficherGraphe(graphe);

            NoeudsStation<double> noeudDepart = graphe.RechercherNoeud(2); // Exemple de nœud de départ
            NoeudsStation<double> noeudDestination = graphe.RechercherNoeud(10); // Exemple de nœud de destination


            // DijkstraTest(graphe, noeudDepart, noeudDestination, noeuds);
            // BellmanFordTest(graphe, noeudDepart, noeudDestination, noeuds, liens);
            // FloydWarshallTest(graphe, noeudDepart, noeudDestination, noeuds);
            
            //Partie Interface
            /*
            ApplicationConfiguration.Initialize();
            PrésentationForm mainForm = new PrésentationForm();
            Application.Run(mainForm);
            */
        }



            static void PeuplementTable(Graphe<double> graphe)
            {
                // Enregistrer le fournisseur d'encodage pour éviter l'erreur IBM437
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                // Récupérer le fichier .xlsx à la racine du projet
                string fichiersExcel = Path.Combine(Directory.GetCurrentDirectory(), "MetroParis.xlsx");


                // Vérifier si le fichier existe
                if (!File.Exists(fichiersExcel))
                {
                    Console.WriteLine($"Le fichier spécifié n'existe pas : {fichiersExcel}");
                    return; // Sortir de la méthode ou gérer l'erreur comme nécessaire
                }

                // Créer un objet FileInfo
                FileInfo fileInfo = new FileInfo(fichiersExcel);

                using (var package = new ExcelPackage(fileInfo))
                {
                    // Parcourir toutes les feuilles
                    foreach (var feuille in package.Workbook.Worksheets)
                    {
                        int ligCount = feuille.Dimension.End.Row;
                        int colCount = feuille.Dimension.End.Column;

                        if (ligCount < 2)
                        {
                            Console.WriteLine($"La feuille {feuille.Name} ne contient pas de données.");
                            continue;
                        }

                        string[,] donnees = new string[ligCount - 1, colCount];
                        for (int i = 0; i < donnees.GetLength(0); i++)
                        {
                            for (int j = 0; j < donnees.GetLength(1); j++)
                            {
                                // Vérifier si la cellule existe avant d'y accéder
                                if (feuille.Cells[i + 2, j + 1] != null) // i + 2 pour ignorer l'en-tête
                                {
                                    donnees[i, j] = feuille.Cells[i + 2, j + 1].Text.Trim();
                                }
                                else
                                {
                                    donnees[i, j] = string.Empty; // Valeur par défaut si la cellule est vide
                                }
                            }
                        }
                        RemplirGraphe(donnees, graphe );
                    }
                }
            }

            static void RemplirGraphe(string[,] mat, Graphe<double> graphe)
            {
                if(mat.GetLength(1)> 6)
                {
                    for (int i = 0; i < mat.GetLength(0); i++)
                    {
                        if (mat[i, 0] == null || mat[i, 1] == null || mat[i, 2] == null || mat[i, 3] == null || mat[i, 4] == null || mat[i, 5] == null || mat[i, 6] == null)
                        {
                            continue;
                        }
                        else
                        {
                            graphe.AjouterNoeud(new NoeudsStation<double>(Convert.ToDouble(mat[i, 0]), mat[i, 1], mat[i, 2], Convert.ToDouble(mat[i, 3]), Convert.ToDouble(mat[i, 4]), mat[i, 5], Convert.ToInt32(mat[i, 6])));
                        }
                    }
                LienParCorrespondance(graphe);
                }
                else
                {
                    for (int i = 0; i < mat.GetLength(0); i++)
                    {
                        if (mat[i, 2] == "")    //quand l'id_precedant est vide
                        {
                            continue;
                        }
                        else if(mat[i, 0] == "" && mat[i, 1] == "" && mat[i,2] == "" && mat[i,3] == "" && mat[i, 4] == "" && mat[i, 5] == "")   //quand on est à la dernière ligne, que du vide
                        {
                            continue;
                        }
                        else if(mat[i,3] == "") //quand l'id_suivant est vide
                        {
                            graphe.AjouterLien(new LienStation<double>(Convert.ToDouble(mat[i, 0]), mat[i, 1], graphe.RechercherNoeud(Convert.ToInt32(mat[i, 2])), null, Convert.ToInt32(mat[i, 4])));
                        }
                        else
                        {
                            graphe.AjouterLien(new LienStation<double>(Convert.ToDouble(mat[i, 0]), mat[i, 1], graphe.RechercherNoeud(Convert.ToInt32(mat[i, 2])), graphe.RechercherNoeud(Convert.ToInt32(mat[i, 3])), Convert.ToInt32(mat[i, 4])));
                        }
                    }
                }
            }

        static void LienParCorrespondance(Graphe<double> graphe)
        {
            Random random = new Random();
            for(int i = 0 ; i < graphe.Noeuds_Pte.Count ; i++)
            {
                for(int j = i+1 ; j < graphe.Noeuds_Pte.Count ; j++)
                {
                    int corres = random.Next(1,5);
                    if(graphe.Noeuds_Pte[i].Nom == graphe.Noeuds_Pte[j].Nom)
                    {
                        graphe.AjouterLien(new LienStation<double>(graphe.Noeuds_Pte[i].Id, graphe.Noeuds_Pte[i].Nom, graphe.Noeuds_Pte[j], null, corres ));
                        graphe.AjouterLien(new LienStation<double>(graphe.Noeuds_Pte[j].Id, graphe.Noeuds_Pte[j].Nom, graphe.Noeuds_Pte[i], null, corres ));
                    }
                }
            }
        }

        ///Methode de test des programmes de plus courts chemins
        static void DijkstraTest(Graphe<double> graphe, NoeudsStation<double> noeudDepart, NoeudsStation<double> noeudDestination, List<NoeudsStation<double>> noeuds)

             {

            var distances = graphe.Dijkstra(noeudDepart, noeuds, graphe);

                 if (distances.ContainsKey(noeudDestination))
                 {
                     Console.WriteLine($"La distance minimale entre le nœud {noeudDepart.Id} et le nœud {noeudDestination.Id} est : {distances[noeudDestination]}");
                 }
                 else
                 {
                     Console.WriteLine("Il n'y a pas de chemin entre le nœud de départ et le nœud de destination.");
                 }
             }
        
        static void BellmanFordTest(Graphe<double> graphe, NoeudsStation<double> noeudDepart, NoeudsStation<double> noeudDestination, List<NoeudsStation<double>> noeuds, List<LienStation<double>> liens)
        {
            try
            {
                // Exécuter l'algorithme de Bellman-Ford
                var distances = graphe.BellmanFord(noeudDepart, noeuds, liens);

                // Afficher la distance minimale entre le nœud de départ et le nœud de destination
                if (distances.ContainsKey(noeudDestination))
                {
                    Console.WriteLine($"La distance minimale entre le nœud {noeudDepart.Id} et le nœud {noeudDestination.Id} est : {distances[noeudDestination]}");
                }
                else
                {
                    Console.WriteLine("Il n'y a pas de chemin entre le nœud de départ et le nœud de destination.");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }  

        static void FloydWarshallTest(Graphe<double> graphe, NoeudsStation<double> noeudDepart, NoeudsStation<double> noeudDestination, List<NoeudsStation<double>> noeuds)
        {
            try
            {
                // Exécuter l'algorithme de Floyd-Warshall
                int[,] distances = graphe.FloydWarshall(noeuds, graphe);

                // Obtenir les indices des nœuds de départ et de destination
                int indexDepart = graphe.Noeuds_Pte.IndexOf(noeudDepart);
                int indexDestination = graphe.Noeuds_Pte.IndexOf(noeudDestination);

                // Afficher la distance minimale entre le nœud de départ et le nœud de destination
                if (distances[indexDepart, indexDestination] != int.MaxValue)
                {
                    Console.WriteLine($"La distance minimale entre le nœud {noeudDepart.Id} et le nœud {noeudDestination.Id} est : {distances[indexDepart, indexDestination]}");
                }
                else
                {
                    Console.WriteLine("Il n'y a pas de chemin entre le nœud de départ et le nœud de destination.");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
