namespace Liv_In_Paris
{
    internal class GraphVisualizer<T> : Form
    {
        private Graphe<T> graphe;
        private const int Largeur = 1600;  /// Taille de la fenêtre
        private const int Hauteur = 1000;
        private const int RayonNoeud = 15; /// Taille des nœuds

        public GraphVisualizer(Graphe<T> graphe)
        {
            this.graphe = graphe;
            this.Text = "Visualisation du Graphe";
            this.Size = new Size(Largeur, Hauteur);
            this.Paint += new PaintEventHandler(DessinerGraphe);
        }

        private void DessinerGraphe(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Calculer les limites des coordonnées
            var (minLongitude, maxLongitude, minLatitude, maxLatitude) = CalculerLimites();


            /// Dessiner les liens
            
            foreach (var lien in graphe.Liens_Pte)
            {
                if (lien.Id_precedent != null && lien.Id != null)
                {
                    PointF point1 = NormaliserCoordonnees(lien.Id_precedent.Id, minLongitude, maxLongitude, minLatitude, maxLatitude);
                    PointF point2 = NormaliserCoordonnees(lien.Id, minLongitude, maxLongitude, minLatitude, maxLatitude);

                    g.DrawLine(ColorationLien(lien), point1, point2);
                }
            }

            /// Dessiner les noeuds
            Brush brushNoeud = Brushes.Blue;
            foreach (var noeud in graphe.Noeuds_Pte)
            {
                PointF point = NormaliserCoordonnees(noeud.Id, minLongitude, maxLongitude, minLatitude, maxLatitude);
                g.FillEllipse(brushNoeud, point.X - RayonNoeud / 2, point.Y - RayonNoeud / 2, RayonNoeud, RayonNoeud);
            }
        }

        public static void AfficherGraphe<T>(Graphe<T> graphe)
        {
            System.Windows.Forms.Application.Run(new GraphVisualizer<T>(graphe));
        }

        private (double minLongitude, double maxLongitude, double minLatitude, double maxLatitude) CalculerLimites()
        {
            double minLongitude = graphe.Noeuds_Pte.Min(n => Convert.ToDouble(n.Longitude));
            double maxLongitude = graphe.Noeuds_Pte.Max(n => Convert.ToDouble(n.Longitude));
            double minLatitude = graphe.Noeuds_Pte.Min(n => Convert.ToDouble(n.Latitude));
            double maxLatitude = graphe.Noeuds_Pte.Max(n => Convert.ToDouble(n.Latitude));

            return (minLongitude, maxLongitude, minLatitude, maxLatitude);
        }

        private PointF NormaliserCoordonnees(T id, double minLongitude, double maxLongitude, double minLatitude, double maxLatitude)
        {
            NoeudsStation<T> noeud = graphe.RechercherNoeud(Convert.ToInt32(id));
            double longitude = Convert.ToDouble(noeud.Longitude);
            double latitude = Convert.ToDouble(noeud.Latitude);

             float x = (float)( 0.85*(longitude - minLongitude) / (maxLongitude - minLongitude) * Largeur) + 100;
            float y = (float)( 0.85*(latitude - minLatitude) / (maxLatitude - minLatitude) * Hauteur);

            y = Hauteur - y - 90;
            return new PointF(x, y);
        }

        private  Pen ColorationLien(LienStation<T> lien)
        {

            switch(this.graphe.RechercherNoeud(Convert.ToInt32(lien.Id)).Libelle)
            {
                case "1":
                return new Pen(Color.FromArgb(0xF9, 0xA8, 0x00),4);
             
                case "2":
                return new Pen(Color.FromArgb(0x00, 0xA1, 0xDE) ,4);

                case "3":
                return new Pen(Color.FromArgb(0x6C, 0xC2, 0x4A), 4);
                
                case "3bis":
                return new Pen(Color.FromArgb(0x00, 0x9B, 0x77),4);
                
                case "4":
                return new Pen(Color.FromArgb(0x8A, 0x3E, 0x98), 4);
                
                case "5":
                return new Pen(Color.FromArgb(0xF6, 0x5A, 0x22), 4);
                
                case "6":
                return new Pen(Color.FromArgb(0x00, 0x9B, 0x3A), 4);
                
                case "7":
                return new Pen(Color.FromArgb(0xEE, 0x3E, 0x80), 4);
                
                case "7bis":
                return new Pen(Color.FromArgb(0xC1, 0x27, 0x2D), 4);
                
                case "8":
                return new Pen(Color.FromArgb(0xA4, 0x3E, 0x98), 4);
                
                case "9":
                return new Pen(Color.FromArgb(0xE6, 0xB9, 0x0F), 4);
                
                case "10":
                return new Pen(Color.FromArgb(0xFF, 0xC6, 0x00), 4);
                
                case "11":
                return new Pen(Color.FromArgb(0x00, 0x73, 0xB6), 4);
                
                case "12":
                return new Pen(Color.FromArgb(0x00, 0xA6, 0x4F), 4);
                
                case "13":
                return new Pen(Color.FromArgb(0x75, 0x8C, 0x90), 4);
                
                case "14":
                return new Pen(Color.FromArgb(0xF2, 0x9C, 0xA3), 4);
            }
            throw new Exception("Libelle n'existe pas");

        }
    }
}
