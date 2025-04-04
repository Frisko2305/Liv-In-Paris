using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class LienStation
    {

        private int id;
        private string nom;
        private NoeudsStation id_precedent;
        private NoeudsStation id_suivant;
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

        public NoeudsStation Id_precedent
        {
            get { return Id_precedent; }
            set { id_precedent = value; }
        }

        public NoeudsStation Id_suivant
        {
            get { return Id_suivant; }
            set { id_suivant = value; }
        }



        public int Poids
        {
            get { return poids; }
            set { poids = value; }
        }


        public LienStation(int id, string nom, NoeudsStation id_precedent,NoeudsStation id_suivant, int poids = 1)
        {
            this.id = id;
            this.nom = nom;
            this.id_precedent = id_precedent;
            this.id_suivant = id_suivant;
            this.poids = poids;
        }


    }
}
