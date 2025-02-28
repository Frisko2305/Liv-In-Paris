using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    internal class Noeuds
    {
        private int numero;
        private double x;

        private double y;


        public int Numero
        {
            get { return numero; }
        }

        public double X
        {
            get { return x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return y; }
            set { this.y = value; }
        }

        public Noeuds(int son_numero)
        {
            this.numero = son_numero;
            this.x = 0;
            this.y = 0;
        }
    }
}
