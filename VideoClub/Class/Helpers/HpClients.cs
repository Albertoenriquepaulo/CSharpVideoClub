using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using VideoClub.Class.Tables;
using Console = Colorful.Console;

namespace VideoClub.Class.Helpers
{
    class HpClients
    {
        //Carga Cliente en la DB
        public static bool InsertClient(SQLDBConnection myDB, Client cToInsert)
        {
            int result; //Para los resultados de las consultas RUDI
            DataTable dTable;
            Console.WriteLine($"REGISTRANDO CLIENTE BAJO EL DNI: {cToInsert.DNI}");

            result = RUDI.Insert(myDB, "Clients", "DNI, Name, LastName, BirthDate, email, pass", $"'{cToInsert.DNI}', '{cToInsert.Name}', '{cToInsert.LastName}', '{cToInsert.Birthdate.ToString("MM/dd/yyyy")}', '{cToInsert.email}', '{cToInsert.pass}'");
            if (result == 1)
            {
                int clientID;
                dTable = RUDI.Read(myDB, "Clients", "ID_Client", $"DNI LIKE '{cToInsert.DNI}'");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
                clientID = Convert.ToInt32(dTable.Rows[0]["ID_Client"]);
                Console.WriteLine($"El cliente '{cToInsert.Name} {cToInsert.LastName}' ha sido creado con exito bajo el ID#: {clientID}", Color.Blue);
                Menu.WriteContinue();
                return true;
            }
            return false;
        }

        public static bool ClientExist(SQLDBConnection myDB, string strDNI)
        {
            DataTable dTable;
            if (strDNI.Length == 9)
            {
                dTable = RUDI.Read(myDB, "Clients", "ID_Client", $"DNI LIKE '{strDNI.ToUpper()}'");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
                if (dTable != null && dTable.Rows.Count > 0)
                    return true;
            }
            return false;
        }

        public static bool ClientEmailExist(SQLDBConnection myDB, int ID_Client)
        {
            DataTable dTable;
            if (ID_Client > 0)
            {
                dTable = RUDI.Read(myDB, "Clients", "Name", $"ID_Client={ID_Client}");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
                if (dTable != null && dTable.Rows.Count > 0)
                    return true;
            }
            return false;
        }

        public static bool ClientPasswordExist(SQLDBConnection myDB, string[] userAndPass)
        {
            DataTable dTable;
            if (userAndPass[0].Length == 9)
            {
                dTable = RUDI.Read(myDB, "Clients", "email", $"DNI LIKE '{userAndPass[0].ToUpper()}' AND pass LIKE '{userAndPass[1].ToLower()}'");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
                if (dTable != null && dTable.Rows.Count > 0)
                    return true;
            }
            return false;
        }
        public static bool ClientAndPassMatch(SQLDBConnection myDB, string strDNI)
        {
            DataTable dTable;
            if (strDNI.Length == 9)
            {
                dTable = RUDI.Read(myDB, "Clients", "ID_Client", $"DNI LIKE '{strDNI.ToUpper()}'");  //SELECT ClientID FROM Clients WHERE DNI = strDNI
                if (dTable != null && dTable.Rows.Count > 0)
                    return true;
            }
            return false;
        }

        public static Client LoadClient(SQLDBConnection myDB, string strDNI)
        {
            DataTable dTable = RUDI.Read(myDB, "Clients", "*", $"DNI LIKE '{strDNI}'");
            return HpVarious.ConvertDataTableToClient(dTable);
        }
    }
}
