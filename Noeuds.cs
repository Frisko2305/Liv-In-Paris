using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphe_maVersion
{
    internal class Noeuds
    {
        private int numero;
        private int[] localisation;

        public int Numero
        {
            get { return numero; }
        }

        public int[] Localisation
        {
            get { return localisation; }
        }

        public Noeuds(int son_numero, int[] sa_localisation)
        {
            this.numero = son_numero;
            this.localisation = sa_localisation;
        }
    }
}
