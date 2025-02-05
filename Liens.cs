using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_in_Paris
{
    internal class Liens
    {
        private Noeuds membre;
        private Noeuds membre_autre;

        public Noeuds Membre
        {
            get { return membre; }
        }

        public Noeuds MembreAutre
        {
            get { return membre_autre; }
        }

        public Liens(Noeuds membre, Noeuds membre_autre)
        {
            this.membre = membre;
            this.membre_autre = membre_autre;
        }
    }
}
