using OfficeOpenXml;

namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// Création du graphe
            Graphe graphe = new Graphe();

            /// Chargement des données depuis le fichier
            graphe.ChargerDepuisFichier();

            /// Positionner les nœuds en cercle
            graphe.PlacerNoeudsEnCercle(800, 800);

            /// Démarrer l'affichage du graphe
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GraphVisualizer.AfficherGraphe(graphe);

            Console.WriteLine("Nœuds du graphe :");
            foreach (var noeud in graphe.Noeuds_Pte)
            {
                Console.Write(noeud.Numero + " ");
            }
            
            Console.WriteLine();
            
            /// Choisir un nœud de départ pour les parcours
            /// Supposons que nous voulons commencer par le premier nœud
            
            Noeuds noeudDepart = graphe.Noeuds_Pte[33];

            /// Pour vérifier la connexité du graphe, on a qu'a vérifié si la liste des noeuds visités (en DFS ou BFS) et de la même longueur que la liste des noeuds du graphe

            /// Effectuer un parcours en largeur (BFS)
            Console.WriteLine("\nParcours en largeur (BFS) à partir du nœud " + noeudDepart.Numero + ":");
            int connexite = graphe.ParcoursBFS(noeudDepart);

            /// Effectuer un parcours en profondeur (DFS)
            Console.WriteLine("\nParcours en profondeur (DFS) à partir du nœud " + noeudDepart.Numero + ":");
            graphe.ParcoursDFS(noeudDepart);

            if(connexite == graphe.Noeuds_Pte.Count)
            {
                Console.WriteLine("Ce graphe est connexe !");
            }

            Console.WriteLine("\n\nMaintenant nous allons créer les scripts SQL des tables à peupler dans la BDD");
            PeuplementTable();
        }

            static void PeuplementTable()
            {
                // Enregistrer le fournisseur d'encodage pour éviter l'erreur IBM437
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                // Dossier contenant les fichiers Excel
                string dossierExcel = @"dossierEXCEL";
                string dossierSQL = @"dossierSQL";

                // Vérifier si le dossier existe
                if (!Directory.Exists(dossierExcel))
                {
                    Console.WriteLine("Le dossier Excel spécifié n'existe pas.");
                    return;
                }

                // Créer le dossier SQL s'il n'existe pas
                if (!Directory.Exists(dossierSQL))
                {
                    Directory.CreateDirectory(dossierSQL);
                }

                // Récupérer tous les fichiers .xlsx dans le dossier
                string[] fichiersExcel = Directory.GetFiles(dossierExcel, "*.xlsx");

                if (fichiersExcel.Length == 0)
                {
                    Console.WriteLine("Aucun fichier Excel trouvé dans le dossier.");
                    return;
                }

                // Traiter chaque fichier Excel
                foreach (var fichierExcel in fichiersExcel)
                {
                    FileInfo fileInfo = new FileInfo(fichierExcel);
                    string nomFichierSansExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    string cheminFichierSQL = Path.Combine(dossierSQL, $"{nomFichierSansExtension}.sql");

                    using (var package = new ExcelPackage(fileInfo))
                    {
                        // Vérifier la présence d'au moins une feuille
                        if (package.Workbook.Worksheets.Count == 0)
                        {
                            Console.WriteLine($"Le fichier {fileInfo.Name} ne contient aucune feuille.");
                            continue;
                        }

                        // Sélectionner la première feuille
                        var worksheet = package.Workbook.Worksheets[1];
                        int rowCount = worksheet.Dimension.End.Row;
                        int colCount = worksheet.Dimension.End.Column;

                        if (rowCount < 2)
                        {
                            Console.WriteLine($"Le fichier {fileInfo.Name} ne contient pas de données.");
                            continue;
                        }

                        // Lire les noms des colonnes (ligne 1)
                        string[] attributes = new string[colCount];
                        for (int col = 1; col <= colCount; col++)
                        {
                            attributes[col - 1] = worksheet.Cells[1, col].Text.Trim();
                        }

                        // Ouvrir un fichier pour écrire le script SQL
                        using (StreamWriter writer = new StreamWriter(cheminFichierSQL))
                        {
                            for (int row = 2; row <= rowCount; row++) // Commence à la ligne 2 pour ignorer les en-têtes
                            {
                                string values = "";

                                for (int col = 1; col <= colCount; col++)
                                {
                                    var cellValue = worksheet.Cells[row, col].Text.Trim(); // Récupérer la valeur et enlever les espaces inutiles

                                    // Vérifier si la cellule est vide pour mettre NULL
                                    if (string.IsNullOrEmpty(cellValue))
                                    {
                                        values += "NULL, ";
                                    }
                                    else
                                    {
                                        cellValue = cellValue.Replace("'", "''"); // Gérer les apostrophes
                                        values += $"'{cellValue}', ";
                                    }
                                }

                                values = values.TrimEnd(',', ' ');
                                string sql = $"INSERT INTO {nomFichierSansExtension} ({string.Join(", ", attributes)}) VALUES ({values});";
                                writer.WriteLine(sql);
                            }
                        }

                        Console.WriteLine($"Script SQL généré pour {fileInfo.Name} → {cheminFichierSQL}");
                    }
                }

                Console.WriteLine("Tous les fichiers ont été traités avec succès !");
            }
    }
}
