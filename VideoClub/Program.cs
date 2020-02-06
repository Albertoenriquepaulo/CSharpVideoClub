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
            Console.ResetColor();
            string[] userAndPass = Menu.PrintLogInMenu(myDB);
            if (userAndPass[2] == "true")
            {
                myClientTester = HpClients.LoadClient(myDB, userAndPass[0]);

                do
                {
                    menuOp = Menu.PrintMainMenu(strLogInOut, myClientTester);
                    if (menuOp == 4 && strLogInOut == "LOGIN")
                    {
                        userAndPass = Menu.PrintLogInMenu(myDB);
                        if (userAndPass[2] == "true")
                        {
                            strLogInOut = "LOGOUT";
                            myClientTester = HpClients.LoadClient(myDB, userAndPass[0]);
                            menuOp = Menu.PrintMainMenu(strLogInOut, myClientTester);
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
                            {
                                HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester);
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

                                            HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, 1);
                                            Menu.WriteContinue();
                                            break;
                                        case (int)Menu.MainOp2.rent: //Alquila Movie
                                            HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, 1);
                                            int ID_Movie = Menu.GetOpNumberFromUser("ID Pelicula", "0123456789", 4);
                                            //Console.Write("\nID Pelicula: ");
                                            //int ID_Movie = Convert.ToInt32(HpVarious.ReadNumber("0123456789"));
                                            double dblNroDaysToRent = Menu.GetOpNumberFromUser("Numero de Dias", "0123456789", 2);
                                            //Console.Write("\nNumero de Dias: ");
                                            //double dblNroDaysToRent = Convert.ToInt32(HpVarious.ReadNumber("0123456789"));
                                            DateTime c_Out = DateTime.Today.AddDays(dblNroDaysToRent);
                                            HpMovies.RentMovie(myDB, myClientTester, ID_Movie, DateTime.Today, c_Out);
                                            break;
                                        case (int)Menu.MainOp2.returnMovie:
                                            HpMovies.ShowMoviesInTableRentedByClient(myDB, myClientTester);
                                            if (strLogInOut == "LOGOUT")
                                            {
                                                ID_Movie = Menu.GetOpNumberFromUser("ID Pelicula", "0123456789", 4);
                                                //Console.Write("\nID Pelicula: ");
                                                //ID_Movie = Convert.ToInt32(HpVarious.ReadNumber("0123456789"));
                                                HpMovies.DeleteMovie(myDB, myClientTester, ID_Movie);
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
                            //HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, true);
                            break;
                        case (int)Menu.MainOp.myRents:
                            if (strLogInOut == "LOGOUT")
                            {
                                HpMovies.ShowMoviesInTableRentedByClient(myDB, myClientTester);
                                Menu.WriteContinue();
                            }
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
                        //case (int)Menu.MainOp.returnMovie:
                        //    if (strLogInOut == "LOGOUT")
                        //    {
                        //        Console.Write("\nID Pelicula: ");
                        //        int ID_Movie = Convert.ToInt32(HpVarious.ReadNumber("0123456789"));
                        //        HpMovies.DeleteMovie(myDB, myClientTester, ID_Movie);
                        //    }

                        //    else
                        //        Menu.WriteNoLog();
                        //    break;
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
