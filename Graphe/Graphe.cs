namespace Liv_In_Paris
{
    /// <summary>
    /// Classe générique représentant un graphe composé de noeuds et de liens.
    /// </summary>
    /// <typeparam name="T">Type générique pour les identifiants des noeuds et des liens.</typeparam>
    internal class Graphe<T>
    {
        /// <summary>
        /// Liste des noeuds dans le graphe.
        /// </summary>
        private List<NoeudsStation<T>> noeuds;

        /// <summary>
        /// Liste des liens entre les noeuds dans le graphe.
        /// </summary>
        private List<LienStation<T>> liens;

        /// <summary>
        /// Obtient ou définit la liste des liens dans le graphe.
        /// </summary>
        public List<LienStation<T>> Liens_Pte
        {
            get { return liens; }
            set { this.liens = value; }
        }

        /// <summary>
        /// Obtient ou définit la liste des noeuds dans le graphe.
        /// </summary>
        public List<NoeudsStation<T>> Noeuds_Pte
        {
            get { return noeuds; }
            set { this.noeuds = value; }
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Graphe{T}"/>.
        /// </summary>
        public Graphe()
        {
            this.noeuds = new List<NoeudsStation<T>>();
            this.liens = new List<LienStation<T>>();
        }

        /// <summary>
        /// Ajoute un noeud au graphe.
        /// </summary>
        /// <param name="noeud">Le noeud à ajouter.</param>
        public void AjouterNoeud(NoeudsStation<T> noeud)
        {
            this.noeuds.Add(noeud);
        }

        /// <summary>
        /// Ajoute un lien au graphe.
        /// </summary>
        /// <param name="lien">Le lien à ajouter.</param>
        public void AjouterLien(LienStation<T> lien)
        {
            this.liens.Add(lien);
        }

        /// <summary>
        /// Recherche un noeud dans le graphe par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du noeud à rechercher.</param>
        /// <returns>Le noeud trouvé.</returns>
        /// <exception cref="Exception">Lancée si le noeud n'existe pas.</exception>
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

        
        /// <summary>
        /// Implémente l'algorithme de Bellman-Ford pour trouver le plus court chemin depuis un noeud de départ.
        /// </summary>
        /// <param name="depart">Le noeud de départ.</param>
        /// <param name="noeuds">La liste des noeuds.</param>
        /// <param name="liens">La liste des liens.</param>
        /// <returns>Un dictionnaire des distances minimales depuis le noeud de départ.</returns>
        /// <exception cref="InvalidOperationException">Lancée si le graphe contient un cycle de poids négatif.</exception>
        public Dictionary<NoeudsStation<T>, int> BellmanFord(NoeudsStation<T> depart, List<NoeudsStation<T>> noeuds, List<LienStation<T>> liens)
        {
            var distances = new Dictionary<NoeudsStation<T>, int>();
            var precedents = new Dictionary<NoeudsStation<T>, NoeudsStation<T>>();

            foreach (var noeud in noeuds)
            {
                distances[noeud] = int.MaxValue;
                precedents[noeud] = null;
            }
            distances[depart] = 0;

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

        /// <summary>
        /// Implémente l'algorithme de Floyd-Warshall pour trouver les plus courts chemins entre toutes les paires de noeuds.
        /// </summary>
        /// <param name="noeuds">La liste des noeuds.</param>
        /// <param name="graphe">Le graphe contenant les liens.</param>
        /// <returns>Une matrice des distances minimales entre les noeuds.</returns>
        public int[,] FloydWarshall(List<NoeudsStation<T>> noeuds, Graphe<double> graphe)
        {
            int n = graphe.noeuds.Count;
            int[,] distances = Program.CreerMatriceAdjacence(n, graphe);

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