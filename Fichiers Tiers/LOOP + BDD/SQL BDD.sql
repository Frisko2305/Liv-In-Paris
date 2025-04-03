CREATE DATABASE IF NOT EXISTS Psi;
--  DROP DATABASE IF EXISTS Psi;
USE Psi;
			-- Tables d'entité
CREATE TABLE Particulier(
   Nom VARCHAR(50),
   Prenom VARCHAR(50),
   Num_tel CHAR(14),		-- pour faire ceux qui rentrent chiffre après chiffres et ceux qui font 2 à 2
   email VARCHAR(100),
   Num_Rue INT,
   Rue VARCHAR(50),
   CP INT,
   Ville VARCHAR(50),
   PRIMARY KEY(Nom, Prenom)
);

CREATE TABLE Entreprise(
   SIRET BIGINT,
   Nom_entreprise VARCHAR(50),
   Nom_referent VARCHAR(50),
   Num_tel_referent CHAR(14),
   Num_Rue INT,
   Rue VARCHAR(50),
   CP INT,
   Ville VARCHAR(50),
   PRIMARY KEY(SIRET)
);

CREATE TABLE Client(
   Id_client INT,
   Mdp VARCHAR(20),
   Nom_particulier VARCHAR(50),
   Prenom_particulier VARCHAR(50),
   SIRET_entreprise BIGINT,
   PRIMARY KEY(Id_client),
   UNIQUE(SIRET_entreprise),
   FOREIGN KEY(Nom_particulier, Prenom_particulier) REFERENCES Particulier(Nom, Prenom) ON DELETE CASCADE,
   FOREIGN KEY(SIRET_entreprise) REFERENCES Entreprise(SIRET) ON DELETE CASCADE,
   CHECK (
		(Nom_particulier IS NOT NULL AND Prenom_particulier IS NOT NULL AND SIRET_entreprise IS NULL) OR
        (Nom_particulier IS NULL AND Prenom_particulier IS NULL AND SIRET_entreprise IS NOT NULL)
	)
);

CREATE TABLE Cuisinier(
   Id_cuisinier INT,
   Mdp VARCHAR(20),
   Nom_cuisinier VARCHAR(50) NOT NULL,
   Prenom_cuisinier VARCHAR(50) NOT NULL,
   PRIMARY KEY(Id_cuisinier),
   UNIQUE(Nom_cuisinier, Prenom_cuisinier),
   FOREIGN KEY(Nom_cuisinier, Prenom_cuisinier) REFERENCES Particulier(Nom, Prenom) ON DELETE CASCADE
);

CREATE TABLE Plat(
   Id_plat INT,
   Type_de_plat VARCHAR(100),
   Stock INT,
   Nb_personnes INT,
   Prix DECIMAL(5,2),
   Date_fabrication DATE,
   Date_peremption DATE,
   Type_de_cuisine VARCHAR(50),
   Regime_alimentaire VARCHAR(50),
   Photo BINARY,
   PRIMARY KEY(Id_plat)
);

CREATE TABLE Ingredients(
   Id_ingredient VARCHAR(50),
   Nom VARCHAR(50),
   Quantite INT,
   PRIMARY KEY(Id_ingredient)
);

CREATE TABLE Commande(
   Id_commande INT,
   Prix DECIMAL(5,2),
   Statut VARCHAR(30),
   Prevision_arrivee DATETIME,
   Id_client INT NOT NULL,
   PRIMARY KEY(Id_commande),
   FOREIGN KEY(Id_client) REFERENCES Client(Id_client)
);

CREATE TABLE Livraison(
   Num_Rue INT,
   Rue VARCHAR(50),
   CP INT,
   Ville VARCHAR(50),
   Id_commande INT,
   PRIMARY KEY(Num_Rue, Rue, CP, Ville),
   FOREIGN KEY(Id_commande) REFERENCES Commande(Id_commande)
);

CREATE TABLE Avis(
   Id_avis INT,
   Note DECIMAL(2,1),
   Commentaire VARBINARY(50),
   Nom VARCHAR(50) NOT NULL,
   Prenom VARCHAR(50) NOT NULL,
   SIRET BIGINT NOT NULL,
   Id_plat INT NOT NULL,
   Id_client INT NOT NULL,
   Id_cuisinier INT NOT NULL,
   PRIMARY KEY(Id_avis),
   FOREIGN KEY(Nom, Prenom) REFERENCES Particulier(Nom, Prenom) ON DELETE CASCADE,
   FOREIGN KEY(SIRET) REFERENCES Entreprise(SIRET) ON DELETE CASCADE,
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat) ON DELETE CASCADE,
   FOREIGN KEY(Id_client) REFERENCES Client(Id_client) ON DELETE CASCADE,
   FOREIGN KEY(Id_cuisinier) REFERENCES Cuisinier(Id_cuisinier) ON DELETE CASCADE
);
		
        -- Table d'association
CREATE TABLE Propose(
   Id_cuisinier INT,
   Id_plat INT,
   PRIMARY KEY(Id_cuisinier, Id_plat),
   FOREIGN KEY(Id_cuisinier) REFERENCES Cuisinier(Id_cuisinier),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat)
);

CREATE TABLE Contient(
   Id_plat INT,
   Id_commande INT,
   Nb_de_plats INT,
   PRIMARY KEY(Id_plat, Id_commande),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat),
   FOREIGN KEY(Id_commande) REFERENCES Commande(Id_commande)
);

CREATE TABLE Inclus(
   Id_commande INT,
   Num_Rue INT,
   Rue VARCHAR(50),
   CP INT,
   Ville VARCHAR(50),
   Nombres_adresse_de_livraison INT,
   PRIMARY KEY(Id_commande, Num_Rue, Rue, CP, Ville),
   FOREIGN KEY(Id_commande) REFERENCES Commande(Id_commande),
   FOREIGN KEY(Num_Rue, Rue, CP, Ville) REFERENCES Livraison(Num_Rue, Rue, CP, Ville)
);

CREATE TABLE Contient_(
   Id_plat INT,
   Id_ingredient VARCHAR(50),
   PRIMARY KEY(Id_plat, Id_ingredient),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat),
   FOREIGN KEY(Id_ingredient) REFERENCES Ingredients(Id_ingredient)
);


			-- Requetes simples
            
SELECT COUNT(*)
FROM Client
JOIN Particulier ON Client.Nom_particulier = Particulier.Nom
WHERE Particulier.Ville = 'Paris';

SELECT *
FROM Ingredients
WHERE Quantite BETWEEN 50 AND 150;

SELECT count(*)
FROM Cuisinier
WHERE Prenom_cuisinier LIKE 'L%';

SELECT Nom_particulier,Prenom_particulier
FROM Client
JOIN Particulier
ON Particulier.Num_Tel
WHERE Particulier.Num_Tel LIKE '01 2%';

SELECT Nom_entreprise
FROM Entreprise
WHERE SIRET LIKE '16%';
