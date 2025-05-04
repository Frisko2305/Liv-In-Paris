namespace Liv_In_Paris
{
    internal class Graphe<T>
    {
        

        private List<NoeudsStation<T>> noeuds;
        private List<LienStation<T>> liens;

        
            

        public List<LienStation<T>> Liens_Pte 
        {
            get { return liens; }
            set { this.liens = value; }
        }

        public List<NoeudsStation<T>> Noeuds_Pte
        {
            get { return noeuds; }
            set { this.noeuds =  value; }
        }

        public Graphe()
        {
            this.noeuds = new List<NoeudsStation<T>>();
            this.liens = new List<LienStation<T>>();
        }


        public void AjouterNoeud( NoeudsStation<T> noeud)
        {
            this.noeuds.Add(noeud);
        }

        public void AjouterLien (LienStation<T> lien)
        {
            this.liens.Add(lien);
        }

        public NoeudsStation<T> RechercherNoeud(int id)
        {
            foreach (var noeud in this.noeuds)
            {
                if (Convert.ToInt32(noeud.Id) == id)
                {
                    return noeud;
                }
            }
            throw new Exception("Noeud inexistant");
        }

        public int[,] CreerMatriceAdjacence()
        {
            int n = noeuds.Count;
            int[,] matrice = new int[n, n];

            // Initialiser la matrice avec des valeurs infinies (int.MaxValue) sauf sur la diagonale
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrice[i, j] = (i == j) ? 0 : int.MaxValue;
                }
            }

            // Remplir la matrice avec les poids des liens
            foreach (var lien in liens)
            {
                if (lien.Id_precedent != null && lien.Id_suivant != null)
                {
                    int i = noeuds.IndexOf(lien.Id_precedent);
                    int j = noeuds.IndexOf(lien.Id_suivant);
                    matrice[i, j] = lien.Poids;

                    // Si le graphe est non orienté, ajouter aussi l'inverse
                    matrice[j, i] = lien.Poids;
                }
            }

            return matrice;
        }

        public Dictionary<NoeudsStation<T>, int> Dijkstra(NoeudsStation<T> depart, List<NoeudsStation<T>> noeuds, Graphe<T> graphe)
        {
            var distances = new Dictionary<NoeudsStation<T>, int>();
            var precedents = new Dictionary<NoeudsStation<T>, NoeudsStation<T>>();
            var noeudsNonVisites = new HashSet<NoeudsStation<T>>(noeuds);

            // Initialiser les distances
            foreach (var noeud in noeuds)
            {
                distances[noeud] = int.MaxValue;
                precedents[noeud] = null;
            }
            distances[depart] = 0;

            while (noeudsNonVisites.Count > 0)
            {
                // Trouver le nœud avec la plus petite distance
                var noeudCourant = noeudsNonVisites.OrderBy(n => distances[n]).First();
                noeudsNonVisites.Remove(noeudCourant);

                foreach (var lien in liens)
                {
                    if (lien.Id_precedent == noeudCourant && lien.Id_suivant != null && noeudsNonVisites.Contains(lien.Id_suivant))
                    {
                        int distance = distances[noeudCourant] + lien.Poids;
                        if (distance < distances[lien.Id_suivant])
                        {
                            distances[lien.Id_suivant] = distance;
                            precedents[lien.Id_suivant] = noeudCourant;
                        }
                    }
                }
            }

            return distances;
        }

        public Dictionary<NoeudsStation<T>, int> BellmanFord(NoeudsStation<T> depart, List<NoeudsStation<T>> noeuds, List<LienStation<T>> liens)
        {
            var distances = new Dictionary<NoeudsStation<T>, int>();
            var precedents = new Dictionary<NoeudsStation<T>, NoeudsStation<T>>();

            // Initialiser les distances
            foreach (var noeud in noeuds)
            {
                distances[noeud] = int.MaxValue;
                precedents[noeud] = null;
            }
            distances[depart] = 0;

            // Relaxer les arêtes |V| - 1 fois
            for (int i = 0; i < noeuds.Count - 1; i++)
            {
                foreach (var lien in liens)
                {
                    if (lien.Id_precedent != null && lien.Id_suivant != null)
                    {
                        int distance = distances[lien.Id_precedent] + lien.Poids;
                        if (distance < distances[lien.Id_suivant])
                        {
                            distances[lien.Id_suivant] = distance;
                            precedents[lien.Id_suivant] = lien.Id_precedent;
                        }
                    }
                }
            }

            // Vérifier les cycles négatifs
            foreach (var lien in liens)
            {
                if (lien.Id_precedent != null && lien.Id_suivant != null)
                {
                    if (distances[lien.Id_precedent] + lien.Poids < distances[lien.Id_suivant])
                    {
                        throw new InvalidOperationException("Le graphe contient un cycle de poids négatif.");
                    }
                }
            }

            return distances;
        }

        public int[,] FloydWarshall(List<NoeudsStation<T>> noeuds, Graphe<T> graphe)
        {
            int n = noeuds.Count;
            int[,] distances = CreerMatriceAdjacence();

            // Appliquer l'algorithme de Floyd-Warshall
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
    }
}
