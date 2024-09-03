using System;
using System.Configuration;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using MySql.Data.MySqlClient;

namespace ciclopedaemon
{
    class ZKFuntion
    {
      
        public void GetGeneralLogData(String ip, int port, int equipo, MySqlConnection connection)
        {
            //Create Standalone SDK class dynamicly.
            zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
           // axCZKEM1.Connect_Net("192.168.1.201", 4370);
            axCZKEM1.Connect_Net(ip, port);
            int idwTMachineNumber = 0;
            int idwEnrollNumber = 0;
            int idwEMachineNumber = 0;
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;

            int idwErrorCode = 0;
            int iGLCount = 0;
            int iIndex = 0;

            axCZKEM1.EnableDevice(1, false);//disable the device
            if (axCZKEM1.ReadGeneralLogData(1))//read all the attendance records to the memory
            {

                // Abrir la conexión
                connection.Open();

                while (axCZKEM1.GetGeneralLogData(1, ref idwTMachineNumber, ref idwEnrollNumber,
                        ref idwEMachineNumber, ref idwVerifyMode, ref idwInOutMode, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute))//get records from the memory
                {
                    Guid uuid = Guid.NewGuid();

                    iGLCount++;
                    Console.WriteLine("uuid: " + uuid.ToString() + 
                        "iGLCount: " + iGLCount.ToString() +
                    "idwEnrollNumber : " + idwEnrollNumber.ToString() +
                    "idwVerifyMode:" + idwVerifyMode.ToString() +
                    "idwInOutMode :" + idwInOutMode.ToString() +
                    "idwYear:" + idwYear.ToString() + " - " + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString());
                    String fecha = idwYear.ToString()+"-"+idwMonth.ToString()+"-"+idwDay.ToString()+" "+idwHour.ToString()+":"+idwMinute.ToString();               
                    iIndex++;

                    Console.WriteLine(fecha);
                  
                        // Crear la consulta para insertar el registro de marcaje
                        string insertQuery = "INSERT INTO RH_Registro_Marcaje (idRH_Registro_Marcaje,fechaRegistro,App_Trabajador_idTrabajador,App_Equipo_idEquipo) VALUES ('" + uuid.ToString() + "','" + fecha + "'," + idwEnrollNumber + "," + equipo + ")";

                        // Crear el comando para ejecutar la consulta
                        MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                        Console.WriteLine(insertQuery); 

                        // Ejecutar la consulta y obtener el número de filas afectadas
                        int rowsAffected = insertCommand.ExecuteNonQuery();
                    
                }
                // Cerrar la conexión
                connection.Close();
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);

                if (idwErrorCode != 0)
                {
                    Console.WriteLine("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString(), "Error");
                }
                else
                {
                    Console.WriteLine("No data from terminal returns!", "Error");
                }
            }
            axCZKEM1.EnableDevice(1, true);//enable the device
        }

    

        public static void GetAllUserInfo(String ip, int port)
        {
            //Create Standalone SDK class dynamicly.
            zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
           // axCZKEM1.Connect_Net("192.168.1.201", 4370);
            axCZKEM1.Connect_Net(ip, port);
            int iEnrollNumber = 0;
            string sname = String.Empty;
            string sPass = String.Empty;
            int iPrivilege = 0;
            bool ienabled = false;

            axCZKEM1.EnableDevice(1, false);
            axCZKEM1.ReadAllUserID(4);//read all the user information to the memory
            while (axCZKEM1.GetAllUserInfo(1, ref iEnrollNumber, ref sname, ref sPass, ref iPrivilege, ref  ienabled))
            {
                Console.WriteLine("iEnrollNumber :" + iEnrollNumber.ToString() +
                    "sname: " + sname +
                    "sPass :" + sPass +
                    "iPrivilege :" + iPrivilege +
                    "ienabled :" + ienabled

                    );
            }

            axCZKEM1.EnableDevice(1, true);


        }

        public static void SetUserInfo(int idApp_Trabajador, string nombre, string codigoPIN , String ip, int port, MySqlConnection connection)
        {
        
            //Create Standalone SDK class dynamicly.
            zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
            axCZKEM1.Connect_Net(ip, port);
            int idwEnrollNumber = 0;
            string sName = "";
            int iPrivilege = 0;
            string sPassword = "";
            bool bEnabled = false;


            axCZKEM1.EnableDevice(1, false);
            idwEnrollNumber = idApp_Trabajador;
            sName = nombre;
            iPrivilege = 1;
            sPassword = codigoPIN;

            if (axCZKEM1.SetUserInfo(1, idwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the memory
                Console.WriteLine(" Success");
            else
                Console.WriteLine(" UnSuccess");

            axCZKEM1.EnableDevice(1, true);

            // Abrir la conexión
            connection.Open();

            // Crear la consulta para actualizar el registro
            string updateQuery = "UPDATE App_Trabajador SET zkChange = 0 WHERE idApp_Trabajador = @idwEnrollNumber AND zkChange = 1";

            // Crear el comando para ejecutar la consulta
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);

            // Asignar valor a los parámetros de la consulta
            updateCommand.Parameters.AddWithValue("@idwEnrollNumber", idwEnrollNumber);

            // Ejecutar la consulta y obtener el número de filas afectadas
            int rowsAffected = updateCommand.ExecuteNonQuery();

            // Cerrar la conexión
            connection.Close();
        }

    }



}
