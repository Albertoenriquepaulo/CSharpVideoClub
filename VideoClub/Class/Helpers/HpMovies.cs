using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using VideoClub.Class.Tables;

namespace VideoClub.Class.Helpers
{
    class HpMovies
    {
        //Si includeStatus = 1 se construye la query con el status = 1, las pelis disponibles
        //includeStatus = 0 se construye la query con el status = 0, las pelis NO disponibles
        //includeStatus = 2 se construye la query sin considerar status, las pelis disponibles y No disponibles
        public static void ShowMoviesInTableRentedByClient(SQLDBConnection myDB, Client cToCompare)
        {
            DataTable dTable;
            Console.WriteLine($"\nMOSTRANDO PELICULAS ALQUILADAS\n");
            HpVarious.ShowProgressBar(10, 100);

            var table = new ConsoleTable("Title");
            // SELECT M.Title FROM Clients C, Rented R, Movies M WHERE C.ID_Client = 1 AND R.ID_Client = 1 AND M.ID_Movie = R.ID_Movie
            dTable = RUDI.Read(myDB, "Clients C, Rented R, Movies M", $"M.Title", $"C.ID_Client = {cToCompare.ID_Client} AND R.ID_Client = {cToCompare.ID_Client} AND M.ID_Movie = R.ID_Movie");

            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dTable.Rows)
                {
                    string[] strInfoToPrint = new string[1];
                    int i = 0;
                    foreach (var item in dataRow.ItemArray)
                    {
                        strInfoToPrint[i++] = item.ToString();
                    }
                    table.AddRow(strInfoToPrint);
                }
            }
            ConsoleTableOptions o;

            table.Write();
            Menu.WriteContinue();
        }

        //Si includeStatus = 1 se construye la query con el status = 1, las pelis disponibles
        //includeStatus = 0 se construye la query con el status = 0, las pelis NO disponibles
        //includeStatus = 2 se construye la query sin considerar status, las pelis disponibles y No disponibles
        public static void ShowMoviesInTableAccordingAge(SQLDBConnection myDB, Client cToCompare, int includeStatus = 2)
        {
            DataTable dTable;
            Console.WriteLine($"\nMOSTRANDO CATALOGO\n");
            HpVarious.ShowProgressBar(10, 100);

            var table = new ConsoleTable("ID", "Title", "Synopsis");
            if (includeStatus == 2)
                dTable = RUDI.Read(myDB, "Movies", $"ID_Movie, Title, Synopsis", $"RecommendedAge <= {HpVarious.GetAges(cToCompare.Birthdate)}");

            else if (includeStatus == 1)
                dTable = RUDI.Read(myDB, "Movies", $"ID_Movie, Title, Synopsis", $"State = 1 AND RecommendedAge <= {HpVarious.GetAges(cToCompare.Birthdate)}");
            else
                dTable = RUDI.Read(myDB, "Movies", $"ID_Movie, Title, Synopsis", $"State = 0 AND RecommendedAge <= {HpVarious.GetAges(cToCompare.Birthdate)}");

            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dTable.Rows)
                {
                    string[] strInfoToPrint = new string[3];
                    int i = 0;
                    foreach (var item in dataRow.ItemArray)
                    {
                        strInfoToPrint[i++] = item.ToString();
                    }
                    table.AddRow(strInfoToPrint);
                }
            }
            table.Write();
            Menu.WriteContinue();
        }

        public static bool RentMovie(SQLDBConnection myDB, Client cWillRent, int ID_Movie)
        {
            Console.WriteLine($"PROCESANDO ALQUILER DE PELICULA DISPONIBLE\n");
            HpVarious.ShowProgressBar(10, 100);
            //var table = new ConsoleTable("ID", "Title", "Synopsis");

            if (CanCLientRentTheMovie(myDB, ID_Movie, cWillRent)) //Si existe la peli y si esta disponible
            {
                if (RUDI.Insert(myDB, "Rented", "ID_Client, ID_Movie", $"{cWillRent.ID_Client},{ID_Movie}") == 1) //Es porque actualizó
                { //UPDATE campo State of This Movie
                    if (RUDI.Update(myDB, "Movies", "State=0", $"ID_Movie={ID_Movie}") == 1)
                    {
                        DataTable dTable = RUDI.Read(myDB, "Rented", "ID_Rented", $"ID_Movie={ID_Movie} AND ID_Client={cWillRent.ID_Client}");
                        Console.WriteLine($"\nEl alquiler se ha procesado con éxito, bajo el ID_Rented: {dTable.Rows[0].Field<int>(0)}", Color.Blue);
                        Menu.WriteContinue();
                        return true;
                    }
                }
                return false;
            }
            else
            {
                Console.WriteLine("Error. Pelicula no Disponible o No tiene la edad suficiente para alquilar la pelicula", Color.Red);
            }
            Menu.WriteContinue();
            return false;
        }

        public static bool MovieExist(SQLDBConnection myDB, int ID_Movie)
        {
            DataTable dTable;
            if (ID_Movie > 0)
            {
                dTable = RUDI.Read(myDB, "Movies", "ID_Movie", $"ID_Movie = {ID_Movie}");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
                if (dTable != null && dTable.Rows.Count > 0)
                    return true;
            }
            return false;
        }

        public static bool IsMovieAvailable(SQLDBConnection myDB, int ID_Movie)
        {
            DataTable dTable;
            if (MovieExist(myDB, ID_Movie))
            {
                dTable = RUDI.Read(myDB, "Movies", "ID_Movie", $"ID_Movie = {ID_Movie} AND State = 1");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
                if (dTable != null && dTable.Rows.Count > 0)
                    return true;
            }
            return false;
        }

        // Si la pelicula existe, esta disponible y el cliente tiene la edad suficiente devolverá true
        public static bool CanCLientRentTheMovie(SQLDBConnection myDB, int ID_Movie, Client cWillRent)
        {
            DataTable dTable;
            if (MovieExist(myDB, ID_Movie))
            {
                dTable = RUDI.Read(myDB, "Movies", "ID_Movie", $"ID_Movie = {ID_Movie} AND State = 1 AND RecommendedAge <= {HpVarious.GetAges(cWillRent.Birthdate)}");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
                if (dTable != null && dTable.Rows.Count > 0)
                    return true;
            }
            return false;
        }
    }
}
