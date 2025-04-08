CREATE DATABASE IF NOT EXISTS Psi;
--  DROP DATABASE IF EXISTS Psi;
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
   Photo BINARY,
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
	
    -- Particulier
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


	-- Entreprise
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


    -- Client
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

    
    -- Cuisinier
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


    -- Plat
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


    -- Ingrédients
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


    -- Commande
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


    -- Livraison
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


    -- Avis
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
