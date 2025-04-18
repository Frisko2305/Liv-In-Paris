using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class NoeudsStation<T>
    {
        private int id;
        private string libelle;
        private string nom;
        private T longitude;
        private T latitude;
        private string communenom;
        private int codeInsee;


        public int Id
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

        public NoeudsStation(int son_id, string libelle, string nom, T longitude, T latitude,   string communenom, int codeInsee)
        {
            this.id = son_id;
            this.libelle = libelle;
            this.nom = nom;
            this.longitude = longitude;
            this.latitude = latitude;
            this.communenom = communenom;
            this.codeInsee = codeInsee;
        }


    }
}
