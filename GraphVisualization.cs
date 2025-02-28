using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Liv_In_Paris
{
    internal class GraphVisualizer : Form
    {
        private Graphe graphe;
        private const int Largeur = 800;  /// Taille de la fenêtre
        private const int Hauteur = 800;
        private const int RayonNoeud = 20; /// Taille des nœuds

        public GraphVisualizer(Graphe graphe)
        {
            this.graphe = graphe;
            this.Text = "Visualisation du Graphe";
            this.Size = new Size(Largeur, Hauteur);
            this.Paint += new PaintEventHandler(DessinerGraphe);
        }

        private void DessinerGraphe(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
            Brush brush = Brushes.Black;

            /// Dessiner les liens
            Pen penLien = new Pen(Color.Gray, 2);
            foreach (var lien in graphe.Liens_Pte)
            {
                g.DrawLine(penLien,
                    (float)lien.Membre.X, (float)lien.Membre.Y,
                    (float)lien.MembreAutre.X, (float)lien.MembreAutre.Y);
            }

            /// Dessiner les noeuds
            Brush brushNoeud = Brushes.Blue;
            foreach (var noeud in graphe.Noeuds_Pte)
            {
                float x = (float)noeud.X - RayonNoeud / 2;
                float y = (float)noeud.Y - RayonNoeud / 2;

                g.FillEllipse(brushNoeud, x, y, RayonNoeud, RayonNoeud);
                g.DrawString(noeud.Numero.ToString(), font, brush, x + 5, y + 5);
            }
        }

        public static void AfficherGraphe(Graphe graphe)
        {
            System.Windows.Forms.Application.Run(new GraphVisualizer(graphe));
        }

    }
}
