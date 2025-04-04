using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class LienStation<T>
    {

        private int id;
        private string nom;
        private NoeudsStation<T> id_precedent;
        private NoeudsStation<T>? id_suivant;
        private int poids;


        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        public string Nom
        {
            get { return nom; }
            set { nom = value ; }
        }

        public NoeudsStation<T> Id_precedent
        {
            get { return id_precedent; }
            set { id_precedent = value; }
        }

        public NoeudsStation<T>? Id_suivant
        {
            get { return id_suivant; }
            set { id_suivant = value; }
        }
        public int Poids
        {
            get { return poids; }
            set { poids = value; }
        }


        public LienStation(int id, string nom, NoeudsStation<T> id_precedent,NoeudsStation<T> id_suivant, int poids = 1)
        {
            this.id = id;
            this.nom = nom;
            this.id_precedent = id_precedent;
            this.id_suivant = id_suivant;
            this.poids = poids;
        }
    }
}
