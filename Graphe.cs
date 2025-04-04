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
        /// private string[,] Graph_association;
        /// private int taille;

        private List<NoeudsStation> noeuds;
        private List<LienStation> liens;


            

        public List<LienStation> Liens_Pte 
        {
            get { return liens; }
            set { this.liens = value; }
        }

        public List<NoeudsStation> Noeuds_Pte
        {
            get { return noeuds; }
            set { this.noeuds =  value; }
        }

        public Graphe()
        {
            this.noeuds = new List<NoeudsStation>();
            this.liens = new List<LienStation>();
        }


        




        

        /*public int ParcoursBFS(NoeudsStation depart)
        {
            var visite = new HashSet<NoeudsStation>();
            var queue = new Queue<NoeudsStation>();
            int check_connexité = 0;
            queue.Enqueue(depart);
            visite.Add(depart);

            int[,] matriceAdjacence = CreerMatriceAdjacence();
            int indexDepart = noeuds.IndexOf(depart);

            while (queue.Count > 0)
            {
                var noeud = queue.Dequeue();
                Console.Write(noeud.Id + " ");
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

              public void ParcoursDFS(NoeudsStation depart)
        {
            var visite = new HashSet<NoeudsStation>();
            var stack = new Stack<NoeudsStation>();
            var indexNoeud = new Dictionary<NoeudsStation, int>();

            /// Préparer l'indexation pour un accès rapide aux indices des nœuds
            for (int i = 0; i < noeuds.Count; i++)
            {
                indexNoeud[noeuds[i]] = i;
            }

            int[,] matriceAdjacence = CreerMatriceAdjacence();

            stack.Push(depart);

            while (stack.Count > 0)
            {
                var noeud = stack.Pop();
                if (!visite.Contains(noeud))
                {
                    visite.Add(noeud);
                    Console.Write(noeud.Id + " ");

                    int indexNoeudCourant = indexNoeud[noeud];

                    /// Ajouter les voisins en ordre inverse pour un meilleur comportement avec la pile
                    for (int i = matriceAdjacence.GetLength(0) - 1; i >= 0; i--)
                    {
                        if (matriceAdjacence[indexNoeudCourant, i] == 1 && !visite.Contains(noeuds[i]))
                        {
                            stack.Push(noeuds[i]);
                        }
                    }
                }
            }
            Console.WriteLine();
        }*/

        ///BONUS

        /// Méthode pour obtenir l'ordre du graphe
        public int ObtenirOrdre()
        {
            return noeuds.Count;
        }


        /// Méthode pour obtenir la taille du graphe
        public int ObtenirTaille()
        {
            return liens.Count;
        }

        public bool EstOriente()
        {
            /// non orienté
            return false;
        }

        public void AfficherProprietes()
        {
            Console.WriteLine($"Ordre du graphe : {ObtenirOrdre()}");
            Console.WriteLine($"Taille du graphe : {ObtenirTaille()}");
            Console.WriteLine($"Le graphe est {(EstOriente() ? "orienté" : "non orienté")}");
        }


        public void AjouterNoeud( NoeudsStation noeud)
        {
            this.noeuds.Add(noeud);
        }

        public void AjouterLien (LienStation lien)
        {
            this.liens.Add(lien);
        }

        public NoeudsStation RechercherNoeud(int id)
        {
            foreach(var noeud in this.noeuds)
            {
                if(noeud.Id ==id)
                {
                    return noeud;
                }

            }
            return null;
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

                 if (x1 == x2) /// Lien horizontal
                 {
                     Graph_association[x1, Math.Min(y1, y2) + 1] = "-";
                 }
                 else if (y1 == y2)  Lien vertical
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
