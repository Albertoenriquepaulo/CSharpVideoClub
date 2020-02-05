using System;
using System.Drawing;
using System.Data;
using VideoClub.Class.Tables;
using Console = Colorful.Console;
using System.Threading;
using System.Text;

namespace VideoClub.Class.Helpers
{
    class HpVarious
    {
        public static Boolean IsDate(String date)
        {
            try
            {
                DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void WriteArt(string strToPrint)
        {
            int DA = 244;
            int V = 212;
            int ID = 255;
            Console.WriteAscii(strToPrint, Color.FromArgb(102, 255, 255));
        }

        // Convierte un DataTable a un Objeto Genérico y lo devuelve
        public static Object ConvertToObject(DataTable dTable)
        {

            var myGenericObj = dTable.AsEnumerable().Select(x =>
            new
            {
                ClientID = x.Field<int>("ClientID"),
                DNI = x.Field<string>("DNI"),
                Name = x.Field<string>("Name"),
                LastName = x.Field<string>("LastName"),

            });

            return myGenericObj;
        }

        public static Client AskNewUserData(SQLDBConnection myDB)
        {
            Client clientToInsert = new Client();
            do
            {
                Console.Write("DNI: ");
                clientToInsert.DNI = Console.ReadLine().ToUpper();
                if (clientToInsert.DNI != "0" && clientToInsert.DNI.Length == 9 /*&& !HpClients.ClientExist(myDB, clientToInsert.DNI)*/)
                {
                    break;
                }
                else
                {
                    if (clientToInsert.DNI == "0")
                        Console.WriteLine("ERROR -> El DNI del cliente no puede ser Cero (0)", Color.Red);
                    else if (clientToInsert.DNI.Length != 9)
                        Console.WriteLine("ERROR -> El DNI del cliente debe contener 9 caracteres", Color.Red);
                    else
                    {
                        Console.WriteLine("ERROR -> El cliente Ya existe en la BD. Intente con otro DNI", Color.Red);
                        Menu.WriteContinue();
                    }
                }
            } while (true);

            //TODO: Este If se puede eliminar, ya que si llega aqui es porque evaluó esta condicion en el While
            if (clientToInsert.DNI != "0" && clientToInsert.DNI.Length == 9 && !HpClients.ClientExist(myDB, clientToInsert.DNI))
            {
                Console.Write("Name: ");
                clientToInsert.Name = Console.ReadLine();
                Console.Write("Last Name: ");
                clientToInsert.LastName = Console.ReadLine();
                Console.Write("BirtDate: ");
                clientToInsert.Birthdate = DateTime.Parse(Console.ReadLine());
                Console.Write("email: ");
                clientToInsert.email = Console.ReadLine();
                Console.Write("Password: ");
                clientToInsert.pass = Console.ReadLine();
            }
            return clientToInsert;
        }

        public static int GetAges(DateTime Birtdate)
        {
            return DateTime.Today.Year - Birtdate.Year;
        }

        public static Client ConvertDataTableToClient(DataTable dTable)
        {
            Client clientToLoad = new Client();

            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow row in dTable.Rows)
                {
                    // ... Escribir valor del primer Field como entero. En la pos(0) tengo los RoomID
                    //Console.WriteLine($"{availableRoom[1]}{row.Field<int>(1)}");
                    clientToLoad.ID_Client = row.Field<int>(0);
                    clientToLoad.DNI = row.Field<string>(1);
                    clientToLoad.Name = row.Field<string>(2);
                    clientToLoad.LastName = row.Field<string>(3);
                    clientToLoad.Birthdate = row.Field<DateTime>(4);
                    clientToLoad.email = row.Field<string>(5);
                    clientToLoad.pass = row.Field<string>(6);
                }
            }
            return clientToLoad;
        }

        public static void ShowProgressBar(int ms, double value)
        {
            using (var progress = new ProgressBar())
            {
                for (int i = 0; i <= 100; i++)
                {
                    progress.Report((double)i / value);
                    Thread.Sleep(ms);
                }
            }
            // Console.WriteLine("Done.");
        }

        public static string ReadPassWord()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            return pass;
        }
        public static string ReadNumber(string strCharAllowed)
        {
            var buf = new StringBuilder();
            for (; ; )
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter && buf.Length >= 0)
                {
                    return buf.ToString();
                }
                else if (key.Key == ConsoleKey.Backspace && buf.Length > 0)
                {
                    buf.Remove(buf.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (strCharAllowed.Contains(key.KeyChar))
                {
                    buf.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
                else
                {
                    System.Console.Beep();
                }
            }
        }
    }
}

