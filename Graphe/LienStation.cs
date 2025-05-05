namespace Liv_In_Paris
{
    /// <summary>
    /// Classe générique représentant un lien entre des stations avec divers attributs.
    /// </summary>
    /// <typeparam name="T">Type générique pour certains attributs comme l'ID.</typeparam>
    class LienStation<T>
    {
        /// <summary>
        /// Identifiant du lien entre les stations.
        /// </summary>
        private T id;

        /// <summary>
        /// Nom du lien entre les stations.
        /// </summary>
        private string nom;

        /// <summary>
        /// Noeud de la station précédente dans le lien.
        /// </summary>
        private NoeudsStation<T> id_precedent;

        /// <summary>
        /// Noeud de la station suivante dans le lien. Peut être null.
        /// </summary>
        private NoeudsStation<T>? id_suivant;

        /// <summary>
        /// Poids du lien entre les stations.
        /// </summary>
        private int poids;

        /// <summary>
        /// Obtient ou définit l'identifiant du lien entre les stations.
        /// </summary>
        public T Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Obtient ou définit le nom du lien entre les stations.
        /// </summary>
        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        /// <summary>
        /// Obtient ou définit le noeud de la station précédente dans le lien.
        /// </summary>
        public NoeudsStation<T> Id_precedent
        {
            get { return id_precedent; }
            set { id_precedent = value; }
        }

        /// <summary>
        /// Obtient ou définit le noeud de la station suivante dans le lien. Peut être null.
        /// </summary>
        public NoeudsStation<T>? Id_suivant
        {
            get { return id_suivant; }
            set { id_suivant = value; }
        }

        /// <summary>
        /// Obtient ou définit le poids du lien entre les stations.
        /// </summary>
        public int Poids
        {
            get { return poids; }
            set { poids = value; }
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="LienStation{T}"/>.
        /// </summary>
        /// <param name="id">Identifiant du lien entre les stations.</param>
        /// <param name="nom">Nom du lien entre les stations.</param>
        /// <param name="id_precedent">Noeud de la station précédente dans le lien.</param>
        /// <param name="id_suivant">Noeud de la station suivante dans le lien. Peut être null.</param>
        /// <param name="poids">Poids du lien entre les stations. La valeur par défaut est 1.</param>
        public LienStation(T id, string nom, NoeudsStation<T> id_precedent, NoeudsStation<T> id_suivant, int poids = 1)
        {
            this.id = id;
            this.nom = nom;
            this.id_precedent = id_precedent;
            this.id_suivant = id_suivant;
            this.poids = poids;
        }
    }
}