using System;
using System.Collections.Generic;
using System.Text;

namespace VideoClub.Class.Tables
{
    class Client

    {
        public Client()
        {

        }
        public Client(int iD_Client, string dNI, string name, string lastName, DateTime dateTime, string email, string pass)
        {
            ID_Client = iD_Client;
            DNI = dNI;
            Name = name;
            LastName = lastName;
            this.Birthdate = dateTime;
            this.email = email;
            this.pass = pass;
        }

        public int ID_Client { get; set; }
        public string DNI { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string email { get; set; }
        public string pass { get; set; }

        public void Clear()
        {
            ID_Client = 0;
            DNI = null;
            Name = null;
            LastName = null;
            // Birthdate = null;
            this.email = null;
            this.pass = null;
        }

    }
}
