using OfficeOpenXml;

namespace Liv_In_Paris
{
    internal class Program
    {
        [STAThread]
        
        /// Appelle les méthode pour créer, afficher le graphe et créer l'interface
        static void Main(string[] args)
        {
            

            Graphe<double> graphe = new Graphe<double>();
            List<NoeudsStation<double>> noeuds = graphe.Noeuds_Pte;
            List<LienStation<double>> liens = graphe.Liens_Pte;

            PeuplementTable(graphe);

            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GraphVisualizer<double>.AfficherGraphe(graphe);

            NoeudsStation<double> noeudDepart = graphe.RechercherNoeud(2); 
            NoeudsStation<double> noeudDestination = graphe.RechercherNoeud(10); 


            FloydWarshallTest(graphe, noeudDepart, noeudDestination, noeuds);
            // BellmanFordTest(graphe, noeudDepart, noeudDestination, noeuds, liens);
            // FloydWarshallTest(graphe, noeudDepart, noeudDestination, noeuds);

            

            ApplicationConfiguration.Initialize();
            PrésentationForm mainForm = new PrésentationForm();
            Application.Run(mainForm);

        }


        /// <summary>
        /// Récupérer les données du fichier excel sous forme de matrice
        /// </summary>
        /// <param name="graphe"></param>
        static void PeuplementTable(Graphe<double> graphe)
        {
            
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            
            string fichiersExcel = Path.Combine(Directory.GetCurrentDirectory(), "MetroParis.xlsx");


            
            if (!File.Exists(fichiersExcel))
            {
                Console.WriteLine($"Le fichier spécifié n'existe pas : {fichiersExcel}");
                return; 
            }

            
            FileInfo fileInfo = new FileInfo(fichiersExcel);

            using (var package = new ExcelPackage(fileInfo))
            {
                
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
                            
                            if (feuille.Cells[i + 2, j + 1] != null) 
                            {
                                donnees[i, j] = feuille.Cells[i + 2, j + 1].Text.Trim();
                            }
                            else
                            {
                                donnees[i, j] = string.Empty; 
                            }
                        }
                    }
                    RemplirGraphe(donnees, graphe);
                }
            }
        }



        /// <summary>
        /// Remplir les 2 listes de la classe graphe à partir de la matrice de peuplement base en fonction du nombre de colonne de chaque feuille de l'excel
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="graphe"></param>
        static void RemplirGraphe(string[,] mat, Graphe<double> graphe)
        {
            if (mat.GetLength(1) > 6)
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
                    if (mat[i, 2] == "")    
                    {
                        continue;
                    }
                    else if (mat[i, 0] == "" && mat[i, 1] == "" && mat[i, 2] == "" && mat[i, 3] == "" && mat[i, 4] == "" && mat[i, 5] == "")   
                    {
                        continue;
                    }
                    else if (mat[i, 3] == "") 
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



        /// <summary>
        /// Création des liens lorsque l'on doit changer de ligne de métro à une station
        /// </summary>
        /// <param name="graphe"></param>
        static void LienParCorrespondance(Graphe<double> graphe)
        {
            Random random = new Random();
            for (int i = 0; i < graphe.Noeuds_Pte.Count; i++)
            {
                for (int j = i + 1; j < graphe.Noeuds_Pte.Count; j++)
                {
                    int corres = random.Next(1, 5);
                    if (graphe.Noeuds_Pte[i].Nom == graphe.Noeuds_Pte[j].Nom)
                    {
                        graphe.AjouterLien(new LienStation<double>(graphe.Noeuds_Pte[i].Id, graphe.Noeuds_Pte[i].Nom, graphe.Noeuds_Pte[j], null, corres));
                        graphe.AjouterLien(new LienStation<double>(graphe.Noeuds_Pte[j].Id, graphe.Noeuds_Pte[j].Nom, graphe.Noeuds_Pte[i], null, corres));
                    }
                }
            }
        }

        ///Methode de test des programmes de plus courts chemins
        /*static void DijkstraTest(Graphe<double> graphe, NoeudsStation<double> noeudDepart, NoeudsStation<double> noeudDestination, List<NoeudsStation<double>> noeuds)

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
        } */




        ///Créer la matrice d'adjacence
        static int[,] CreerMatriceAdjacence(int n, Graphe<double> graphe)
        {
            var liens = graphe.Liens_Pte;
            var noeuds = graphe.Noeuds_Pte;
            int[,] matrice = new int[n, n];

           
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrice[i, j] = (i == j) ? 0 : int.MaxValue;
                }
            }
          
            foreach (var lien in liens)
            {
                if (lien.Id_precedent != null && lien.Id_suivant != null)
                {
                    int i = noeuds.IndexOf(lien.Id_precedent);
                    int j = noeuds.IndexOf(graphe.RechercherNoeud(Convert.ToInt32(lien.Id)));
                    matrice[i, j] = lien.Poids;

                   
                    matrice[j, i] = lien.Poids;
                }
            }

            return matrice;
        }



        /// <summary>
        /// Trouver le chemin le plus court en utilisant l'algorithme de floyd Warshall
        /// </summary>
        /// <param name="graphe"></param>
        /// <returns></returns>
        static int[,] FloydWarshall(Graphe<double> graphe)
        {
            int n = graphe.Noeuds_Pte.Count;
            int[,] distances = CreerMatriceAdjacence(n, graphe);

            
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] != int.MaxValue && distances[k, j] != int.MaxValue)
                        {
                            distances[i, j] = Math.Min(distances[i, j], distances[i, k] + distances[k, j]);
                        }
                    }
                }
            }

            return distances;
        }



        /// <summary>
        /// Appeler la métode FloydWarshall
        /// </summary>
        /// <param name="graphe"></param>
        /// <param name="noeudDepart"></param>
        /// <param name="noeudDestination"></param>
        /// <param name="noeuds"></param>
        static void FloydWarshallTest(Graphe<double> graphe, NoeudsStation<double> noeudDepart, NoeudsStation<double> noeudDestination, List<NoeudsStation<double>> noeuds)
        {
            try
            {
                
                int[,] distances = FloydWarshall(graphe);

              
                int indexDepart = graphe.Noeuds_Pte.IndexOf(noeudDepart);
                int indexDestination = graphe.Noeuds_Pte.IndexOf(noeudDestination);

           
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
