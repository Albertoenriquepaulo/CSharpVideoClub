using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using VideoClub.Class.Tables;
using Console = Colorful.Console;

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
            System.Console.ResetColor();
            Console.WriteLine($"\nMOSTRANDO TUS PELICULAS ALQUILADAS\n");
            HpVarious.ShowProgressBar(10, 100);

            var table = new ConsoleTable("ID", "Title", "Fecha Alquilada", "Fecha Devolucion", "Tiempo de Entrega");
            // SELECT M.Title FROM Clients C, Rented R, Movies M WHERE C.ID_Client = 1 AND R.ID_Client = 1 AND M.ID_Movie = R.ID_Movie
            dTable = RUDI.Read(myDB, "Clients C, Rented R, Movies M", $"R.ID_Movie, M.Title, R.C_In, R.C_Out", $"C.ID_Client = {cToCompare.ID_Client} AND R.ID_Client = {cToCompare.ID_Client} AND M.ID_Movie = R.ID_Movie");
            dTable.Columns.Add("Caducado", typeof(String));
            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dTable.Rows)
                {
                    string[] strInfoToPrint = new string[5];
                    int i = 0;
                    foreach (var item in dataRow.ItemArray)
                    {
                        if (i < 2)
                        {
                            strInfoToPrint[i++] = item.ToString();
                        }
                        else if (i == 4)
                            if (Convert.ToDateTime(strInfoToPrint[3]) < DateTime.Today)
                                strInfoToPrint[i++] = "Caducado";
                            else
                                strInfoToPrint[i++] = "En Plazo";
                        else
                        {
                            strInfoToPrint[i++] = item.ToString().Substring(0, 10); ;
                        }

                    }
                    table.AddRow(strInfoToPrint);
                }
            }
            ConsoleTableOptions o;
            Console.ResetColor();
            table.Write();

        }

        //Si includeStatus = 1 se construye la query con el status = 1, las pelis disponibles
        //includeStatus = 0 se construye la query con el status = 0, las pelis NO disponibles
        //includeStatus = 2 se construye la query sin considerar status, las pelis disponibles y No disponibles
        public static void ShowMoviesInTableAccordingAge(SQLDBConnection myDB, Client cToCompare, int includeStatus = 2)
        {
            DataTable dTable;
            Console.WriteLine($"\nMOSTRANDO CATALOGO DISPONIBLE\n");
            HpVarious.ShowProgressBar(5, 100);

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
            Console.ResetColor();
            table.Write();
        }

        public static bool RentMovie(SQLDBConnection myDB, Client cWillRent, int ID_Movie, DateTime c_In, DateTime c_Out)
        {
            Console.WriteLine($"\nPROCESANDO ALQUILER DE PELICULA DISPONIBLE\n");
            HpVarious.ShowProgressBar(10, 100);
            //var table = new ConsoleTable("ID", "Title", "Synopsis");

            if (CanCLientRentTheMovie(myDB, ID_Movie, cWillRent)) //Si existe la peli y si esta disponible
            {
                if (RUDI.Insert(myDB, "Rented", "ID_Client, ID_Movie, C_In, C_Out", $"{cWillRent.ID_Client}, {ID_Movie}, '{c_In.ToString("MM/dd/yyyy")}', '{c_Out.ToString("MM/dd/yyyy")}'") == 1) //Es porque actualizó
                { //UPDATE campo State of This Movie
                    if (RUDI.Update(myDB, "Movies", "State=0", $"ID_Movie={ID_Movie}") == 1)
                    {
                        DataTable dTable = RUDI.Read(myDB, "Rented", "ID_Rented", $"ID_Movie={ID_Movie} AND ID_Client={cWillRent.ID_Client}");
                        Console.WriteLine($"\nEl alquiler se ha procesado con éxito, bajo el ID: {dTable.Rows[0].Field<int>(0)}", Color.Blue);
                        Menu.WriteContinue();
                        return true;
                    }
                }
                return false;
            }
            else
            {
                Console.WriteLine("Error. Pelicula no Disponible o No tiene la edad suficiente para alquilar la pelicula", Color.Red);
                System.Console.ResetColor();
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

        public static bool CanMovieBeRemove(SQLDBConnection myDB, int ID_Movie)
        {
            DataTable dTable;
            if (MovieExist(myDB, ID_Movie))
            {
                dTable = RUDI.Read(myDB, "Movies", "ID_Movie", $"ID_Movie = {ID_Movie} AND State = 0");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
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

        public static bool DeleteMovie(SQLDBConnection myDB, Client clientForRemoveMovie, int ID_Movie)
        {
            Console.WriteLine($"\nPROCESANDO DEVOLUCIÓN DE PELICULA\n");
            HpVarious.ShowProgressBar(10, 100);
            //var table = new ConsoleTable("ID", "Title", "Synopsis");

            if (CanMovieBeRemove(myDB, ID_Movie)) //Si existe la peli y si No esta disponible, es que esta alquilada y puede ser removida
            {
                if (RUDI.Delete(myDB, "Rented", $"ID_Movie = {ID_Movie}") == 1)
                { //UPDATE campo State of This Movie
                    if (RUDI.Update(myDB, "Movies", "State=1", $"ID_Movie={ID_Movie}") == 1)
                    {
                        DataTable dTable = RUDI.Read(myDB, "Rented", "ID_Rented", $"ID_Movie={ID_Movie} AND ID_Client={clientForRemoveMovie.ID_Client}");
                        Console.WriteLine($"\nLa pelicula bajo el ID: {ID_Movie} ha sido removida con exito, ahora se encuentra disponible", Color.Blue);
                        Menu.WriteContinue();
                        return true;
                    }
                }
                return false;
            }
            else
            {
                Console.WriteLine("Error. No existe la pelicula o no ha sido alquilada bajo su usuario", Color.Red);
            }
            Menu.WriteContinue();
            return false;
        }
    }
}
