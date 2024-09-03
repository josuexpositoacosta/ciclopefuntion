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

            // Obtener el valor del servidor y el puerto
           // string server = ConfigurationManager.AppSettings["server"];           
           // string port = ConfigurationManager.AppSettings["port"];
          //  int resultport;           
           // int.TryParse(port, out resultport);          
           //   Console.WriteLine("la conección : " + server+ " : "+ resultport);

            MySqlConnection connection = CreateConnection(connectionString);

            try
            {        

               List<equipo> listaequipo =  ExecuteQueryGetAllEquipo(connection);
               ZKFuntion ZKFuntion = new ZKFuntion();

               foreach(equipo e in listaequipo)
               {               
                 ZKFuntion.GetGeneralLogData(e.ip,e.port,e.id, connection);

                 List<trabajador> listaTrabajador = ExecuteQueryGetAllTrabajador(connection);

                 foreach (trabajador t in listaTrabajador)
                 {
                     ZKFuntion.SetUserInfo(t.idt, t.nom, t.pin, e.ip, e.port,connection);
                 }


                 ZKFuntion.GetAllUserInfo(e.ip, e.port);

               }

              CloseConnection(connection); 
               
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
            connection.Open();
            return connection;
        }

        static void CloseConnection(MySqlConnection connection)
        {
            connection.Close();
        }

        static List<equipo> ExecuteQueryGetAllEquipo(MySqlConnection connection)
        {           

            string query = "SELECT idApp_Equipo, ip FROM App_Equipo ";
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


        static List<trabajador> ExecuteQueryGetAllTrabajador(MySqlConnection connection)
        {
            string query = "SELECT idApp_Trabajador,nombre,codigoPIN FROM App_Trabajador Where zkChange = 1";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            // Crear una lista vacía de equipos
            List<trabajador> lista = new List<trabajador>();

            while (reader.Read())
            {
                int idt = reader.GetInt32(0);
                string nom = reader.GetString(1);
                string pin = reader.GetString(2);

                trabajador equipo1 = new trabajador(idt, nom, pin);

                lista.Add(equipo1);


                Console.WriteLine("ID: {0}, nombre: {1}, pin: {2}", idt, nom, pin);
            }

            reader.Close();

            return lista;
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


    public class trabajador
    {
        private int _idt;
        private String _nom;
        private String _pin;

        public trabajador(int idt, String nom, String pin)
        {
            this._idt = idt;
            this._nom = nom;
            this._pin = pin;
        }

        public int idt
        {
            get { return _idt; }
            set { _idt = value; }
        }

        public String nom
        {
            get { return _nom; }
            set { _nom = value; }
        }
        public String pin
        {
            get { return _pin; }
            set { _pin = value; }
        }

    }  



}
