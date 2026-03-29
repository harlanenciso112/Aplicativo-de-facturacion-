using System;
using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;
using Pantallas_Sistema_facturacion.Infrastructure.Data;
using MySqlConnector;

namespace Pantallas_Sistema_facturacion.Infrastructure.Repositories
{
    public class MySqlUsuarioRepository : IUsuarioRepository
    {
        private readonly AccesoDatos _db;

        public MySqlUsuarioRepository(AccesoDatos db)
        {
            _db = db;
            AsegurarEsquemaUsuarios();
        }

        public List<Usuario> ObtenerTodos()
        {
            var usuarios = new List<Usuario>();
            var dt = _db.Consultar(@"
                SELECT u.id, u.usuario, u.password, u.nombre, u.activo, u.rol_id,
                       IFNULL(r.nombre, 'Sin rol') AS rol_nombre
                FROM tblusuario u
                LEFT JOIN tblrol r ON r.id = u.rol_id
                ORDER BY u.usuario");

            foreach (System.Data.DataRow row in dt.Rows)
            {
                usuarios.Add(new Usuario
                {
                    Id = Convert.ToInt32(row["id"]),
                    UsuarioLogin = row["usuario"].ToString() ?? string.Empty,
                    Password = row["password"].ToString() ?? string.Empty,
                    Nombre = row["nombre"].ToString() ?? string.Empty,
                    RolId = row["rol_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["rol_id"]),
                    RolNombre = row["rol_nombre"].ToString() ?? string.Empty,
                    Activo = Convert.ToBoolean(row["activo"]),
                    UltimoAcceso = string.Empty
                });
            }

            return usuarios;
        }

        public bool ExisteUsuario(string usuarioLogin)
        {
            var dt = _db.Consultar(
                "SELECT COUNT(*) FROM tblusuario WHERE usuario = @usuario",
                new MySqlParameter("@usuario", usuarioLogin));

            return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool ExistePassword(string password)
        {
            var dt = _db.Consultar(
                "SELECT COUNT(*) FROM tblusuario WHERE password = @password",
                new MySqlParameter("@password", password));

            return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public void Crear(Usuario usuario)
        {
            _db.Ejecutar(
                "INSERT INTO tblusuario (usuario, password, nombre, rol_id, activo) VALUES (@usuario, @password, @nombre, @rol_id, @activo)",
                new MySqlParameter("@usuario", usuario.UsuarioLogin),
                new MySqlParameter("@password", usuario.Password),
                new MySqlParameter("@nombre", usuario.Nombre),
                new MySqlParameter("@rol_id", usuario.RolId),
                new MySqlParameter("@activo", usuario.Activo ? 1 : 0));
        }

        public void Actualizar(Usuario usuario)
        {
            _db.Ejecutar(
                "UPDATE tblusuario SET usuario = @usuario, nombre = @nombre, rol_id = @rol_id, activo = @activo WHERE id = @id",
                new MySqlParameter("@usuario", usuario.UsuarioLogin),
                new MySqlParameter("@nombre", usuario.Nombre),
                new MySqlParameter("@rol_id", usuario.RolId),
                new MySqlParameter("@activo", usuario.Activo ? 1 : 0),
                new MySqlParameter("@id", usuario.Id));
        }

        public void Eliminar(int id)
        {
            _db.Ejecutar("DELETE FROM tblusuario WHERE id = @id", new MySqlParameter("@id", id));
        }

        public void ActualizarPassword(int id, string password)
        {
            _db.Ejecutar(
                "UPDATE tblusuario SET password = @password WHERE id = @id",
                new MySqlParameter("@password", password),
                new MySqlParameter("@id", id));
        }

        private void AsegurarEsquemaUsuarios()
        {
            var dtCol = _db.Consultar(@"
                SELECT COUNT(*)
                FROM information_schema.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE()
                  AND TABLE_NAME = 'tblusuario'
                  AND COLUMN_NAME = 'rol_id';");

            if (dtCol.Rows.Count > 0 && Convert.ToInt32(dtCol.Rows[0][0]) == 0)
            {
                _db.Ejecutar(@"
                    ALTER TABLE tblusuario
                    ADD COLUMN rol_id INT NULL AFTER nombre;");
            }

            var dtFk = _db.Consultar(@"
                SELECT COUNT(*)
                FROM information_schema.TABLE_CONSTRAINTS
                WHERE CONSTRAINT_SCHEMA = DATABASE()
                  AND TABLE_NAME = 'tblusuario'
                  AND CONSTRAINT_NAME = 'fk_tblusuario_rol';");

            if (dtFk.Rows.Count > 0 && Convert.ToInt32(dtFk.Rows[0][0]) == 0)
            {
                _db.Ejecutar(@"
                    ALTER TABLE tblusuario
                    ADD CONSTRAINT fk_tblusuario_rol
                    FOREIGN KEY (rol_id) REFERENCES tblrol(id)
                    ON UPDATE CASCADE;");
            }

            _db.Ejecutar(@"
                UPDATE tblusuario u
                LEFT JOIN tblrol r ON r.nombre = 'Administrador'
                SET u.rol_id = r.id
                WHERE u.usuario = 'admin' AND u.rol_id IS NULL;");
        }
    }
}