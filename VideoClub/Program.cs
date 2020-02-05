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

            Client myClientTester = new Client() { ID_Client = 10, Name = "Alberto", LastName = "Paulo", Birthdate = Convert.ToDateTime("29/01/2007"), email = "Albertopaulo@gmail.com", pass = "123456" };
            int menuOp = 0;
            Client currentClient = new Client();
            string strLogInOut = "LOGOUT";
            // Console.WriteLine("Hello World!");
            //HpClients.InsertClient(myDB, HpVarious.AskNewUserData(myDB));
            string[] userAndPass = Menu.PrintLogInMenu(myDB);
            if (userAndPass[2] == "true")
            {
                myClientTester = HpClients.LoadClient(myDB, userAndPass[0]);

                do
                {
                    menuOp = Menu.PrintMainMenu(strLogInOut);
                    if (menuOp == 4 && strLogInOut == "LOGIN")
                    {
                        userAndPass = Menu.PrintLogInMenu(myDB);
                        if (userAndPass[2] == "true")
                        {
                            strLogInOut = "LOGOUT";
                            myClientTester = HpClients.LoadClient(myDB, userAndPass[0]);
                            menuOp = Menu.PrintMainMenu(strLogInOut);
                        }
                    }
                    //else
                    //{
                    //    menuOp = Menu.PrintMainMenu(strLogInOut);
                    //}
                    //menuOp = Menu.PrintMainMenu(strLogInOut);
                    switch (menuOp)
                    {
                        case (int)Menu.MainOp.availableMovies:
                            if (strLogInOut == "LOGOUT")
                                HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester);
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

                                            HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, 1);

                                            break;
                                        case (int)Menu.MainOp2.rent: //Alquila Movie
                                            Console.Write("\nID Pelicula: ");
                                            int ID_Movie = Convert.ToInt32(HpVarious.ReadNumber("0123456789"));
                                            HpMovies.RentMovie(myDB, myClientTester, ID_Movie);
                                            //HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, true);
                                            break;
                                        default:
                                            if (menuOp > 3)
                                            {
                                                Console.WriteLine("\nOpción no disponible...\n\n", Color.Blue);
                                                Menu.WriteContinue();
                                                menuOp = 3;
                                            }

                                            break;
                                    }
                                } while (menuOp < 3);
                            else
                                Menu.WriteNoLog();
                            //HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, true);
                            break;
                        case (int)Menu.MainOp.myRents:
                            if (strLogInOut == "LOGOUT")
                                HpMovies.ShowMoviesInTableRentedByClient(myDB, myClientTester);
                            else
                                Menu.WriteNoLog();
                            break;
                        case (int)Menu.MainOp.logOutIN:
                            if (strLogInOut == "LOGOUT")
                            {
                                myClientTester.Clear();
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
                                menuOp = 4;
                            }
                            break;

                    }
                } while (menuOp < 5);
                Console.Write("\n\nGood Bye Dude...!!!\n\n\n", Color.Azure);
            }

        }
    }
}
