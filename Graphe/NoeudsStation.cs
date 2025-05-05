
namespace Liv_In_Paris
{
    /// <summary>
/// Classe générique représentant un noeud de station avec divers attributs.
/// </summary>
/// <typeparam name="T">Type générique pour certains attributs comme l'ID, la longitude et la latitude.</typeparam>
class NoeudsStation<T>
{
    /// <summary>
    /// Identifiant du noeud de station.
    /// </summary>
    private T id;

    /// <summary>
    /// Libellé du noeud de station.
    /// </summary>
    private string libelle;

    /// <summary>
    /// Nom du noeud de station.
    /// </summary>
    private string nom;

    /// <summary>
    /// Longitude du noeud de station.
    /// </summary>
    private T longitude;

    /// <summary>
    /// Latitude du noeud de station.
    /// </summary>
    private T latitude;

    /// <summary>
    /// Nom de la commune où se trouve le noeud de station.
    /// </summary>
    private string communenom;

    /// <summary>
    /// Code INSEE de la commune.
    /// </summary>
    private int codeInsee;

    /// <summary>
    /// Obtient ou définit l'identifiant du noeud de station.
    /// </summary>
    public T Id
    {
        get { return id; }
        set { id = value; }
    }

    /// <summary>
    /// Obtient ou définit le libellé du noeud de station.
    /// </summary>
    public string Libelle
    {
        get { return libelle; }
        set { libelle = value; }
    }

    /// <summary>
    /// Obtient ou définit le nom du noeud de station.
    /// </summary>
    public string Nom
    {
        get { return nom; }
        set { nom = value; }
    }

    /// <summary>
    /// Obtient ou définit la longitude du noeud de station.
    /// </summary>
    public T Longitude
    {
        get { return longitude; }
        set { longitude = value; }
    }

    /// <summary>
    /// Obtient ou définit la latitude du noeud de station.
    /// </summary>
    public T Latitude
    {
        get { return latitude; }
        set { latitude = value; }
    }

    /// <summary>
    /// Obtient ou définit le nom de la commune où se trouve le noeud de station.
    /// </summary>
    public string Communenom
    {
        get { return communenom; }
        set { communenom = value; }
    }

    /// <summary>
    /// Obtient ou définit le code INSEE de la commune.
    /// </summary>
    public int CodeInsee
    {
        get { return codeInsee; }
        set { codeInsee = value; }
    }

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="NoeudsStation{T}"/>.
    /// </summary>
    /// <param name="id">Identifiant du noeud de station.</param>
    /// <param name="libelle">Libellé du noeud de station.</param>
    /// <param name="nom">Nom du noeud de station.</param>
    /// <param name="longitude">Longitude du noeud de station.</param>
    /// <param name="latitude">Latitude du noeud de station.</param>
    /// <param name="communenom">Nom de la commune où se trouve le noeud de station.</param>
    /// <param name="codeInsee">Code INSEE de la commune.</param>
    public NoeudsStation(T id, string libelle, string nom, T longitude, T latitude, string communenom, int codeInsee)
    {
        this.id = id;
        this.libelle = libelle;
        this.nom = nom;
        this.longitude = longitude;
        this.latitude = latitude;
        this.communenom = communenom;
        this.codeInsee = codeInsee;
    }
}
}