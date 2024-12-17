--if your database is not empty, you can use this code in comments to empty it

--TRUNCATE TABLE AnnoncesTags;
--TRUNCATE TABLE AnnoncesGeoSectors;
--TRUNCATE TABLE Comments;
--TRUNCATE TABLE Transactions;
--TRUNCATE TABLE ContactMessages;
--TRUNCATE TABLE AspNetUserRoles;
--DELETE FROM AspNetRoles;
--DBCC CHECKIDENT ('AspNetRoles', RESEED, 0);
--DELETE FROM Tags;
--DBCC CHECKIDENT ('Tags', RESEED, 0);
--DELETE FROM GeographicalSectors;
--DBCC CHECKIDENT ('GeographicalSectors', RESEED, 0);
--DELETE FROM Annonces;
--DBCC CHECKIDENT ('Annonces', RESEED, 0);
--DELETE FROM AspNetUsers;
--DBCC CHECKIDENT ('AspNetUsers', RESEED, 0);


-- Table AspNetUsers
-- insert with Program.cs

--Table Tags
insert into Tags (CategoryName) values ('Housework');
insert into Tags (CategoryName) values ('Gardening');
insert into Tags (CategoryName) values ('Cooking');
insert into Tags (CategoryName) values ('DIY');
insert into Tags (CategoryName) values ('Shopping');
insert into Tags (CategoryName) values ('Transport');
insert into Tags (CategoryName) values ('Moving');
insert into Tags (CategoryName) values ('Child care');

--Table GeographicalSectors
insert into GeographicalSectors (Place) values ('Grenoble');
insert into GeographicalSectors (Place) values ('Lyon');
insert into GeographicalSectors (Place) values ('Paris');
insert into GeographicalSectors (Place) values ('Marseille');
insert into GeographicalSectors (Place) values ('Toulouse');
insert into GeographicalSectors (Place) values ('Bordeaux');
insert into GeographicalSectors (Place) values ('Nantes');
insert into GeographicalSectors (Place) values ('Lille');

--Table Annonce
insert into Annonces (Titre, Description, Points, Date, Active, IdUser) values ('Housework', 'I need help with housework', 10, GETDATE(), 'True', 2);
insert into Annonces (Titre, Description, Points, Date, Active, IdUser) values ('Gardening', 'I need help with gardening', 10, GETDATE(), 'True', 3);
insert into Annonces (Titre, Description, Points, Date, Active, IdUser) values ('Cooking', 'I need help with cooking', 10, GETDATE(), 'True', 4);
insert into Annonces (Titre, Description, Points, Date, Active, IdUser) values ('DIY', 'I need help with DIY', 10, GETDATE(), 'True', 5);

--Table AnnoncesTags
insert into AnnoncesTags (IdTag, IdAnnonce) values (1, 1);
insert into AnnoncesTags (IdTag, IdAnnonce) values (2, 2);
insert into AnnoncesTags (IdTag, IdAnnonce) values (3, 3);
insert into AnnoncesTags (IdTag, IdAnnonce) values (4, 4);


--Table AnnoncesGeographicalSectors
insert into AnnoncesGeoSectors (IdGeographicalSector, IdAnnonce) values (1, 1);
insert into AnnoncesGeoSectors (IdGeographicalSector, IdAnnonce) values (2, 2);
insert into AnnoncesGeoSectors (IdGeographicalSector, IdAnnonce) values (3, 3);
insert into AnnoncesGeoSectors (IdGeographicalSector, IdAnnonce) values (4, 4);

--Table Transactions
insert into Transactions (UserIdBuyer, UserIdSeller, IdAnnonce, DateTransaction, Status) values (1, 2, 1, GETDATE()+1, 'Pending');
insert into Transactions (UserIdBuyer, UserIdSeller, IdAnnonce, DateTransaction, Status) values (2, 3, 2, GETDATE()+2, 'Pending');
insert into Transactions (UserIdBuyer, UserIdSeller, IdAnnonce, DateTransaction, Status) values (3, 4, 3, GETDATE()+3, 'Pending');
insert into Transactions (UserIdBuyer, UserIdSeller, IdAnnonce, DateTransaction, Status) values (4, 5, 4, GETDATE()+4, 'Pending');

--Table Comments
insert into Comments (Date, Notice, IdUserGiven, IdUserRecipient) values (GETDATE()+2, 'Good job', 1, 2);
insert into Comments (Date, Notice, IdUserGiven, IdUserRecipient) values (GETDATE()+3, 'Good job', 2, 3);
insert into Comments (Date, Notice, IdUserGiven, IdUserRecipient) values (GETDATE()+4, 'Good job', 3, 4);
insert into Comments (Date, Notice, IdUserGiven, IdUserRecipient) values (GETDATE()+5, 'Good job', 4, 5);

--Table ContactMessages
insert into ContactMessages (Subject, Message, DateSent, IsRead, UserId) values ('Hello', 'Hello, I need help', GETDATE(), 'False', 1);
insert into ContactMessages (Subject, Message, DateSent, IsRead, UserId) values ('Hello', 'Hello, I need help', GETDATE(), 'False', 2);
insert into ContactMessages (Subject, Message, DateSent, IsRead, UserId) values ('Hello', 'Hello, I need help', GETDATE(), 'False', 3);
insert into ContactMessages (Subject, Message, DateSent, IsRead, UserId) values ('Hello', 'Hello, I need help', GETDATE(), 'False', 4);




