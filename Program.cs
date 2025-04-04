using OfficeOpenXml;

namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// Création du graphe
            Graphe graphe = new Graphe();



            /// Positionner les nœuds en cercle
 

            /// Démarrer l'affichage du graphe
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GraphVisualizer.AfficherGraphe(graphe);

            graphe.AfficherProprietes();

            Console.WriteLine("Nœuds du graphe :");
            foreach (var noeud in graphe.Noeuds_Pte)
            {
                Console.Write(noeud.Id + " ");
            }
            
            Console.WriteLine();
            
            /// Choisir un nœud de départ pour les parcours
            /// Supposons que nous voulons commencer par le premier nœud
            
            NoeudsStation noeudDepart = graphe.Noeuds_Pte[33];

            /// Pour vérifier la connexité du graphe, on a qu'a vérifié si la liste des noeuds visités (en DFS ou BFS) et de la même longueur que la liste des noeuds du graphe

            /// Effectuer un parcours en largeur (BFS)
            Console.WriteLine("\nParcours en largeur (BFS) à partir du nœud " + noeudDepart.Id + ":");
            int connexite = graphe.ParcoursBFS(noeudDepart);

            /// Effectuer un parcours en profondeur (DFS)
            Console.WriteLine("\nParcours en profondeur (DFS) à partir du nœud " + noeudDepart.Id + ":");
            graphe.ParcoursDFS(noeudDepart);

            if(connexite == graphe.Noeuds_Pte.Count)
            {
                Console.WriteLine("\nCe graphe est connexe !");
            }

            Console.WriteLine("\n\nMaintenant nous allons créer les scripts SQL des tables à peupler dans la BDD");
            PeuplementTable(graphe);
        }



            static void PeuplementTable(Graphe graphe)
            {
                // Enregistrer le fournisseur d'encodage pour éviter l'erreur IBM437
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                // Dossier contenant les fichiers Excel
                string dossierExcel = @"dossierEXCEL";


                // Vérifier si le dossier existe
                if (!Directory.Exists(dossierExcel))
                {
                    Console.WriteLine("Le dossier Excel spécifié n'existe pas.");
                    return;
                }



                // Récupérer tous les fichiers .xlsx dans le dossier
                var fichiersExcel = Directory.GetFiles(dossierExcel, "*.xlsx");

                if (fichiersExcel.Length == 0)
                {
                    Console.WriteLine("Aucun fichier Excel trouvé dans le dossier.");
                    return; // Sortir de la méthode si aucun fichier n'est trouvé
                }

                // Récupérer le premier fichier Excel
                string fichierExcel = fichiersExcel[0];

                // Vérifier si le chemin du fichier est valide
                if (string.IsNullOrEmpty(fichierExcel))
                {
                    Console.WriteLine("Le chemin du fichier est vide ou nul.");
                    return; // Sortir de la méthode ou gérer l'erreur comme nécessaire
                }

                // Vérifier si le fichier existe
                if (!File.Exists(fichierExcel))
                {
                    Console.WriteLine($"Le fichier spécifié n'existe pas : {fichierExcel}");
                    return; // Sortir de la méthode ou gérer l'erreur comme nécessaire
                }

                // Créer un objet FileInfo
                FileInfo fileInfo = new FileInfo(fichierExcel);
                Console.WriteLine($"Traitement du fichier : {fileInfo.Name}");

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
                        for(int i=0; i<donnees.GetLength(0); i++)
                        {
                        
                            for(int j=0; j<donnees.GetLength(1); j++)
                            {
                                donnees[i, j] = feuille.Cells[i + 1, j].Text.Trim();
                            }
                        }

                    RemplirGraphe(donnees, graphe);
                    }
                }


            }

            static void RemplirGraphe(string[,] mat, Graphe graphe)
            {
                if(mat.GetLength(1)>5)
                {
                    for (int i = 0; i < mat.GetLength(0); i++)
                    {
                        if (mat[i, 1] == null || mat[i, 2] == null || mat[i, 3] == null || mat[i, 4] == null || mat[i, 5] == null || mat[i, 6] == null || mat[i, 7] == null)
                        {
                            continue;
                        }
                        else
                        {

                            graphe.AjouterNoeud(new NoeudsStation(Convert.ToInt32(mat[i, 1]), Convert.ToInt32(mat[i, 2]), mat[i, 3], Convert.ToDouble(mat[i, 4]), Convert.ToDouble(mat[i, 5]), mat[i, 6], Convert.ToInt32(mat[i, 7]), ExistenceStation(mat[i, 3], graphe)));
                        }

                    }
                }
                else
                {
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    if (mat[i, 1] == null || mat[i, 2] == null || mat[i, 3] == null || mat[i, 4] == null || mat[i, 5] == null )
                    {
                        continue;
                    }
                    else
                    {
                        
                        graphe.AjouterLien(new LienStation(Convert.ToInt32(mat[i, 1]), mat[i, 2], graphe.RechercherNoeud(Convert.ToInt32(mat[i, 3])), graphe.RechercherNoeud(Convert.ToInt32(mat[i, 4])), Convert.ToInt32(mat[i, 5])));
                    }

                }
            }
                
            }



        static int ExistenceStation(string nom, Graphe graphe)
        {
            int compteur = 0;
            if(graphe.Noeuds_Pte!=null)
            foreach(NoeudsStation noeud in graphe.Noeuds_Pte)
            {
                if(noeud.Nom==nom)
                {
                    compteur++;
                }
            }
            return compteur;
        }
        
    }
}
