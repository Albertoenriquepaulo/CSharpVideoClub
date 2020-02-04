using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FastMember;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.Drawing;
using Console = Colorful.Console;

namespace VideoClub.Class
{
    class RUDI
    {
        const string INSERTINTO = "INSERT INTO ";
        const string DELETEFROM = "DELETE FROM ";
        public string table_name { get; set; }
        public string col { get; set; }
        public string value { get; set; }

        //INSERT INTO table_name (column1, column2, column3, ...) VALUES(value1, value2, value3, ...);
        public static int Insert(SQLDBConnection myDB, string table_name, string cols, string values)
        {
            int result = 0;
            string query = $"{INSERTINTO}{table_name} ({cols}) VALUES ({values})";

            if (myDB.GetConnection())
            {
                myDB.CreateCMD();
                myDB.SetSQLQuery(query);

                try
                {
                    result = myDB.CMD.ExecuteNonQuery();
                }
                catch (SqlException MySqlError)
                {
                    Console.WriteLine(MySqlError.Message);
                }
            }
            else
            {
                Console.WriteLine("ERROR Trying to connect", Color.Red);
            }
            myDB.Close();
            return result;
        }

        //DELETE FROM table_name WHERE condition;
        public static int Delete(SQLDBConnection myDB, string table_name, string condition)
        {
            int result = 0;
            string query = $"{DELETEFROM}{table_name} WHERE {condition}";

            if (myDB.GetConnection())
            {
                myDB.CreateCMD();
                myDB.SetSQLQuery(query);

                try
                {
                    result = myDB.CMD.ExecuteNonQuery();
                }
                catch (SqlException MySqlError)
                {
                    Console.WriteLine(MySqlError.Message);
                }
            }
            else
            {
                Console.WriteLine("ERROR Trying to connect", Color.Red);
            }
            myDB.Close();
            return result;
        }

        //UPDATE table_name SET column1 = value1, column2 = value2, ... WHERE condition;
        // colAndVal = "column1 = value1, column2 = value2"
        // condition = ""column1 LIKE 'Archivo'"
        public static int Update(SQLDBConnection myDB, string table_name, string colAndVal, string condition)
        {
            int result = 0;
            string query = $"UPDATE {table_name} SET {colAndVal} WHERE {condition}";

            if (myDB.GetConnection())
            {
                myDB.CreateCMD();
                myDB.SetSQLQuery(query);

                try
                {
                    result = myDB.CMD.ExecuteNonQuery();
                }
                catch (SqlException MySqlError)
                {
                    Console.WriteLine(MySqlError.Message);
                }
            }
            else
            {
                Console.WriteLine("ERROR Trying to connect", Color.Red);
            }
            myDB.Close();
            return result;
        }

        // http://www.codedigest.com/CodeDigest/172-How-to-Convert-SqlDataReader-object-to-DataTable-in-C---ADO-Net-.aspx
        public static DataTable Read(SQLDBConnection myDB, string table_name, string cols = "*", string condition = null)
        {
            string query;
            SqlDataReader readerCollection = null;
            DataTable dt = new DataTable();

            if (condition != null)
            {
                query = $"SELECT {cols} FROM {table_name} WHERE {condition}";
            }
            else
            {
                query = $"SELECT {cols} FROM {table_name}";
            }

            if (myDB.GetConnection())
            {
                myDB.CreateCMD();
                myDB.SetSQLQuery(query);

                try
                {
                    readerCollection = myDB.CMD.ExecuteReader();
                    dt.Load(readerCollection);
                }
                catch (SqlException MySqlError)
                {
                    Console.WriteLine(MySqlError.Message);
                }
            }
            else
            {
                Console.WriteLine("ERROR Trying to connect", Color.Red);
            }
            myDB.Close();
            return dt;
        }

        //EXECUTE AvailableRooms '01/09/2020', '02/16/2020';
        public static DataTable ReadFromSP(SQLDBConnection myDB, string sp, DateTime[] checkIN_OUT)
        {
            string query;
            SqlDataReader readerCollection = null;
            DataTable dt = new DataTable();

            query = $"EXECUTE {sp} '{checkIN_OUT[0].ToString("MM/dd/yyyy")}', '{checkIN_OUT[1].ToString("MM/dd/yyyy")}'";



            if (myDB.GetConnection())
            {
                myDB.CreateCMD();
                myDB.SetSQLQuery(query);

                try
                {
                    readerCollection = myDB.CMD.ExecuteReader();
                    dt.Load(readerCollection);
                }
                catch (SqlException MySqlError)
                {
                    Console.WriteLine(MySqlError.Message);
                }
            }
            else
            {
                Console.WriteLine("ERROR Trying to connect", Color.Red);
            }
            myDB.Close();
            return dt;
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------

        //SELECT column1, column2, ... FROM table_name WHERE condition; 
        //https://stackoverflow.com/questions/41040189/fastest-way-to-map-result-of-sqldatareader-to-object
        public static async Task<T> Read2<T>(SQLDBConnection myDB, string table_name, string cols = "*", string condition = null) where T : class, new()
        {
            string query;
            SqlDataReader readerCollection = null;


            if (condition != null)
            {
                query = $"SELECT {cols} FROM {table_name} WHERE {condition}";
            }
            else
            {
                query = $"SELECT {cols} FROM {table_name}";
            }

            if (myDB.GetConnection())
            {
                myDB.CreateCMD();
                myDB.SetSQLQuery(query);

                try
                {
                    readerCollection = myDB.CMD.ExecuteReader();
                }
                catch (SqlException MySqlError)
                {
                    Console.WriteLine(MySqlError.Message);
                }
            }
            else
            {
                Console.WriteLine("ERROR Trying to connect", Color.Red);
            }
            myDB.Close();

            if (readerCollection.HasRows)
            {
                var newObject = new T();

                if (await readerCollection.ReadAsync())
                {
                    MapDataToObject(readerCollection, newObject);
                }

                return newObject;
            }
            else
            { return null; }

            //return readerCollection;
        }

        public static void MapDataToObject<T>(SqlDataReader dataReader, T newObject)
        {
            if (newObject == null) throw new ArgumentNullException(nameof(newObject));

            // Fast Member Usage
            var objectMemberAccessor = TypeAccessor.Create(newObject.GetType());
            var propertiesHashSet =
                    objectMemberAccessor
                    .GetMembers()
                    .Select(mp => mp.Name)
                    .ToHashSet();

            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                if (propertiesHashSet.Contains(dataReader.GetName(i)))
                {
                    objectMemberAccessor[newObject, dataReader.GetName(i)]
                        = dataReader.IsDBNull(i) ? null : dataReader.GetValue(i);
                }
            }
        }

        // https://www.experts-exchange.com/questions/29106915/C-Convert-SQLDataReader-to-List.html
        public static List<T> Read1<T>(SQLDBConnection myDB, string table_name, string cols = "*", string condition = null) where T : class, new()
        {
            string query;
            SqlDataReader readerCollection = null;


            if (condition != null)
            {
                query = $"SELECT {cols} FROM {table_name} WHERE {condition}";
            }
            else
            {
                query = $"SELECT {cols} FROM {table_name}";
            }

            if (myDB.GetConnection())
            {
                myDB.CreateCMD();
                myDB.SetSQLQuery(query);

                try
                {
                    readerCollection = myDB.CMD.ExecuteReader();
                }
                catch (SqlException MySqlError)
                {
                    Console.WriteLine(MySqlError.Message);
                }
            }
            else
            {
                Console.WriteLine("ERROR Trying to connect", Color.Red);
            }
            List<T> res = new List<T>();
            while (readerCollection.Read())
            {
                T t = new T();

                for (int inc = 0; inc < readerCollection.FieldCount; inc++)
                {
                    Type type = t.GetType();
                    PropertyInfo prop = type.GetProperty(readerCollection.GetName(inc));
                    //https://gist.github.com/mrkodssldrf/7023997
                }

                res.Add(t);
            }
            readerCollection.Close();

            return res;
        }




    }
}

