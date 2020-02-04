using Colorful;
using System;
using System.Drawing;
using Console = Colorful.Console;

namespace VideoClub.Class.Helpers
{
    class Menu
    {
        const string APP_NAME = "BBK Video Club";
        public enum MainOp
        {
            availableMovies = 1,
            rent,
            myRents,
            exit
        }
        public enum MainOp2
        {
            showMovies = 1,
            rent,
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
                userAndPass[1] = HpVarious.ReadPassWord();//Console.ReadLine();
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
        public static int PrintMainMenu()
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);

            Console.Clear();
            //Console.WriteLine("SISTEMA RESERVA DE HOTEL BBKBOOTCAMP 2020 (6ta Edición)\n");
            HpVarious.WriteArt(APP_NAME);
            Console.WriteLineAlternating("\t(1) MOSTRAR TODAS LAS PELICULAS (Alquiladas o No)", alternator);
            Console.WriteLineAlternating("\t(2) ALQUILAR PELICULA", alternator);
            Console.WriteLineAlternating("\t(3) MIS ALQUILERES", alternator);
            Console.WriteLineAlternating("\t(4) LOGOUT (SALIR)", alternator);
            Console.Write("\nOpcion: ");
            return Convert.ToInt32(HpVarious.ReadNumber());//return Convert.ToInt32(Console.ReadLine());
        }

        public static int PrintMenuOp2()
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);

            Console.Clear();
            HpVarious.WriteArt(APP_NAME);
            WriteArea("AREA ALAQUILAR\n");
            Console.WriteLineAlternating("\t(1) MOSTRAR PELICULAS DISPONIBLES PERMITIDAS", alternator);
            Console.WriteLineAlternating("\t(2) ALQUILAR PELICULA", alternator);
            Console.WriteLineAlternating("\t(3) VOLVER", alternator);
            Console.Write("\nOpcion: ");
            return Convert.ToInt32(HpVarious.ReadNumber());
        }

        public static void WriteArea(string strArea)
        {
            Console.WriteLine(strArea, Color.FromArgb(0, 102, 255));
        }

        public static void WriteContinue()
        {
            Console.Write("Pulse Cualquier Tecla Para Continuar", Color.Azure);
            Console.ReadLine();
        }

        public static void WriteConstruction()
        {
            Console.WriteLine("En Construcción", Color.Brown);
            WriteContinue();
        }
    }
}

