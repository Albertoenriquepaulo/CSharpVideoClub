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
            // Console.WriteLine("Hello World!");
            //HpClients.InsertClient(myDB, HpVarious.AskNewUserData(myDB));
            string[] userAndPass = Menu.PrintLogInMenu(myDB);
            if (userAndPass[2] == "true")
            {
                myClientTester = HpClients.LoadClient(myDB, userAndPass[0]);

                do
                {
                    menuOp = Menu.PrintMainMenu();
                    switch (menuOp)
                    {
                        case (int)Menu.MainOp.availableMovies:
                            HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester);
                            break;
                        case (int)Menu.MainOp.rent:
                            do
                            {
                                menuOp = Menu.PrintMenuOp2();
                                switch (menuOp)
                                {
                                    case (int)Menu.MainOp2.showMovies:
                                        HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, 1);
                                        break;
                                    case (int)Menu.MainOp2.rent: //Alquila Movie
                                        Console.Write("ID Pelicula: ");
                                        int ID_Movie = Convert.ToInt32(Console.ReadLine());
                                        HpMovies.RentMovie(myDB, myClientTester, ID_Movie);
                                        //HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, true);
                                        break;
                                }
                            } while (menuOp < 3);
                            //HpMovies.ShowMoviesInTableAccordingAge(myDB, myClientTester, true);
                            break;
                        case (int)Menu.MainOp.myRents:
                            HpMovies.ShowMoviesInTableRentedByClient(myDB, myClientTester);
                            break;
                    }
                } while (menuOp < 4);

            }

        }
    }
}
