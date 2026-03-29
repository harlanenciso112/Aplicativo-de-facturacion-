using MySqlConnector;
using System.Data;

namespace Pantallas_Sistema_facturacion.Infrastructure.Data
{
    public class AccesoDatos
    {
        private MySqlConnection conexion;

        private string cadenaConexion =
            "server=localhost;database=facturacion;user=root;password=root;";

        public AccesoDatos()
        {
            conexion = new MySqlConnection(cadenaConexion);
        }

        // Abrir conexión
        public MySqlConnection AbrirConexion()
        {
            if (conexion.State == ConnectionState.Closed)
            {
                conexion.Open();
            }

            return conexion;
        }

        // Cerrar conexión
        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        // Ejecutar SELECT
        public DataTable Consultar(string sql)
        {
            DataTable tabla = new DataTable();

            MySqlCommand comando = new MySqlCommand(sql, AbrirConexion());

            MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
            adapter.Fill(tabla);

            CerrarConexion();

            return tabla;
        }

        // Ejecutar INSERT UPDATE DELETE
        public int Ejecutar(string sql)
        {
            MySqlCommand comando = new MySqlCommand(sql, AbrirConexion());
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();
            return filasAfectadas;
        }

        public DataTable Consultar(string sql, params MySqlParameter[] parametros)
        {
            DataTable tabla = new DataTable();
            MySqlCommand comando = new MySqlCommand(sql, AbrirConexion());
            comando.Parameters.AddRange(parametros);
            MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
            adapter.Fill(tabla);
            CerrarConexion();
            return tabla;
        }

        public int Ejecutar(string sql, params MySqlParameter[] parametros)
        {
            MySqlCommand comando = new MySqlCommand(sql, AbrirConexion());
            comando.Parameters.AddRange(parametros);
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();
            return filasAfectadas;
        }
    }
}