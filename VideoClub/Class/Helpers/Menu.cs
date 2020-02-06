using Colorful;
using System;
using System.Drawing;
using VideoClub.Class.Tables;
using Console = Colorful.Console;

namespace VideoClub.Class.Helpers
{
    class Menu
    {
        const string APP_NAME = "BBK Video Vintage";
        public enum MainOp
        {
            availableMovies = 1,
            rent,
            myRents,
            logOutIN,
            returnMovie,
            exit
        }
        public enum MainOp2
        {
            showMovies = 1,
            rent,
            returnMovie,
            back
        }


        public static string[] PrintLogInMenu(SQLDBConnection myDB)
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);
            string[] userAndPass = new string[3];
            bool exist;
            Console.Clear();
            //Console.WriteLine("SISTEMA RESERVA DE HOTEL BBKBOOTCAMP 2020 (6ta Edición)\n");
            HpVarious.WriteArt(APP_NAME);
            Console.WriteAlternating("Bienvenido, deberá contar con un usuario válido para acceder a las opciones del aplicativo\n\n", alternator);
            do
            {
                Console.WriteAlternating("Nombre Usuario (DNI): ", alternator);
                userAndPass[0] = Console.ReadLine();
                exist = HpClients.ClientExist(myDB, userAndPass[0]);
                if (!exist)
                {
                    Console.WriteLine("ERROR. Usuario no existe. Indique un usuario válido!!!\n", Color.Red);
                }

            } while (!exist);

            do
            {
                Console.WriteAlternating("Contraseña: ", alternator);
                userAndPass[1] = HpVarious.ReadPassWord();
                exist = HpClients.ClientPasswordExist(myDB, userAndPass);
                if (!exist)
                {
                    Console.WriteLine("\t\tERROR. Password Incorrecto. Introduzca nuevamente la contraseña", Color.Red);
                }
                else
                {
                    userAndPass[2] = "true";
                }
            } while (!exist);

            return userAndPass;
        }
        public static int PrintMainMenu(string strLogInOut, Client myClientTester)
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);
            string strOp;
            Console.Clear();
            Console.ResetColor();
            HpVarious.WriteArt(APP_NAME);
            if (myClientTester.Name != null)
                Console.WriteLine($"{myClientTester.Name} {myClientTester.LastName}, Bienvenido...\n");
            Console.WriteLineAlternating("\t(1) MOSTRAR CATALOGO", alternator);
            Console.WriteLineAlternating("\t(2) ALQUILAR/DEVOLVER PELICULA", alternator);
            Console.WriteLineAlternating("\t(3) MIS ALQUILERES", alternator);
            Console.WriteLineAlternating($"\t(4) {strLogInOut}", alternator);
            Console.WriteLineAlternating("\t(5) SALIR", alternator);
            do
            {
                Console.Write("\nOpcion: ", Color.CadetBlue);
                Console.ResetColor();
                strOp = HpVarious.ReadNumber("12345", 1);
            } while (strOp == "");

            return Convert.ToInt32(strOp);//return Convert.ToInt32(Console.ReadLine());
        }

        public static int PrintMenuOp2()
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);
            string strOp;
            Console.Clear();
            HpVarious.WriteArt(APP_NAME);
            WriteArea("AREA ALQUILAR\n");
            Console.WriteLineAlternating("\t(1) MOSTRAR PELICULAS DISPONIBLES PERMITIDAS", alternator);
            Console.WriteLineAlternating("\t(2) ALQUILAR PELICULA", alternator);
            Console.WriteLineAlternating("\t(3) DEVOLUCION PELICULA", alternator);
            Console.WriteLineAlternating("\t(4) VOLVER", alternator);
            do
            {
                Console.Write("\nOpcion: ", Color.CadetBlue);
                Console.ResetColor();
                strOp = HpVarious.ReadNumber("1234", 1);
            } while (strOp == "");

            return Convert.ToInt32(strOp);
        }

        public static void WriteArea(string strArea)
        {
            Console.WriteLine(strArea, Color.FromArgb(0, 102, 255));
        }

        public static void WriteContinue()
        {
            Console.Write("Presione Enter Para Continuar\n", Color.Azure);
            Console.ResetColor();
            Console.ReadLine();
        }

        public static void WriteConstruction()
        {
            Console.WriteLine("En Construcción", Color.Brown);
            Console.ResetColor();
            WriteContinue();
        }

        public static void WriteNoLog()
        {
            Console.WriteLine("\nERROR. Debe estar logeado\n\n", Color.Red);
            WriteContinue();
        }

        public static int GetOpNumberFromUser(string strText, string strAllowedNumbers, int qtDigitsAllowed)
        {
            string strOp;

            do
            {
                Console.Write($"\n{strText}: ");
                strOp = HpVarious.ReadNumber(strAllowedNumbers, qtDigitsAllowed);
            } while (strOp == "");

            return Convert.ToInt32(strOp);
        }
    }
}

