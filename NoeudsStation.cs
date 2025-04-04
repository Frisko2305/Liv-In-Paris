using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class NoeudsStation
    {
        private int id;
        private int libelle;
        private string nom;
        private double longitude;
        private double latitude;
        private string communenom;
        private int codeInsee;
        private int z;


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Libelle
        {
            get { return libelle; }
            set { libelle = value; }
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }

        }


        public double Latitude
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

        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        public NoeudsStation(int son_id, int libelle, string nom, double longitude, double latitude,   string communenom, int codeInsee, int z)
        {
            this.id = son_id;
            this.libelle = libelle;
            this.nom = nom;
            this.longitude = longitude;
            this.latitude = latitude;
            this.z = z;
            this.communenom = communenom;
            this.codeInsee = codeInsee;
        }


    }
}
