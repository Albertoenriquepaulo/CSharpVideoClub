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
            do
            {
                Console.WriteAlternating("\tNombre Usuario (DNI): ", alternator);
                userAndPass[0] = Console.ReadLine();
                exist = HpClients.ClientExist(myDB, userAndPass[0]);
                if (!exist)
                {
                    Console.WriteLine("ERROR. Usuario no existe. Indique un usuario válido", Color.Red);
                }

            } while (!exist);

            do
            {
                Console.WriteAlternating("\tContraseña: ", alternator);
                userAndPass[1] = Console.ReadLine();
                exist = HpClients.ClientPasswordExist(myDB, userAndPass);
                if (!exist)
                {
                    Console.WriteLine("ERROR. Password Incorrecto. Indique nuevamente la contraseña", Color.Red);
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
            return Convert.ToInt32(Console.ReadLine());
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
            return Convert.ToInt32(Console.ReadLine());
        }

        public static string GetDNIFromUser(string areaMenu)
        {
            string strDNI;
            Console.Clear();
            HpVarious.WriteArt(APP_NAME);
            do
            {
                WriteArea(areaMenu);
                Console.Write("DNI: ");
                strDNI = Console.ReadLine().ToUpper();
                if (strDNI != "0" && strDNI.Length == 9)
                {
                    return strDNI;
                }
                else
                {
                    Console.WriteLine("ERROR -> DNI debe ser igual a 9 caracteres y diferente de 0", Color.Red);
                }
            } while (true);
        }

        public static int PrintRoomMenu()
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);

            Console.Clear();
            HpVarious.WriteArt(APP_NAME);
            WriteArea("AREA HABITACIONES\n");
            Console.WriteLineAlternating("\t(1) REGISTRAR HABITACION (INCLUIR NUEVA HABITACION)", alternator);
            Console.WriteLineAlternating("\t(2) CONSULTAR HABITACIONES", alternator);
            Console.WriteLineAlternating("\t(3) VOLVER", alternator);
            Console.Write("\nOpcion: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        public static int PrintBookingMenu()
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);

            Console.Clear();
            HpVarious.WriteArt(APP_NAME);
            WriteArea("AREA RESERVACIONES\n");
            Console.WriteLineAlternating("\t(1) RESERVAR (FECHA INICIAL Y FECHA FINAL)", alternator);
            Console.WriteLineAlternating("\t(2) MODIFICAR RESERVACION EXISTENTE", alternator);
            Console.WriteLineAlternating("\t(3) ELIMINAR RESERVACION EXISTENTE", alternator);
            Console.WriteLineAlternating("\t(4) VOLVER", alternator);
            Console.Write("\nOpcion: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        public static int PrintBookingLowLevelMenu()
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);

            Console.Clear();
            HpVarious.WriteArt(APP_NAME);
            WriteArea("AREA RESERVACIONES -> MODIFICAR RESERVACION EXISTENTE\n");
            Console.WriteLineAlternating("\t(1) MODIFICAR CHECK_IN (FECHA INICIAL)", alternator);
            Console.WriteLineAlternating("\t(2) MODIFICAR CHECK_OUT (FECHA FINAL)", alternator);
            Console.WriteLineAlternating("\t(3) MODIFICAR AMBAS CHECK_IN (FECHA INICIAL) Y CHECK_OUT (FECHA FINAL)", alternator);
            Console.WriteLineAlternating("\t(4) VOLVER", alternator);
            Console.Write("\nOpcion: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        // Carga un arreglo de dos posiciones:
        // [0] -> Fecha CheckIn
        // [1] -> Fecha CheckOut
        // No devuelve nada ya que los arreglos se pasan automáticamente por referencia
        public static void PrintBookingQuestions(DateTime[] Dates)
        {
            ColorAlternatorFactory alternatorFactory = new ColorAlternatorFactory();
            ColorAlternator alternator = alternatorFactory.GetAlternator(1, Color.Aqua, Color.Aquamarine);

            bool condition;
            do
            {
                Console.WriteAlternating("FECHA INICIAL (e.g. dd/mm/yyyy): ", alternator);
                Dates[0] = DateTime.Parse(Console.ReadLine());
                Console.WriteAlternating("FECHA FINAL (e.g. dd/mm/yyyy):  ", alternator);
                Dates[1] = DateTime.Parse(Console.ReadLine());
                condition = (DateTime.Compare(Dates[0], Dates[1]) < 0 && DateTime.Compare(Dates[0], DateTime.Today) > 0 && DateTime.Compare(Dates[1], DateTime.Today) > 0);
                if (!condition)
                {
                    Console.WriteLine("ERROR -> Introduzca nuevamente las fechas. \nFECHA INICIAL no puede ser mayor que FECHA FINAL.\nFECHA FINAL no puede ser menor que FECHA INICIAL.\nNinguna de las fechas debe ser mayor que la FECHA ACTUAL.\n", Color.Red);
                }

            } while (!condition);
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

