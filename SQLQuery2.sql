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

INSERT INTO Clients VALUES ('123', 'Axl', 'Rose', '05/01/2004', 'email@email.com', '123');

--SELECT ID_Movie, Title, Synopsis FROM Movies WHERE RecommendedAge <= 41

--INSERT INTO Rented VALUES (1, 1);
--INSERT INTO Rented VALUES (1, 5);
--INSERT INTO Rented VALUES (1, 18);

--SELECT * FROM Movies
--SELECT * FROM Clients
SELECT * FROM Rented


SELECT Title, RecommendedAge FROM Movies WHERE Movies.RecommendedAge <= 41 AND Movies.State = 0; 

SELECT * FROM Clients

SELECT email FROM Clients WHERE DNI LIKE 'Y6534922S' AND email LIKE '123456'

SELECT * FROM Clients WHERE DNI LIKE 'y6534922s'
SELECT * FROM Movies
SELECT ID_Movie, Title, Synopsis FROM Movies WHERE State = 1 AND RecommendedAge <= 41
SELECT ID_Movie, Title, Synopsis FROM Movies WHERE State = 1 AND RecommendedAge <= 41

--UPDATE Movies SET State = 1
--DELETE FROM Rented
--DBCC CHECKIDENT (Rented, RESEED, 0)

SELECT * FROM Clients
SELECT * FROM Movies
SELECT * FROM Rented

SELECT M.Title FROM Clients C, Rented R, Movies M WHERE C.ID_Client = 1 AND R.ID_Client = 1 AND M.ID_Movie = R.ID_Movie
SELECT M.Title FROM Clients C, Rented R, Movies M WHERE C.ID_Client = 1 AND R.ID_Client = 1 AND M.ID_Movie = R.ID_Movie

SELECT ID_Movie FROM Movies WHERE ID_Movie=4 AND State=1

UPDATE Clients
SET DNI = 111222333
WHERE ID_Client = 3

UPDATE Clients
SET email = 'BruceDickinson@maiden.com'
WHERE ID_Client = 2