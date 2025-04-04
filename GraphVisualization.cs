using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Liv_In_Paris
{
    internal class GraphVisualizer<T> : Form
    {
        private Graphe<T> graphe;
        private const int Largeur = 1600;  /// Taille de la fenêtre
        private const int Hauteur =1000;
        private const int RayonNoeud = 20; /// Taille des nœuds

        public GraphVisualizer(Graphe<T> graphe)
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

            // Calculer les limites des coordonnées
            var (minLongitude, maxLongitude, minLatitude, maxLatitude) = CalculerLimites();


            /// Dessiner les liens
            Pen penLien = new Pen(Color.Gray, 2);

            foreach (var lien in graphe.Liens_Pte)
            {
                if (lien.Id_precedent != null && lien.Id_suivant != null)
                {
                    PointF point1 = NormaliserCoordonnees(lien.Id_precedent, minLongitude, maxLongitude, minLatitude, maxLatitude);
                    PointF point2 = NormaliserCoordonnees(lien.Id_suivant, minLongitude, maxLongitude, minLatitude, maxLatitude);

                    g.DrawLine(penLien, point1, point2);
                }
               
                /*g.DrawLine(penLien,
                    (float)RecupereCoordonnee(lien, "longitude"), (float)RecupereCoordonnee(lien, "latitude"),
                    (float)ConvertParametre(lien.Id_precedent.Longitude), (float)ConvertParametre(lien.Id_precedent.Latitude));
                */
            }

            /// Dessiner les noeuds
            Brush brushNoeud = Brushes.Blue;
            foreach (var noeud in graphe.Noeuds_Pte)
            {
                PointF point = NormaliserCoordonnees(noeud, minLongitude, maxLongitude, minLatitude, maxLatitude);
                /*
                float x = (float)ConvertLongitue( noeud);
                float y = (float)ConvertLatitude(noeud);
                
                g.FillEllipse(brush, x - 5, y - 5, 10, 10);
                g.DrawString(noeud.Nom.ToString(), font, brush, x + 5, y + 5);
                */
                g.FillEllipse(brushNoeud, point.X - RayonNoeud / 2, point.Y - RayonNoeud / 2, RayonNoeud, RayonNoeud);
                // g.DrawString(noeud.Nom.ToString(), font, brush, point.X + RayonNoeud / 2, point.Y + RayonNoeud / 2);
            }
        }

        public static void AfficherGraphe<T>(Graphe<T> graphe)
        {

            System.Windows.Forms.Application.Run(new GraphVisualizer<T>(graphe));
        }




        public double RecupereCoordonnee(LienStation<T> lien, string type)
        {
            NoeudsStation<T> noeud = this.graphe.RechercherNoeud(lien.Id);
            if (type=="longitude")
            {
                dynamic longitude = noeud.Longitude;
                return longitude;
            }
            else if(type=="latitude")
            {
                dynamic latitude = noeud.Latitude;
                return latitude;
            }
            else
            {
                throw new ArgumentException("Ceci est une erreur d'argument.");
            }
        }

        private (double minLongitude, double maxLongitude, double minLatitude, double maxLatitude) CalculerLimites()
        {
            double minLongitude = graphe.Noeuds_Pte.Min(n => Convert.ToDouble(n.Longitude));
            double maxLongitude = graphe.Noeuds_Pte.Max(n => Convert.ToDouble(n.Longitude));
            double minLatitude = graphe.Noeuds_Pte.Min(n => Convert.ToDouble(n.Latitude));
            double maxLatitude = graphe.Noeuds_Pte.Max(n => Convert.ToDouble(n.Latitude));

            return (minLongitude, maxLongitude, minLatitude, maxLatitude);
        }

        private PointF NormaliserCoordonnees(NoeudsStation<T> noeud, double minLongitude, double maxLongitude, double minLatitude, double maxLatitude)
        {
            double longitude = Convert.ToDouble(noeud.Longitude);
            double latitude = Convert.ToDouble(noeud.Latitude);

            float x = (float)((longitude - minLongitude) / (maxLongitude - minLongitude) * Largeur);
            float y = (float)((latitude - minLatitude) / (maxLatitude - minLatitude) * Hauteur);

            y = Hauteur - y;
            return new PointF(x, y);
        }
    }
}
