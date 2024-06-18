using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0513formulario
{
    class ConexionMySQL
    {
        private MySqlConnection conexion;
        private string servidor;
        private string baseDatos;
        private string usuario;
        private string password;

        public ConexionMySQL()
        {
            // Configura la conexión con tus propios datos
            servidor = "localhost";
            baseDatos = "colores";
            usuario = "root";
            password = "";

            // Crea la cadena de conexión
            string cadenaConexion = $"Server={servidor};Database={baseDatos};Uid={usuario};Pwd={password};";

            // Inicializa la conexión
            conexion = new MySqlConnection(cadenaConexion);
        }
        public MySqlConnection AbrirConexion()
        {
            try
            {
                conexion.Open();
                Console.WriteLine("Conexión abierta.");
                return conexion;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al abrir la conexión: " + ex.Message);
                return null;
            }
        }

        public void CerrarConexion()
        {
            conexion.Close();
            Console.WriteLine("Conexión cerrada.");
        }
 
    }
}
