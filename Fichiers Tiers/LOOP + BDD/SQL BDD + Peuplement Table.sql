CREATE DATABASE IF NOT EXISTS Psi;
 -- DROP DATABASE IF EXISTS Psi;
USE Psi;
			-- Tables d'entité
CREATE TABLE Particulier(
   Nom VARCHAR(50),
   Prenom VARCHAR(50),
   Num_tel CHAR(14),		-- On oblige à saisire sous format "0X XX XX XX XX"
   email VARCHAR(100),
   Num_Rue INT,
   Rue VARCHAR(50),
   CP INT,
   Ville VARCHAR(50),
   PRIMARY KEY(Nom, Prenom)
);
-- DELETE FROM Particulier;

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
-- DELETE FROM Entreprise;

CREATE TABLE Client(
   Id_client INT,
   Mdp VARCHAR(20),
   Nom_particulier VARCHAR(50),
   Prenom_particulier VARCHAR(50),
   SIRET_entreprise BIGINT,
   Photo_profil MEDIUMBLOB,
   PRIMARY KEY(Id_client),
   UNIQUE(SIRET_entreprise),
   FOREIGN KEY(Nom_particulier, Prenom_particulier) REFERENCES Particulier(Nom, Prenom) ON DELETE CASCADE,
   FOREIGN KEY(SIRET_entreprise) REFERENCES Entreprise(SIRET) ON DELETE CASCADE,
   CHECK (
		(Nom_particulier IS NOT NULL AND Prenom_particulier IS NOT NULL AND SIRET_entreprise IS NULL) OR
        (Nom_particulier IS NULL AND Prenom_particulier IS NULL AND SIRET_entreprise IS NOT NULL)
	)
);
-- DELETE FROM Client;

CREATE TABLE Cuisinier(
   Id_cuisinier INT,
   Mdp VARCHAR(20),
   Nom_cuisinier VARCHAR(50) NOT NULL,
   Prenom_cuisinier VARCHAR(50) NOT NULL,
   Photo_profil MEDIUMBLOB,
   PRIMARY KEY(Id_cuisinier),
   UNIQUE(Nom_cuisinier, Prenom_cuisinier),
   FOREIGN KEY(Nom_cuisinier, Prenom_cuisinier) REFERENCES Particulier(Nom, Prenom) ON DELETE CASCADE
);
-- DELETE FROM Cuisinier;

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
   Photo BLOB,
   PRIMARY KEY(Id_plat)
);
-- DELETE FROM Plat;

CREATE TABLE Ingredients(
   Id_ingredient VARCHAR(50),
   Nom VARCHAR(50),
   Quantite INT,
   PRIMARY KEY(Id_ingredient)
);
-- DELETE FROM Ingredients;

CREATE TABLE Commande(
   Id_commande INT,
   Prix DECIMAL(5,2),
   Statut VARCHAR(30),
   Prevision_arrivee DATETIME,
   Id_client INT NOT NULL,
   PRIMARY KEY(Id_commande),
   FOREIGN KEY(Id_client) REFERENCES Client(Id_client)
);
-- DELETE FROM Commande;

CREATE TABLE Livraison(
   Num_Rue INT,
   Rue VARCHAR(50),
   CP INT,
   Ville VARCHAR(50),
   Id_commande INT,
   PRIMARY KEY(Num_Rue, Rue, CP, Ville),
   FOREIGN KEY(Id_commande) REFERENCES Commande(Id_commande)
);
-- DELETE FROM Livraison;

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
-- DELETE FROM Avis;
		
        -- Table d'association
CREATE TABLE Propose(
   Id_cuisinier INT,
   Id_plat INT,
   PRIMARY KEY(Id_cuisinier, Id_plat),
   FOREIGN KEY(Id_cuisinier) REFERENCES Cuisinier(Id_cuisinier),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat)
);
-- DELETE FROM Propose;

CREATE TABLE Contient(
   Id_plat INT,
   Id_commande INT,
   Nb_de_plats INT,
   PRIMARY KEY(Id_plat, Id_commande),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat),
   FOREIGN KEY(Id_commande) REFERENCES Commande(Id_commande)
);
-- DELETE FROM Contient;

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
-- DELETE FROM Inclus;

CREATE TABLE Contient_(
   Id_plat INT,
   Id_ingredient VARCHAR(50),
   PRIMARY KEY(Id_plat, Id_ingredient),
   FOREIGN KEY(Id_plat) REFERENCES Plat(Id_plat),
   FOREIGN KEY(Id_ingredient) REFERENCES Ingredients(Id_ingredient)
);
-- DELETE FROM Contient_;

		-- Peuplement des tables
	
    -- Peuplement Particulier
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Dupont', 'Jean', '01 45 23 67 89', 'jean.dupont@fakemail.com', '12', 'Rue de Rivoli', '75001', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Martin', 'Sophie', '01 56 34 78 90', 'sophie.martin@mockmail.net', '45', 'Avenue des Champs-Élysées', '75008', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Bernard', 'Michel', '01 67 89 45 23', 'michel.bernard@testinbox.org', '78', 'Boulevard Haussmann', '75009', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Petit', 'Claire', '01 78 90 56 34', 'claire.petit@randommail.io', '23', 'Rue Saint-Honoré', '75001', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Moreau', 'Paul', '01 89 45 67 23', 'paul.moreau@demoemail.com', '56', 'Avenue Foch', '75116', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Lefebvre', 'Isabelle', '01 90 56 78 12', 'isabelle.lefebvre@bogusemail.net', '34', 'Rue Lafayette', '75009', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Garcia', 'Luc', '01 23 67 89 45', 'luc.garcia@dummyinbox.com', '89', 'Boulevard Saint-Michel', '75005', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Lopez', 'Marie', '01 34 78 90 56', 'marie.lopez@madeupmail.org', '67', 'Rue de la Paix', '75002', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Gonzalez', 'Nicolas', '01 45 67 23 89', 'nicolas.gonzalez@samplemail.io', '11', 'Avenue de l''Opéra', '75001', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Rodriguez', 'Camille', '01 56 78 12 34', 'camille.rodriguez@inventedmail.net', '90', 'Rue du Faubourg Saint-Honoré', '75008', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Hernandez', 'Antoine', '01 67 89 45 12', 'antoine.hernandez@nopemail.com', '55', 'Boulevard Malesherbes', '75008', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Schneider', 'Julien', '01 78 90 34 56', 'julien.schneider@fakemail.net', '42', 'Rue de Turenne', '75003', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Fischer', 'Elise', '01 89 12 56 78', 'elise.fischer@mockdomain.org', '31', 'Boulevard de Sébastopol', '75002', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Weber', 'Thomas', '01 90 23 67 45', 'thomas.weber@testmail.io', '77', 'Avenue Victor Hugo', '75116', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Muller', 'Laurence', '01 12 34 56 78', 'laurence.muller@randominbox.com', '25', 'Rue Réaumur', '75002', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Roche', 'Frédéric', '07 10 91 64 54', 'frédéric.roche@blanc.org', 74, 'boulevard de Bègue', '97337', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Lefèvre', 'Philippe', '06 30 11 73 53', 'philippe.lefèvre@daniel.org', 2, 'rue Nicolas Dos Santos', '71913', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Baudry', 'Lucie', '07 00 42 44 57', 'lucie.baudry@daniel.fr', 97, 'boulevard Anouk Lebon', '04249', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Poulain', 'Isaac', '06 34 69 36 82', 'isaac.poulain@lecoq.fr', 67, 'avenue Audrey Bodin', '20403', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Boyer', 'Luc', '07 62 61 52 14', 'luc.boyer@becker.fr', 87, 'boulevard Timothée Julien', '52722', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Fournier', 'Thomas', '07 06 08 05 12', 'thomas.fournier@samson.org', 28, 'rue de Marques', '33697', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Brun', 'Margot', '06 04 06 95 57', 'margot.brun@lemonnier.net', 48, 'rue Barthelemy', '90398', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Bonneau', 'Louis', '07 22 58 48 95', 'louis.bonneau@laroche.org', 54, 'chemin Renée Perret', '59856', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Morel', 'Anaïs', '07 90 47 08 72', 'anaïs.morel@henry.net', 9, 'rue Garnier', '01405', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Regnier', 'Suzanne', '06 50 27 71 03', 'suzanne.regnier@vidal.com', 100, 'avenue Mathilde Buisson', '57410', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Georges', 'Susan', '06 73 36 27 75', 'susan.georges@le.fr', 49, 'rue de Bouvet', '32648', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Da Silva', 'Robert', '06 30 02 52 81', 'robert.da silva@launay.com', 97, 'boulevard de Pelletier', '11924', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Thierry', 'Nicolas', '07 17 01 16 46', 'nicolas.thierry@aubert.fr', 97, 'chemin de Ollivier', '68912', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Wagner', 'Sophie', '07 01 45 57 62', 'sophie.wagner@guillaume.org', 77, 'boulevard Patricia Lambert', '25342', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Roger', 'Dominique', '07 66 21 61 38', 'dominique.roger@pottier.net', 34, 'avenue de Morel', '71938', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Bernard', 'Tristan', '07 28 01 58 85', 'tristan.bernard@ollivier.com', 85, 'rue Jeannine Joly', '44320', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Blondel', 'Anne', '07 83 92 13 92', 'anne.blondel@barthelemy.fr', 4, 'rue Hortense Hernandez', '32431', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Bazin', 'Théodore', '07 58 88 78 71', 'théodore.bazin@paris.fr', 39, 'rue Alexandrie Guibert', '31364', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Huet', 'Maggie', '07 51 27 71 20', 'maggie.huet@humbert.fr', 18, 'avenue de Camus', '34458', 'Paris');
INSERT INTO Particulier (Nom, Prenom, Num_tel, email, Num_Rue, Rue, CP, Ville) VALUES ('Fontaine', 'Alain', '07 34 52 11 14', 'alain.fontaine@pichon.com', 19, 'rue Perrin', '46360', 'Paris');


	-- Peuplement Entreprise
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (21619009196044, 'ParisTech Solutions', 'Simon', '622351285', '16', 'Boulevard Raspail', '75001', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (16593760297582, 'Innovatech France', 'Lemoine', '679380501', '195', 'Boulevard Raspail', '75002', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (16728799614726, 'EcoEnergy Group', 'Laurent', '658678802', '30', 'Boulevard Saint-Michel', '75003', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (66845121871244, 'Digital Wave', 'Dubois', '654968966', '194', 'Boulevard Saint-Michel', '75004', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (71545421471430, 'GreenBuild Corp', 'Colin', '695814024', '188', 'Boulevard Haussmann', '75005', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (77985164597385, 'SmartCom Networks', 'Simon', '657870628', '105', 'Rue Saint-Honoré', '75006', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (50330030541557, 'NextGen Robotics', 'Lemoine', '678074981', '143', 'Avenue de l''Opéra', '75007', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (80654494216740, 'BioHealth Innov', 'Durand', '668252744', '85', 'Avenue Montaigne', '75008', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (50434007721092, 'SecureData Systems', 'Lemoine', '640812635', '59', 'Rue Saint-Honoré', '75009', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (16273402436509, 'Urban Mobility Tech', 'Durand', '699276790', '168', 'Rue Lafayette', '75010', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (12091511850336, 'CloudVision AI', 'Bertrand', '696232577', '98', 'Boulevard Saint-Michel', '75011', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (35736502840212, 'Quantum IT Solutions', 'Dubois', '654601915', '112', 'Boulevard Raspail', '75012', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (63063409867949, 'BlueSky Aerospace', 'Germain', '660874931', '79', 'Rue de Rivoli', '75013', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (10575439913150, 'Future Finance', 'Garcia', '674537251', '28', 'Rue de Rennes', '75014', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES (56317477012275, 'Solaris Energy', 'Perrot', '668885784', '195', 'Avenue des Champs-Élysées', '75015', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('00194627579614', 'Roche SARL', 'Marinon', '07 85 46 15 47', 58, 'boulevard de Bègue', '97337', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('50410902066985', 'Lefèvre Inc.', 'Constantin', '06 32 14 77 52', 10, 'rue Nicolas Dos Santos', '71913', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('75918948277878', 'Baudry Ltd', 'Marcel', '07 08 40 40 55', 26, 'boulevard Anouk Lebon', '04249', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('15575674421893', 'Poulain SAS', 'Ismael', '06 34 69 36 82', 5, 'avenue Audrey Bodin', '20403', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('42086885306941', 'Boyer SA', 'Benjamin', '07 62 61 52 14', 84, 'boulevard Timothée Julien', '52722', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('71906462471656', 'Fournier SARL', 'Feaurnet', '07 06 08 05 12', 14, 'rue de Marques', '33697', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('04527796024153', 'Brun Ltd', 'Bougenot', '06 04 06 90 57', 20, 'rue Barthelemy', '90398', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('57586684034233', 'Bonneau Inc.', 'Marcilat', '07 20 50 48 95', 2, 'chemin Renée Perret', '59856', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('31354744055777', 'Morel SA', 'Poisson', '07 90 47 08 72', 4, 'rue Garnier', '01405', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('68287167703647', 'Regnier SAS', 'Declire', '06 50 20 71 03', 58, 'avenue Mathilde Buisson', '57410', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('27925070069231', 'Georges Ltd', 'Salmendor', '06 73 36 20 75', 25, 'rue de Bouvet', '32648', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('50412516543948', 'Da Silva Inc.', 'Constanti', '06 30 02 50 81', 36, 'boulevard de Pelletier', '11924', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('67798923742406', 'Thierry SAS', 'Parginot', '07 17 01 10 46', 78, 'chemin de Ollivier', '68912', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('57927783352507', 'Wagner SARL', 'Massine', '07 01 40 57 62', 14, 'boulevard Patricia Lambert', '25342', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('57518838229779', 'Roger SA', 'Levicar', '07 60 21 61 38', 20, 'avenue de Morel', '71938', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('90412516543948', 'Bernard Ltd', 'Jinot', '07 28 01 50 85', 10, 'rue Jeannine Joly', '44320', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('72636964897378', 'Blondel Inc.', 'Blondot', '07 83 92 10 92', 8, 'rue Hortense Hernandez', '32431', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('78676484385935', 'Bazin SARL', 'Parcival', '07 58 80 78 71', 34, 'rue Alexandrie Guibert', '31364', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('54728694242483', 'Huet SA', 'Florait', '07 51 27 71 20', 25, 'avenue de Camus', '34458', 'Paris');
INSERT INTO Entreprise (SIRET, Nom_entreprise, Nom_referent, Num_tel_referent, Num_Rue, Rue, CP, Ville) VALUES ('25490269184448', 'Fontaine Ltd', 'Gargen', '07 34 50 11 14', 46, 'rue Perrin', '46360', 'Paris');


    -- Peuplement Client
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (8189, 'J[.>k#/|', 'Dupont', 'Jean', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (1499, 'zi/~r>Rl', NULL, NULL, '16593760297582');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (4091, '6ZP=^GLv', NULL, NULL, '16728799614726');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (7796, 't/f96|FO', NULL, NULL, '66845121871244');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (3943, 'm;ql&*\8', 'Moreau', 'Paul', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (3916, '#+xejou:', 'Lefebvre', 'Isabelle', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (9558, '\c|D%>An', NULL, NULL, '50330030541557');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (1252, 'lB>`h9)"', NULL, NULL, '80654494216740');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (5814, 'U-n|!=Qy', NULL, NULL, '50434007721092');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (5831, '2TT{znKV', NULL, NULL, '16273402436509');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (4418, 'q*Qs2v<$', NULL, NULL, '12091511850336');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (9254, '5M7+s=oY', 'Schneider', 'Julien', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (7505, 'sLiEda!{', 'Fischer', 'Elise', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (1033, '(FAEb\AR', 'Weber', 'Thomas', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES (2789, 'X"*TLekj', NULL, NULL, '56317477012275');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('6183', 'password123', 'Roche', 'Frédéric', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('6455', 'securepass', 'Lefèvre', 'Philippe', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('6225', 'chefpass', 'Baudry', 'Lucie', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4368', 'cookpass', 'Poulain', 'Isaac', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('3184', 'clientpass', 'Boyer', 'Luc', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('5910', 'userpass', 'Fournier', 'Thomas', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4168', 'secure123', 'Brun', 'Margot', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8287', 'mypass', 'Bonneau', 'Louis', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4167', 'letmein', 'Morel', 'Anais' , NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8278', 'welcome', 'Regnier', 'Suzanne', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8743', 'password123', 'Georges', 'Susan', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8247', 'securepass', 'Da Silva', 'Robert', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('3423', 'chefpass', 'Thierry', 'Nicolas', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4058', 'cookpass', 'Wagner', 'Sophie', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('1510', 'clientpass', 'Roger', 'Dominique', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4755', 'userpass', 'Bernard', 'Tristan', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4162', 'secure123', 'Blondel', 'Anne', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8227', 'mypass', 'Bazin', 'Théodore', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('5032', 'letmein', 'Huet', 'Maggie', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('5127', 'welcome', 'Fontaine', 'Alain', NULL);
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('6180', 'password123', NULL, NULL, '00194627579614');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('6425', 'securepass', NULL, NULL, '50410902066985');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('6235', 'chefpass', NULL, NULL, '75918948277878');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4068', 'cookpass', NULL, NULL, '15575674421893');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('3154', 'clientpass', NULL, NULL, '42086885306941');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('5010', 'userpass', NULL, NULL, '71906462471656');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4160', 'secure123', NULL, NULL, '04527796024153');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('5287', 'mypass', NULL, NULL, '57586684034233');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('7868', 'letmein', NULL, NULL, '31354744055777');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8187', 'welcome', NULL, NULL, '68287167703647');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8543', 'password123', NULL, NULL, '27925070069231');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8047', 'securepass', NULL, NULL, '50412516543948');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('1523', 'chefpass', NULL, NULL, '67798923742406');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4558', 'cookpass', NULL, NULL, '57927783352507');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('1310', 'clientpass', NULL, NULL, '57518838229779');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4555', 'userpass', NULL, NULL, '90412516543948');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('4868', 'secure123', NULL, NULL, '72636964897378');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('8387', 'mypass', NULL, NULL, '78676484385935');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('5432', 'letmein', NULL, NULL, '54728694242483');
INSERT INTO Client (Id_client, Mdp, Nom_particulier, Prenom_particulier, SIRET_entreprise) VALUES ('5927', 'welcome', NULL, NULL, '25490269184448');

    
    -- Peuplement Cuisinier
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (8193, '}''<q9?0n', 'Dupont', 'Jean');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (2412, 'V&s%Pu-$', 'Martin', 'Sophie');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (1494, 'xx{_QsTe', 'Bernard', 'Michel');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (8091, '^r*VS\zi', 'Petit', 'Claire');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (3594, 'J#Iwof2e', 'Moreau', 'Paul');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (1015, 'N$0I0qhP', 'Lefebvre', 'Isabelle');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (6695, ',^OUje;M', 'Garcia', 'Luc');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (1887, 'txOS$-0f', 'Lopez', 'Marie');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (1495, '&X+WDlP-', 'Gonzalez', 'Nicolas');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (3170, 'M6f@\Y^k', 'Rodriguez', 'Camille');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (1195, '%ek`"<t4', 'Hernandez', 'Antoine');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (2999, 'vGP8REl[', 'Schneider', 'Julien');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (4358, 'n=H"}4T[', 'Fischer', 'Elise');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (3430, '8%.+\Z?d', 'Weber', 'Thomas');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES (4987, '.T^rl^r{', 'Muller', 'Laurence');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('8474', 'chefpass', 'Roche', 'Frédéric');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('9392', 'cookpass', 'Lefèvre', 'Philippe');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('1587', 'kitchenpass', 'Baudry', 'Lucie');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('4053', 'foodpass', 'Poulain', 'Isaac');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('9536', 'dishpass', 'Boyer', 'Luc');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('1467', 'mealpass', 'Fournier', 'Thomas');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('8985', 'tastepass', 'Brun', 'Margot');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('4825', 'yummypass', 'Bonneau', 'Louis');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('7399', 'delishpass', 'Morel', 'Anaïs');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('4953', 'bakepass', 'Regnier', 'Suzanne');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('5266', 'chefpass', 'Georges', 'Susan');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('9308', 'cookpass', 'Da Silva', 'Robert');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('7217', 'kitchenpass', 'Thierry', 'Nicolas');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('2472', 'foodpass', 'Wagner', 'Sophie');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('6303', 'dishpass', 'Roger', 'Dominique');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('8992', 'mealpass', 'Bernard', 'Tristan');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('5392', 'tastepass', 'Blondel', 'Anne');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('7983', 'yummypass', 'Bazin', 'Théodore');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('9734', 'delishpass', 'Huet', 'Maggie');
INSERT INTO Cuisinier (Id_cuisinier, Mdp, Nom_cuisinier, Prenom_cuisinier) VALUES ('1292', 'bakepass', 'Fontaine', 'Alain');


    -- Peuplement Plat
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (1632, 'Sushi', '33', '1', '8.51', '2025-02-24', '2025-02-27', 'Japonaise', 'Équilibré');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (1587, 'Feijoada', '44', '2', '13.53', '2025-02-26', '2025-03-01', 'Brésilienne', 'Sans gluten');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (8652, 'Couscous', '26', '6', '11.45', '2025-02-24', '2025-03-03', 'Maghrébine', 'Classique');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (4563, 'Pizza Margherita', '22', '1', '10.93', '2025-02-28', '2025-03-01', 'Italienne', 'Sans lactose');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (2589, 'Pad Thaï', '31', '1', '5.21', '2025-02-25', '2025-02-27', 'Thaïlandaise', 'Classique');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (6547, 'Tacos', '44', '1', '22.64', '2025-02-26', '2025-03-01', 'Mexicaine', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (8965, 'Ratatouille', '30', '6', '19.07', '2025-02-26', '2025-03-03', 'Française', 'Casher');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (3214, 'Chili con carne', '18', '4', '11.91', '2025-03-01', '2025-03-07', 'Tex-Mex', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (5046, 'Biryani', '28', '1', '22.79', '2025-02-25', '2025-03-01', 'Indienne', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (3058, 'Moussaka', '26', '3', '15.47', '2025-02-25', '2025-02-26', 'Grecque', 'Sans lactose');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (4205, 'Falafel', '7', '6', '16.2', '2025-02-27', '2025-02-28', 'Moyen-Orient', 'Bio');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (7604, 'Burger végétarien', '45', '5', '27.25', '2025-02-25', '2025-02-26', 'Végétarienne', 'Vegan');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (6950, 'Saumon Teriyaki', '12', '5', '25.6', '2025-02-28', '2025-03-07', 'Asiatique', 'Sans gluten');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (3210, 'Paella', '16', '5', '22.66', '2025-02-27', '2025-03-02', 'Espagnole', 'Halal');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES (8543, 'Goulash', '13', '3', '15.81', '2025-02-28', '2025-03-05', 'Hongroise', 'Classique');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('6183', 'Pizza', 20, 1, 12.50, '2023-10-01', '2023-10-05', 'Italienne', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('6455', 'Salade', 15, 2, 8.00, '2023-09-25', '2023-10-02', 'Française', 'Vegan');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('6225', 'Burger', 10, 1, 15.00, '2023-10-03', '2023-10-08', 'Américaine', 'Sans gluten');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('4368', 'Pâtes', 25, 2, 10.50, '2023-09-30', '2023-10-07', 'Italienne', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('3184', 'Sushi', 30, 1, 18.00, '2023-10-02', '2023-10-06', 'Japonaise', 'Sans gluten');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('5910', 'Tacos', 12, 1, 11.50, '2023-10-04', '2023-10-09', 'Mexicaine', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('7104', 'Quiche', 8, 4, 22.00, '2023-09-28', '2023-10-05', 'Française', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('5008', 'Soupe', 35, 2, 7.50, '2023-10-01', '2023-10-04', 'Française', 'Vegan');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('2118', 'Wrap', 18, 1, 9.00, '2023-09-29', '2023-10-06', 'Américaine', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('5058', 'Risotto', 22, 2, 16.50, '2023-10-03', '2023-10-07', 'Italienne', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('4011', 'Pizza', 20, 1, 12.50, '2023-10-01', '2023-10-05', 'Italienne', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('2472', 'Salade', 15, 2, 8.00, '2023-09-25', '2023-10-02', 'Française', 'Vegan');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('9239', 'Burger', 10, 1, 15.00, '2023-10-03', '2023-10-08', 'Américaine', 'Sans gluten');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('4058', 'Pâtes', 25, 2, 10.50, '2023-09-30', '2023-10-07', 'Italienne', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('2658', 'Sushi', 30, 1, 18.00, '2023-10-02', '2023-10-06', 'Japonaise', 'Sans gluten');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('9063', 'Tacos', 12, 1, 11.50, '2023-10-04', '2023-10-09', 'Mexicaine', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('7154', 'Quiche', 8, 4, 22.00, '2023-09-28', '2023-10-05', 'Française', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('9221', 'Soupe', 35, 2, 7.50, '2023-10-01', '2023-10-04', 'Française', 'Vegan');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('2918', 'Wrap', 18, 1, 9.00, '2023-09-29', '2023-10-06', 'Américaine', 'Végétarien');
INSERT INTO Plat (Id_plat, Type_de_plat, Stock, Nb_personnes, Prix, Date_fabrication, Date_peremption, Type_de_cuisine, Regime_alimentaire) VALUES ('1292', 'Risotto', 22, 2, 16.50, '2023-10-03', '2023-10-07', 'Italienne', 'Végétarien');


    -- Peuplement Ingrédients
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (371, 'Farine', 487);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (721, 'Sucre', 123);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (872, 'Beurre', 58);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (154, 'Lait', 381);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (951, 'Œufs', 226);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (707, 'Sel', 386);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (435, 'Poivre', 93);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (312, 'Tomates', 281);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (648, 'Poulet', 304);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (143, 'Pâtes', 136);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (578, 'Riz', 435);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (577, 'Fromage', 185);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (353, 'Oignons', 261);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (732, 'Ail', 64);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES (348, 'Persil', 109);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('6183', 'Tomate', 50);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('6455', 'Mozzarella', 30);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('6225', 'Laitue', 40);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('4368', 'Poulet', 25);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('3184', 'Saumon', 35);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('5910', 'Riz', 50);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('7104', 'Carotte', 20);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('5008', 'Pomme de terre', 45);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('2118', 'Oignon', 30);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('5048', 'Ail', 15);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('4011', 'Tomate', 50);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('2472', 'Mozzarella', 30);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('9239', 'Laitue', 40);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('4058', 'Poulet', 25);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('2658', 'Saumon', 35);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('9063', 'Riz', 50);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('7174', 'Carotte', 20);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('9221', 'Pomme de terre', 45);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('2108', 'Oignon', 30);
INSERT INTO Ingredients (Id_ingredient, Nom, Quantite) VALUES ('1292', 'Ail', 15);

    -- Peuplement Commande
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (7008, '428.74', 'En préparation', '2025-03-03 06:23:26', 8189);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (6287, '448.83', 'En préparation', '2025-03-02 20:23:26', 1499);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (7364, '242.26', 'Traitement de la commande', '2025-03-01 20:23:26', 4091);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (4213, '157.49', 'Traitement de la commande', '2025-03-02 05:23:26', 7796);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (5210, '354.37', 'En cours de livraison', '2025-03-02 15:23:26', 3943);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (7593, '178.22', 'En cours de livraison', '2025-03-03 00:23:26', 3916);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (1074, '265.8', 'En préparation', '2025-03-02 17:23:26', 9558);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (8626, '205.5', 'Traitement de la commande', '2025-03-02 08:23:26', 1252);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (2480, '220.4', 'Traitement de la commande', '2025-03-02 22:23:26', 5814);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (2404, '458.63', 'Traitement de la commande', '2025-03-03 01:23:26', 5831);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (2315, '409.33', 'En cours de livraison', '2025-03-02 14:23:26', 4418);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (4528, '357.89', 'Traitement de la commande', '2025-03-02 13:23:26', 9254);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (1385, '103.35', 'En cours de livraison', '2025-03-02 18:23:26', 7505);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (1696, '396.19', 'En cours de livraison', '2025-03-02 10:23:26', 1033);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES (7899, '103.51', 'En cours de livraison', '2025-03-02 17:23:26', 2789);
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('6183', 25.00, 'En préparation', '2023-10-01 18:30:00', '6183');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('6455', 30.50, 'En livraison', '2023-10-02 19:00:00', '6455');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('6225', 15.75, 'Livrée', '2023-10-03 20:15:00', '6225');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('4368', 45.20, 'En préparation', '2023-10-04 18:45:00', '4368');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('3184', 12.30, 'En livraison', '2023-10-05 19:30:00', '3184');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('3222', 30.47, 'En préparation', '2024-01-22 06:58:07', '4168');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('2255', 22.15, 'En préparation', '2024-02-28 03:58:36', '8287');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('4690', 46.80, 'En livraison', '2024-10-02 16:28:12', '4168');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('3587', 35.84, 'Livrée', '2024-04-24 12:47:28', '8287');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('1164', 11.68, 'En préparation', '2024-08-16 23:21:12', '4168');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('4211', 25.00, 'En préparation', '2023-10-01 18:30:00', '8743');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('2872', 30.50, 'En livraison', '2023-10-02 19:00:00', '8247');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('9939', 15.75, 'Livrée', '2023-10-03 20:15:00', '3423');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('4158', 45.20, 'En préparation', '2023-10-04 18:45:00', '1510');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('2658', 12.30, 'En livraison', '2023-10-05 19:30:00', '4755');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('3202', 30.47, 'En préparation', '2024-01-22 06:58:07', '4168');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('2215', 22.15, 'En préparation', '2024-02-28 03:58:36', '8287');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('4680', 46.80, 'En livraison', '2024-10-02 16:28:12', '5032');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('3584', 35.84, 'Livrée', '2024-04-24 12:47:28', '5127');
INSERT INTO Commande (Id_commande, Prix, Statut, Prevision_arrivee, Id_client) VALUES ('1168', 11.68, 'En préparation', '2024-08-16 23:21:12', '4168');


    -- Peuplement Livraison
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('12', 'Rue de Rivoli', '75001', 'Paris', 7008);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('45', 'Avenue des Champs-Élysées', '75008', 'Paris', 6287);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('78', 'Boulevard Haussmann', '75009', 'Paris', 7364);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('23', 'Rue Saint-Honoré', '75001', 'Paris', 4213);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('56', 'Avenue Foch', '75116', 'Paris', 5210);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('34', 'Rue Lafayette', '75009', 'Paris', 7593);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('89', 'Boulevard Saint-Michel', '75005', 'Paris', 1074);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('67', 'Rue de la Paix', '75002', 'Paris', 8626);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('11', 'Avenue de l''Opéra', '75001', 'Paris', 2480);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('90', 'Rue du Faubourg Saint-Honoré', '75008', 'Paris', 2404);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('55', 'Boulevard Malesherbes', '75008', 'Paris', 2315);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('42', 'Rue de Turenne', '75003', 'Paris', 4528);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('31', 'Boulevard de Sébastopol', '75002', 'Paris', 1385);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('77', 'Avenue Victor Hugo', '75116', 'Paris', 1696);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES ('25', 'Rue Réaumur', '75002', 'Paris', 7899);
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (76, 'avenue Vincent Gauthier', '09847', 'Paris', '6183');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (97, 'rue Simon', '56150', 'Paris', '6455');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (35, 'chemin Gérard Foucher', '33298', 'Paris', '6225');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (20, 'chemin de Dos Santos', '94517', 'Paris', '4368');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (61, 'chemin de Vaillant', '85727', 'Paris', '3184');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (13, 'rue Poirier', '07916', 'Paris', '3202');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (54, 'chemin de Pinto', '76984', 'Paris', '2215');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (17, 'rue Benjamin Regnier', '20366', 'Paris', '4680');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (62, 'rue William Fernandes', '52936', 'Paris', '3584');
INSERT INTO Livraison (Num_Rue, Rue, CP, Ville, Id_commande) VALUES (92, 'chemin Duval', '95903', 'Paris', '1168');

    -- Peuplement Avis
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (1284, '3', 'Délicieux, j''ai adoré !', 'Dupont', 'Jean', '21619009196044', 1632, 8189, 8193);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (3952, '4', 'Très bon, je recommande !', 'Martin', 'Sophie', '16593760297582', 1587, 1499, 2412);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (7461, '2', 'Mauvaise expérience, plat froid.', 'Bernard', 'Michel', '16728799614726', 8652, 4091, 1494);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (2083, '4', 'Très bon, je recommande !', 'Petit', 'Claire', '66845121871244', 4563, 7796, 8091);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (5197, '2', 'Mauvaise expérience, plat froid.', 'Moreau', 'Paul', '71545421471430', 2589, 3943, 3594);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (8642, '5', 'Excellent plat, très savoureux !', 'Lefebvre', 'Isabelle', '77985164597385', 6547, 3916, 1015);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (3025, '1', 'Très déçu, ne vaut pas le prix.', 'Garcia', 'Luc', '50330030541557', 8965, 9558, 6695);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (6719, '5', 'Excellent plat, très savoureux !', 'Lopez', 'Marie', '80654494216740', 3214, 1252, 1887);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (4508, '3', 'Délicieux, j''ai adoré !', 'Gonzalez', 'Nicolas', '50434007721092', 5046, 5814, 1495);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (9376, '3', 'Délicieux, j''ai adoré !', 'Rodriguez', 'Camille', '16273402436509', 3058, 5831, 3170);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (2851, '2', 'Mauvaise expérience, plat froid.', 'Hernandez', 'Antoine', '12091511850336', 4205, 4418, 1195);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (6734, '5', 'Excellent plat, très savoureux !', 'Schneider', 'Julien', '35736502840212', 7604, 9254, 2999);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (5298, '3', 'Délicieux, j''ai adoré !', 'Fischer', 'Elise', '63063409867949', 6950, 7505, 4358);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (8147, '2', 'Mauvaise expérience, plat froid.', 'Weber', 'Thomas', '10575439913150', 3210, 1033, 3430);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES (3620, '1', 'Très déçu, ne vaut pas le prix.', 'Muller', 'Laurence', '56317477012275', 8543, 2789, 4987);
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES ('3202', 1.4, 'Le plat était froid à l\'arrivée.', 'Roche', 'Frédéric', '00194627579614', '6183', '6183', '8474');
INSERT INTO Avis (Id_avis, Note, Commentaire, Nom, Prenom, SIRET, Id_plat, Id_client, Id_cuisinier) VALUES ('1751', 4.7, 'Excellent service, plat délicieux!', 'Lefèvre', 'Philippe', '50410902066985', '6225', '6455', '9392');


