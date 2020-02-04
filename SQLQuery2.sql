--SELECT * FROM Clients

--INSERT INTO Clients (DNI, Name, LastName, BirthDate, email, passs) VALUES ('Y6534922S', 'Alberto', 'Paulo', '01/29/1979', 'Albertopaulo@gmail.com', '123456')

--INSERT INTO Movies VALUES ('T1', 18, 'S1');
--INSERT INTO Movies VALUES ('T2', 5, 'S2');
--INSERT INTO Movies VALUES ('T3', 13, 'S3');
--INSERT INTO Movies VALUES ('T4', 7, 'S4');
--INSERT INTO Movies VALUES ('T5', 16, 'S5');
--INSERT INTO Movies VALUES ('T6', 18, 'S6');
--INSERT INTO Movies VALUES ('T7', 5, 'S7');
--INSERT INTO Movies VALUES ('T8', 18, 'S8');

--SELECT * FROM Movies


--SELECT ID_Movie, Title, Synopsis FROM Movies WHERE RecommendedAge <= 41

--INSERT INTO Rented VALUES (1, 1);
--INSERT INTO Rented VALUES (1, 5);
--INSERT INTO Rented VALUES (1, 18);

--SELECT * FROM Movies
--SELECT * FROM Clients
SELECT * FROM Rented

SELECT * FROM Movies
SELECT Title, RecommendedAge FROM Movies WHERE Movies.RecommendedAge <= 41 AND Movies.State = 0; 

SELECT * FROM Clients

SELECT email FROM Clients WHERE DNI LIKE 'Y6534922S' AND email LIKE '123456'

SELECT * FROM Clients WHERE DNI LIKE 'y6534922s'
--UPDATE Movies SET State = 1