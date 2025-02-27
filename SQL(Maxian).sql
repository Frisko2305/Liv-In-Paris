CREATE DATABASE Psi;
USE Psi;

CREATE DATABASE Psi;
USE Psi;

CREATE TABLE Cuisinier(
   Id INT,
   Mdp VARCHAR(20),
   Nom VARCHAR(50),
   Prénom VARCHAR(50),
   Adresse VARCHAR(100),
   Ville VARCHAR(50),
   Num_tél CHAR(10),
   Adresse_mail VARCHAR(50),
   PRIMARY KEY(Id)
);

CREATE TABLE Particulier(
   Nom VARCHAR(50) PRIMARY KEY,
   Prénom VARCHAR(50),
   Adresse VARCHAR(100),
   Ville VARCHAR(50),
   Num_tél CHAR(10),
   PRIMARY KEY(Nom)
);

CREATE TABLE Entreprise(
   SIRET INT PRIMARY KEY,
   Nom_entreprise VARCHAR(50),
   Nom_référent VARCHAR(50),
   Num_tél_référent CHAR(10),
   Adresse VARCHAR(100),
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
   Date_péremption DATE,
   Type_de_cuisine VARCHAR(50),
   Régime_alimentaire VARCHAR(50),
   Photo BLOB,
   PRIMARY KEY(Id_plat)
);

CREATE TABLE Livraison(
   Adresse_de_livraison VARCHAR(50),
   PRIMARY KEY(Adresse_de_livraison)
);

CREATE TABLE Ingrédients(
   Id_ingrédient VARCHAR(50),
   Nom Varchar(50),
   Quantité INT,
   PRIMARY KEY(Id_ingrédient)
);

CREATE TABLE Clients(
   Id_client INT,
   Mdp VARCHAR(20),
   Nom VARCHAR(50) NOT NULL,
   SIRET INT NOT NULL,
   PRIMARY KEY(Id_client),
   UNIQUE(Nom),
   UNIQUE(SIRET),
   FOREIGN KEY(Nom) REFERENCES Particulier(Nom),
   FOREIGN KEY(SIRET) REFERENCES Entreprise(SIRET)
);

CREATE TABLE Commande(
   Id_commande INT,
   Sous_total DECIMAL(5,2),
   Statut VARCHAR(30),
   Prévision_arrivée DATETIME,
   Id_client INT NOT NULL,
   PRIMARY KEY(Id_commande),
   FOREIGN KEY(Id_client) REFERENCES Client(Id_client)
);

CREATE TABLE Avis(
   Note DECIMAL(2,1),
   Commentaire VARBINARY(50),
   Id_plat INT NOT NULL,
   Id_client INT NOT NULL,
   Id INT NOT NULL,
   PRIMARY KEY(Note),
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
   Id_ingrédient VARCHAR(50),
   PRIMARY KEY(Id_plat, Id_ingrédient),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat),
   FOREIGN KEY(Id_ingrédient) REFERENCES Ingrédients(Id_ingrédient)
);
	
