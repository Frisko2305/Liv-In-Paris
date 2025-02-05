namespace Graphe_maVersion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Pour représenter l'association grâce à un graphe :
            // Nous plaçons un noeud dans chaque coin d'une matrice carré.
            // Nous plaçons aussi des liens s'ils existent
            // C'est un modèle simple mais il représente notre association

            // Taille de la matrice (4x4 pour cet exemple)
            int taille = 4;
            Graphe graphe = new Graphe(taille);

            // Création des nœuds dans les coins
            var noeud1 = new Noeuds(1, new int[] { 0, 0 });
            var noeud2 = new Noeuds(2, new int[] { 0, 3 });
            var noeud3 = new Noeuds(3, new int[] { 3, 0 });
            var noeud4 = new Noeuds(4, new int[] { 3, 3 });

            // Ajout des nœuds au graphe
            graphe.AjouterNoeud(noeud1);
            graphe.AjouterNoeud(noeud2);
            graphe.AjouterNoeud(noeud3);
            graphe.AjouterNoeud(noeud4);

            // Création des liens sur les contours
            graphe.AjouterLien(noeud1, noeud2); // Lien haut
            graphe.AjouterLien(noeud2, noeud4); // Lien droite
            graphe.AjouterLien(noeud4, noeud3); // Lien bas
            graphe.AjouterLien(noeud3, noeud1); // Lien gauche

            // Affichage de la matrice
            graphe.AfficherMatrice();
        }
    }
}
