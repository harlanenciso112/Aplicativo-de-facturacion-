using System;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;
using Pantallas_Sistema_facturacion.Infrastructure.Data;
using MySqlConnector;

namespace Pantallas_Sistema_facturacion.Infrastructure.Repositories
{
    public class MySqlAuthRepository : IAuthRepository
    {
        private readonly AccesoDatos _db;

        public MySqlAuthRepository(AccesoDatos db)
        {
            _db = db;
        }

        public bool ValidarCredenciales(string usuario, string password)
        {
            return ObtenerUsuarioAutenticado(usuario, password) != null;
        }

        public Usuario? ObtenerUsuarioAutenticado(string usuario, string password)
        {
            var dt = _db.Consultar(@"
                SELECT u.id, u.usuario, u.password, u.nombre, u.activo, u.rol_id,
                       IFNULL(r.nombre, 'Sin rol') AS rol_nombre
                FROM tblusuario u
                LEFT JOIN tblrol r ON r.id = u.rol_id
                WHERE u.usuario = @usuario AND u.password = @password AND u.activo = 1
                LIMIT 1",
                new MySqlParameter("@usuario", usuario),
                new MySqlParameter("@password", password));

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            var row = dt.Rows[0];
            return new Usuario
            {
                Id = Convert.ToInt32(row["id"]),
                UsuarioLogin = row["usuario"].ToString() ?? string.Empty,
                Password = row["password"].ToString() ?? string.Empty,
                Nombre = row["nombre"].ToString() ?? string.Empty,
                RolId = row["rol_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["rol_id"]),
                RolNombre = row["rol_nombre"].ToString() ?? string.Empty,
                Activo = true,
                UltimoAcceso = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
    }
}