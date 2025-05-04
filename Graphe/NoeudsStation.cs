
namespace Liv_In_Paris
{
    class NoeudsStation<T>
    {
        private T id;
        private string libelle;
        private string nom;
        private T longitude;
        private T latitude;
        private string communenom;
        private int codeInsee;


        public T Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Libelle
        {
            get { return libelle; }
            set { libelle = value; }
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public T Longitude
        {
            get { return longitude; }
            set { longitude = value; }

        }


        public T Latitude
        {
            get { return latitude; }
            set { latitude = value; }

        }

        public string Communenom
        {
            get { return communenom; }
            set { communenom = value; }
        }

        public int CodeInsee
        {
            get { return codeInsee; }
            set { codeInsee = value; }
        }

        public NoeudsStation(T id, string libelle, string nom, T longitude, T latitude,   string communenom, int codeInsee)
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
