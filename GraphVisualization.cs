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
        private const int Hauteur =600;
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

            double longitude = 0;
            double latitude = 0;
            foreach (var lien in graphe.Liens_Pte)
            {
               
                g.DrawLine(penLien,
                    (float)RecupereCoordonnee(lien, "longitude"), (float)RecupereCoordonnee(lien, "latitude"),
                    (float)lien.Id_precedent.Longitude, (float)lien.Id_precedent.Latitude);
            }

            /// Dessiner les noeuds
            Brush brushNoeud = Brushes.Blue;
            foreach (var noeud in graphe.Noeuds_Pte)
            {
                float x = (float)ConvertLongitue( noeud);
                float y = (float)ConvertLatitude(noeud);

                g.FillEllipse(brush, x - 5, y - 5, 10, 10);
                g.DrawString(noeud.Nom.ToString(), font, brush, x + 5, y + 5);
                
            }
        }

        public static void AfficherGraphe(Graphe graphe)
        {
            System.Windows.Forms.Application.Run(new GraphVisualizer(graphe));
        }

        public double RecupereCoordonnee(LienStation lien, string type)
        {
            NoeudsStation noeud = this.graphe.RechercherNoeud(lien.Id);
            if (type=="longitude")
            {
                return (double)noeud.Longitude;
            }
            else if(type=="latitude")
            {
                return (double)noeud.Latitude;
            }
            else
            {
                throw new ArgumentException("Ceci est une erreur d'argument.");
            }
        }

        private float ConvertLongitue(NoeudsStation noeud)
        {
            return (float)((noeud.Longitude + 73.0) * 10);
        }

        private float ConvertLatitude(NoeudsStation noeud)
        {
            return (float)((90.0 - noeud.Latitude) * 10);
        }


    }
}
