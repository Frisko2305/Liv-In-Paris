using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphe_maVersion
{
    internal class Graphe
    {
        private string[,] Graph_association;
        private int taille;

        private List<Noeuds> noeuds; 
        private List<Liens> liens;

        public Graphe(int sa_taille_association)
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
        }

    }
}
