using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Liv_In_Paris
{
    internal class Graphe
    {
        //private string[,] Graph_association;
        //private int taille;

        private List<Noeuds> noeuds;
        private List<Liens> liens;

        public List<Liens> Liens_Pte 
        {
            get { return liens; }
            set { this.liens = value; }
        }

        public List<Noeuds> Noeuds_Pte
        {
            get { return noeuds; }
            set { this.noeuds = value; }
        }

        public Graphe()
        {
            this.noeuds = new List<Noeuds>();
            this.liens = new List<Liens>();
        }


        public void ChargerDepuisFichier()
        {
            string nomfichier = "soc-karate.mtx";

            // Vérifier que le fichier existe
            if (!File.Exists(nomfichier))
            {
                Console.WriteLine("Erreur : Le fichier " + nomfichier + " est introuvable.");
                return;
            }

            string[] lignes = File.ReadAllLines(nomfichier);
            var noeudsDict = new Dictionary<int, Noeuds>();

            foreach (var ligne in lignes)
            {
                // Ignorer les lignes vides ou qui commencent par '%'
                if (string.IsNullOrWhiteSpace(ligne) || ligne.StartsWith("%"))
                    continue;

                var parts = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Vérifier que la ligne contient bien deux éléments
                if (parts.Length < 2)
                    continue;

                int n1, n2;

                // Vérifier que les éléments sont bien des nombres
                if (!int.TryParse(parts[0], out n1) || !int.TryParse(parts[1], out n2))
                    continue;  // Ignorer la ligne si ce n'est pas le cas

                // Ajouter les nœuds s'ils ne sont pas déjà présents
                if (!noeudsDict.ContainsKey(n1))
                    noeudsDict[n1] = new Noeuds(n1);

                if (!noeudsDict.ContainsKey(n2))
                    noeudsDict[n2] = new Noeuds(n2);

                // Ajouter le lien
                liens.Add(new Liens(noeudsDict[n1], noeudsDict[n2]));
            }

            // Ajouter les nœuds à la liste
            noeuds.AddRange(noeudsDict.Values);
            noeuds.Sort((n1, n2) => n1.Numero.CompareTo(n2.Numero));
        }

        public void PlacerNoeudsEnCercle(int largeur, int hauteur)
        {
            int rayon = Math.Min(largeur, hauteur) / 3;
            int centreX = largeur / 2;
            int centreY = hauteur / 2;
            int totalNoeuds = noeuds.Count;

            for (int i = 0; i < totalNoeuds; i++)
            {
                double angle = 2 * Math.PI * i / totalNoeuds;
                noeuds[i].X = centreX + rayon * Math.Cos(angle);
                noeuds[i].Y = centreY + rayon * Math.Sin(angle);
            }
        }

        public int[,] CreerMatriceAdjacence()
        {
            int n = noeuds.Count;int[,] matrice = new int[n, n];
            var indexNoeud = new Dictionary<Noeuds, int>();
            
            for (int i = 0; i < n; i++)
            {
                indexNoeud[noeuds[i]] = i;
            }

            foreach (var lien in liens)
            {
                int i = indexNoeud[lien.Membre];
                int j = indexNoeud[lien.MembreAutre];
                matrice[i, j] = 1;
                matrice[j, i] = 1;
            }

            return matrice;
        }

        public int ParcoursBFS(Noeuds depart)
        {
            var visite = new HashSet<Noeuds>();
            var queue = new Queue<Noeuds>();
            int check_connexité = 0;
            queue.Enqueue(depart);
            visite.Add(depart);

            int[,] matriceAdjacence = CreerMatriceAdjacence();
            int indexDepart = noeuds.IndexOf(depart);

            while (queue.Count > 0)
            {
                var noeud = queue.Dequeue();
                Console.Write(noeud.Numero + " ");
                check_connexité ++;

                int indexNoeud = noeuds.IndexOf(noeud);
                for (int i = 0; i < matriceAdjacence.GetLength(0); i++)
                {
                    if (matriceAdjacence[indexNoeud, i] == 1 && !visite.Contains(noeuds[i]))
                    {
                        visite.Add(noeuds[i]);
                        queue.Enqueue(noeuds[i]);
                    }
                }
            }
            Console.WriteLine();
            return check_connexité;
        }

        public void ParcoursDFS(Noeuds depart)
        {
            var visite = new HashSet<Noeuds>();
            var stack = new Stack<Noeuds>();
            stack.Push(depart);

            int[,] matriceAdjacence = CreerMatriceAdjacence();
            int indexDepart = noeuds.IndexOf(depart);

            while (stack.Count > 0)
            {
                var noeud = stack.Pop();
                if (!visite.Contains(noeud))
                {
                    visite.Add(noeud);
                    Console.Write(noeud.Numero + " ");

                    int indexNoeud = noeuds.IndexOf(noeud);
                    for (int i = 0; i < matriceAdjacence.GetLength(0); i++)
                    {
                        if (matriceAdjacence[indexNoeud, i] == 1 && !visite.Contains(noeuds[i]))
                        {
                            stack.Push(noeuds[i]);
                        }
                    }
                }
            }
            Console.WriteLine();
        }


        #region /Premiere version

        /*public Graphe(int sa_taille_association)
         {
             this.noeuds = new List<Noeuds>();
             this.liens = new List<Liens>();
             this.taille = sa_taille_association;

             this.Graph_association = new  string[taille, taille];

             for (int i = 0; i < taille; i++)
             {
                 for (int j = 0; j < taille; j++)
                 {
                     Graph_association[i, j] = " ";
                 }
             }
         }

         public void AjouterNoeud(Noeuds n1)
         {
             this.noeuds.Add(n1);
             Graph_association[n1.Localisation[0], n1.Localisation[1]] = $"N{n1.Numero}";
         }

         public void AjouterLien(Noeuds n1, Noeuds n2)
         {
             if (!noeuds.Contains(n1) || !noeuds.Contains(n2))
             {
                 Console.WriteLine("Erreur : Les deux nœuds doivent être présents dans le graphe.");
             }
             else
             {
                 this.liens.Add(new Liens(n1, n2));

                 int x1 = n1.Localisation[0];
                 int y1 = n1.Localisation[1];
                 int x2 = n2.Localisation[0];
                 int y2 = n2.Localisation[1];

                 if (x1 == x2) // Lien horizontal
                 {
                     Graph_association[x1, Math.Min(y1, y2) + 1] = "-";
                 }
                 else if (y1 == y2) // Lien vertical
                 {
                     Graph_association[Math.Min(x1, x2) + 1, y1] = "|";
                 }
             }         
         }

         public void AfficherMatrice()
         {
             Console.WriteLine("Représentation du graphe :");
             for (int i = 0; i < Graph_association.GetLength(0); i++)
             {
                 for (int j = 0; j < Graph_association.GetLength(1); j++)
                 {
                     Console.Write(Graph_association[i, j]);
                 }
                 Console.WriteLine();
             }
         }*/
        #endregion
    }
}
