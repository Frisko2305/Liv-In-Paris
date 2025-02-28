CREATE DATABASE Psi;	
USE Psi;

CREATE DATABASE Psi;
USE Psi;

CREATE TABLE Particulier(
   Nom VARCHAR(50),
   Prenom VARCHAR(50),
   Num_tel CHAR(10),
   email VARCHAR(100),
   Num_Rue INT,
   Rue VARCHAR(50),
   CP INT,
   Ville VARCHAR(50),
   PRIMARY KEY(Nom, Prenom)
);

CREATE TABLE Entreprise(
   SIRET INT,
   Nom_entreprise VARCHAR(50),
   Nom_referent VARCHAR(50),
   Num_tel_referent CHAR(10),
   Num_Rue INT,
   Rue VARCHAR(50),
   CP INT,
   Ville VARCHAR(50),
   PRIMARY KEY(SIRET)
);

CREATE TABLE Plat(
   Id_plat INT,
   Type_de_plat VARCHAR(14),
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

CREATE TABLE Livraison(
   Adresse_de_livraison VARCHAR(50),
   PRIMARY KEY(Adresse_de_livraison)
);

CREATE TABLE Ingredients(
   Id_ingredient VARCHAR(50),
   Nom VARCHAR(50),
   Quantite INT,
   PRIMARY KEY(Id_ingredient)
);

CREATE TABLE Cuisinier(
   Id INT,
   Mdp VARCHAR(20),
   Nom_cuisinier VARCHAR(50) NOT NULL,
   Prenom_cuisinier VARCHAR(50) NOT NULL,
   PRIMARY KEY(Id),
   UNIQUE(Nom, Prenom),
   FOREIGN KEY(Nom_cuisinier, Prenom_cuisinier) REFERENCES Particulier(Nom, Prenom)
);

CREATE TABLE Client(
   Id_client INT,
   Mdp VARCHAR(20),
   Nom VARCHAR(50) NOT NULL,
   Prenom VARCHAR(50) NOT NULL,
   Nom_particulier VARCHAR(50),
   Prenom_particulier VARCHAR(50),
   SIRET_entreprise INT,
   PRIMARY KEY(Id_client),
   UNIQUE(Nom, Prenom),
   UNIQUE(SIRET),
   FOREIGN KEY(Nom_particulier, Prenom_particulier) REFERENCES Particulier(Nom, Prenom) ON DELETE CASCADE,
   FOREIGN KEY(SIRET_entreprise) REFERENCES Entreprise(SIRET) ON DELETE CASCADE,
   CHECK (
		(Nom_particulier IS NOT NULL AND Prenom_particulier IS NOT NULL AND SIRET_entreprise IS NULL) OR
        (Nom_particulier IS NULL AND Prenom_particulier IS NULL AND SIRET_entreprise IS NOT NULL)
	)
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

CREATE TABLE Avis(
   Note DECIMAL(2,1),
   Commentaire VARBINARY(50),
   Nom VARCHAR(50) NOT NULL,
   Prenom VARCHAR(50) NOT NULL,
   SIRET INT NOT NULL,
   Id_plat INT NOT NULL,
   Id_client INT NOT NULL,
   Id INT NOT NULL,
   PRIMARY KEY(Note),
   FOREIGN KEY(Nom, Prenom) REFERENCES Particulier(Nom, Prenom),
   FOREIGN KEY(SIRET) REFERENCES Entreprise(SIRET),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat),
   FOREIGN KEY(Id_client) REFERENCES Client(Id_client),
   FOREIGN KEY(Id) REFERENCES Cuisinier(Id)
);

CREATE TABLE Propose(
   Id INT,
   Id_plat INT,
   PRIMARY KEY(Id, Id_plat),
   FOREIGN KEY(Id) REFERENCES Cuisinier(Id),
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
   Adresse_de_livraison VARCHAR(50),
   Nombres_adresse_de_livraison INT,
   PRIMARY KEY(Id_commande, Adresse_de_livraison),
   FOREIGN KEY(Id_commande) REFERENCES Commande(Id_commande),
   FOREIGN KEY(Adresse_de_livraison) REFERENCES Livraison(Adresse_de_livraison)
);

CREATE TABLE Contient_(
   Id_plat INT,
   Id_ingredient VARCHAR(50),
   PRIMARY KEY(Id_plat, Id_ingredient),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat),
   FOREIGN KEY(Id_ingredient) REFERENCES Ingredients(Id_ingredient)
);
