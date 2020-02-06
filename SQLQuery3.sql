SELECT * FROM Clients
SELECT * FROM Movies
SELECT * FROM Rented


--INSERT INTO Clients (DNI, Name, LastName, BirthDate, email, pass) VALUES ('444555666', 'Super', 'Boy', '04/13/2016', 'Superboy@gmail.com', '123')

UPDATE Rented
SET C_Out = '02/05/2020'
WHERE ID_Rented = 9
-- DELETE FROM Rented WHERE ID_Movie = 5
--INSERT INTO Rented (ID_Client, ID_Movie, C_In, C_Out) VALUES (3, 5, '02/06/2020', '02/10/2020')

INSERT INTO Movies (Title, RecommendedAge, Synopsis, State) VALUES ('T9', 3, 'Para menores de 3 años', 1)