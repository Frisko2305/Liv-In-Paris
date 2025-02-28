using OfficeOpenXml;
using System;
using System.IO;
namespace espace_travail
{
    class Program
    {
        static void Main()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string fileClient = "";
            string fileCuisinier = "";
            string filePlats = "";
            string fileEntreprise = "";
            string fileCommande = "";
            string fileLivraison = "";
            string fileAvis = "";
            

            List<string> files = new List<string> ();

            string[] att_fileClient = { };
            string[] att_fileCuisinier = { };
            string[] att_filePlats = { };
            string[] att_fileEntreprise = { };
            string[] att_fileCommande = { };
            string[] att_Livraison = { };
            string[] att_fileAvis_Client = { };
            string[] att_fileAvis_Cuisiniers = { };

            List<string[]> attribute = new List<string[]> ();

            var fileInfo = new FileInfo("Clients_test.xlsx");

            using (var package = new ExcelPackage(fileInfo))
            {
                // Sélectionner la première feuille
                var worksheet = package.Workbook.Worksheets[1];
                var rowCount = worksheet.Dimension.End.Row;  // Nombre de lignes
                var colCount = worksheet.Dimension.End.Column;  // Nombre de colonnes

                // Nom de la table
                string tableName = "client_paris";

                // Attributs de la table dans l'ordre attendu
                string[] attributes = new string[]
                {
                    "numC", "nom", "prenom", "rue", "numRue", "CP", 
                    "ville", "noTel", "email", "Metro_proche"
                };

                // Ouvrir un flux pour écrire le script SQL
                using (var writer = new StreamWriter("script.sql"))
                {
                    // Parcourir les lignes et créer des commandes INSERT
                    for (int row = 2; row <= rowCount; row++)  // On commence à la ligne 2 pour ignorer l'en-tête
                    {
                        string values = "";
                    
                        // Parcourir les colonnes et récupérer les valeurs
                        for (int col = 1; col <= colCount; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Text.Replace("'", "''");  // Gérer les apostrophes
                            values += $"'{cellValue}', ";
                        }

                        // Supprimer la dernière virgule et ajouter la commande SQL
                        values = values.TrimEnd(',', ' ');
                        string sql = $"INSERT INTO {tableName} ({string.Join(", ", attributes)}) VALUES ({values});";
                        writer.WriteLine(sql);
                    }
                }
            }

            Console.WriteLine("Script SQL généré avec succès !");
        }
    }
}
