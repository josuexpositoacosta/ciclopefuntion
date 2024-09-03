using System;
using System.Configuration;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using MySql.Data.MySqlClient;

namespace ciclopedaemon
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            MySqlConnection connection = CreateConnection(connectionString);

            // MySqlConnection connection =  new MySqlConnection(connectionString);

          //  MySqlCommand command = connection.CreateCommand();
           // command.CommandText = "SELECT * FROM app_equipo";
            try
            {
                connection.Open();

                //conection and query to database
                ExecuteQueryGetAllEquipo(connection);

               
               // MySqlDataReader reader = command.ExecuteReader();

               

                //while (reader.Read())
                //{
                //    Console.WriteLine(reader.GetString(0));
                //}
                
                
                
               // reader.Close();


               // ZKFuntion ZKFuntion = new ZKFuntion();
              //  ZKFuntion.GetGeneralLogData("192.168.1.201",4370, connection);


              //  CloseConnection(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            Console.ReadKey();
 
        
      }




         static MySqlConnection CreateConnection(string connectionString)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
          //  connection.Open();
            return connection;
        }

        static void CloseConnection(MySqlConnection connection)
        {
            connection.Close();
        }

        static List<equipo> ExecuteQueryGetAllEquipo(MySqlConnection connection)
        {           

            string query = "SELECT idApp_Equipo, ip FROM app_equipo ";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            // Crear una lista vacía de equipos
            List<equipo> listaEquipos = new List<equipo>();

            while (reader.Read())
            {
                 int id = reader.GetInt32(0);
                string ip = reader.GetString(1);
                //int port = reader.GetInt32(2);

                equipo equipo1 = new equipo(id, ip, 4370);

                listaEquipos.Add(equipo1);


                Console.WriteLine("ID: {0}, ip: {1}, port: 4370", id, ip);
            }

            reader.Close();
           
            return listaEquipos;
        }

        static void ExecuteQuery(MySqlConnection connection)
        {
            string query = "SELECT id, name, age FROM a";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                int age = reader.GetInt32(2);
                Console.WriteLine("ID: {0}, Name: {1}, Age: {2}", id, name, age);
            }
            reader.Close();
        }

    }




    public class equipo{
        
        private int _id;

        private String _ip;

        private int _port;

        public equipo(int id, String ip, int port){
        this._id = id;
        this._ip = ip;
        this._port = port;
        }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        public int port
        {
            get { return _port; }
            set { _port = value; }
        }

    }


        



}
