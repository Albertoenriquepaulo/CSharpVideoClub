using VideoClub.Class;
using VideoClub.Class.Helpers;
using System;
using System.Drawing;
using Console = Colorful.Console;
using VideoClub.Class.Tables;

namespace VideoClub
{
    class Program
    {
        public static SQLDBConnection myDB;
        static void Main(string[] args)
        {
            ConfigFile myConfigFile = new ConfigFile();
            myDB = new SQLDBConnection(myConfigFile.GetKeyValue("Data Source"), myConfigFile.GetKeyValue("Catalog"),
                                     Convert.ToBoolean(myConfigFile.GetKeyValue("Integrated Security")));

            Client myActiveClient;// = new Client() { ID_Client = 10, Name = "Alberto", LastName = "Paulo", Birthdate = Convert.ToDateTime("29/01/2007"), email = "Albertopaulo@gmail.com", pass = "123456" };
            int menuOp;
            string strLogInOut = "LOGOUT";
            Console.ResetColor();
            string[] userAndPass = Menu.PrintLogInMenu(myDB);
            if (userAndPass[2] == "true")
            {
                myActiveClient = HpClients.LoadClient(myDB, userAndPass[0]);

                do
                {
                    menuOp = Menu.PrintMainMenu(strLogInOut, myActiveClient);
                    if (menuOp == 4 && strLogInOut == "LOGIN")
                    {
                        userAndPass = Menu.PrintLogInMenu(myDB);
                        if (userAndPass[2] == "true")
                        {
                            strLogInOut = "LOGOUT";
                            myActiveClient = HpClients.LoadClient(myDB, userAndPass[0]);
                            menuOp = Menu.PrintMainMenu(strLogInOut, myActiveClient);
                        }
                    }
                    switch (menuOp)
                    {
                        case (int)Menu.MainOp.availableMovies:
                            if (strLogInOut == "LOGOUT")
                            {
                                HpMovies.ShowMoviesInTableAccordingAge(myDB, myActiveClient);
                                Menu.WriteContinue();
                            }
                            else
                                Menu.WriteNoLog();
                            break;
                        case (int)Menu.MainOp.rent:
                            if (strLogInOut == "LOGOUT")
                                do
                                {
                                    menuOp = Menu.PrintMenuOp2();
                                    switch (menuOp)
                                    {
                                        case (int)Menu.MainOp2.showMovies:

                                            HpMovies.ShowMoviesInTableAccordingAge(myDB, myActiveClient, 1);
                                            Menu.WriteContinue();
                                            break;
                                        case (int)Menu.MainOp2.rent: //Alquila Movie
                                            HpMovies.ShowMoviesInTableAccordingAge(myDB, myActiveClient, 1);
                                            int ID_Movie = Menu.GetOpNumberFromUser("ID Pelicula", "0123456789", 4);
                                            double dblNroDaysToRent = Menu.GetOpNumberFromUser("Numero de Dias", "0123456789", 2);
                                            DateTime c_Out = DateTime.Today.AddDays(dblNroDaysToRent);
                                            HpMovies.RentMovie(myDB, myActiveClient, ID_Movie, DateTime.Today, c_Out);
                                            break;
                                        case (int)Menu.MainOp2.returnMovie:
                                            HpMovies.ShowMoviesInTableRentedByClient(myDB, myActiveClient);
                                            if (strLogInOut == "LOGOUT")
                                            {
                                                ID_Movie = Menu.GetOpNumberFromUser("ID Pelicula", "0123456789", 4);
                                                HpMovies.DeleteMovie(myDB, myActiveClient, ID_Movie);
                                            }
                                            else
                                                Menu.WriteNoLog();
                                            break;
                                        default:
                                            if (menuOp > 4)
                                            {
                                                Console.WriteLine("\nOpción no disponible...\n\n", Color.Blue);
                                                Menu.WriteContinue();
                                                menuOp = 4;
                                            }

                                            break;
                                    }
                                } while (menuOp < 4);
                            else
                                Menu.WriteNoLog();
                            break;
                        case (int)Menu.MainOp.myRents:
                            if (strLogInOut == "LOGOUT")
                            {
                                HpMovies.ShowMoviesInTableRentedByClient(myDB, myActiveClient);
                                Menu.WriteContinue();
                            }
                            else
                                Menu.WriteNoLog();
                            break;
                        case (int)Menu.MainOp.logOutIN:
                            if (strLogInOut == "LOGOUT")
                            {
                                myActiveClient.Clear();
                                strLogInOut = "LOGIN";
                                Console.WriteLine("\nLogout realizado con éxito...\n\n", Color.Blue);
                                Menu.WriteContinue();
                            }
                            break;
                        default:
                            if (menuOp > 5)
                            {
                                Console.WriteLine("\nOpción no disponible...\n\n", Color.Blue);
                                Menu.WriteContinue();
                                menuOp = 5;
                            }
                            break;
                    }
                } while (menuOp < 5);
                Console.Write("\n\nGood Bye Dude...!!!\n\n\n", Color.Azure);
            }
        }
    }
}
