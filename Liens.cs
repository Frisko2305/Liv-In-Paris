using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphe_maVersion
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
