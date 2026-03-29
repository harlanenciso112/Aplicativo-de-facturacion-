using System;
using System.Collections.Generic;
using MySqlConnector;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;
using Pantallas_Sistema_facturacion.Infrastructure.Data;

namespace Pantallas_Sistema_facturacion.Infrastructure.Repositories
{
    public class MySqlRolRepository : IRolRepository
    {
        private readonly AccesoDatos _db;

        public MySqlRolRepository(AccesoDatos db)
        {
            _db = db;
            AsegurarTablaRoles();
            AsegurarRolesBase();
        }

        public List<Rol> ObtenerTodos(string filtro)
        {
            var roles = new List<Rol>();
            var sql = string.IsNullOrWhiteSpace(filtro)
                ? "SELECT id, nombre, descripcion, activo FROM tblrol ORDER BY nombre"
                : "SELECT id, nombre, descripcion, activo FROM tblrol WHERE nombre LIKE @filtro OR descripcion LIKE @filtro ORDER BY nombre";

            var dt = string.IsNullOrWhiteSpace(filtro)
                ? _db.Consultar(sql)
                : _db.Consultar(sql, new MySqlParameter("@filtro", $"%{filtro}%"));

            foreach (System.Data.DataRow row in dt.Rows)
            {
                roles.Add(new Rol
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nombre = row["nombre"].ToString() ?? string.Empty,
                    Descripcion = row["descripcion"].ToString() ?? string.Empty,
                    Activo = Convert.ToBoolean(row["activo"])
                });
            }

            return roles;
        }

        public List<Rol> ObtenerActivos()
        {
            var roles = new List<Rol>();
            var dt = _db.Consultar("SELECT id, nombre, descripcion, activo FROM tblrol WHERE activo = 1 ORDER BY nombre");

            foreach (System.Data.DataRow row in dt.Rows)
            {
                roles.Add(new Rol
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nombre = row["nombre"].ToString() ?? string.Empty,
                    Descripcion = row["descripcion"].ToString() ?? string.Empty,
                    Activo = Convert.ToBoolean(row["activo"])
                });
            }

            return roles;
        }

        public Rol? ObtenerPorId(int id)
        {
            var dt = _db.Consultar(
                "SELECT id, nombre, descripcion, activo FROM tblrol WHERE id = @id",
                new MySqlParameter("@id", id));

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            var row = dt.Rows[0];
            return new Rol
            {
                Id = Convert.ToInt32(row["id"]),
                Nombre = row["nombre"].ToString() ?? string.Empty,
                Descripcion = row["descripcion"].ToString() ?? string.Empty,
                Activo = Convert.ToBoolean(row["activo"])
            };
        }

        public Rol? ObtenerPorNombre(string nombre)
        {
            var dt = _db.Consultar(
                "SELECT id, nombre, descripcion, activo FROM tblrol WHERE nombre = @nombre",
                new MySqlParameter("@nombre", nombre));

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            var row = dt.Rows[0];
            return new Rol
            {
                Id = Convert.ToInt32(row["id"]),
                Nombre = row["nombre"].ToString() ?? string.Empty,
                Descripcion = row["descripcion"].ToString() ?? string.Empty,
                Activo = Convert.ToBoolean(row["activo"])
            };
        }

        public void Crear(Rol rol)
        {
            _db.Ejecutar(
                "INSERT INTO tblrol (nombre, descripcion, activo) VALUES (@nombre, @descripcion, @activo)",
                new MySqlParameter("@nombre", rol.Nombre),
                new MySqlParameter("@descripcion", rol.Descripcion),
                new MySqlParameter("@activo", rol.Activo ? 1 : 0));
        }

        public void Actualizar(Rol rol)
        {
            _db.Ejecutar(
                "UPDATE tblrol SET nombre = @nombre, descripcion = @descripcion, activo = @activo WHERE id = @id",
                new MySqlParameter("@nombre", rol.Nombre),
                new MySqlParameter("@descripcion", rol.Descripcion),
                new MySqlParameter("@activo", rol.Activo ? 1 : 0),
                new MySqlParameter("@id", rol.Id));
        }

        public void Eliminar(int id)
        {
            _db.Ejecutar("DELETE FROM tblrol WHERE id = @id", new MySqlParameter("@id", id));
        }

        private void AsegurarTablaRoles()
        {
            _db.Ejecutar(@"
                CREATE TABLE IF NOT EXISTS tblrol (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    nombre VARCHAR(100) NOT NULL UNIQUE,
                    descripcion VARCHAR(255),
                    activo TINYINT(1) NOT NULL DEFAULT 1
                ) ENGINE=InnoDB;");
        }

        private void AsegurarRolesBase()
        {
            _db.Ejecutar(@"
                INSERT INTO tblrol (nombre, descripcion, activo)
                SELECT 'Administrador', 'Acceso completo del sistema', 1
                WHERE NOT EXISTS (SELECT 1 FROM tblrol WHERE nombre = 'Administrador');");

            _db.Ejecutar(@"
                INSERT INTO tblrol (nombre, descripcion, activo)
                SELECT 'Vendedor', 'Registro de ventas y facturas', 1
                WHERE NOT EXISTS (SELECT 1 FROM tblrol WHERE nombre = 'Vendedor');");

            _db.Ejecutar(@"
                INSERT INTO tblrol (nombre, descripcion, activo)
                SELECT 'Cajero', 'Cobro y consulta de facturas', 1
                WHERE NOT EXISTS (SELECT 1 FROM tblrol WHERE nombre = 'Cajero');");

            _db.Ejecutar(@"
                INSERT INTO tblrol (nombre, descripcion, activo)
                SELECT 'Contador', 'Consulta de facturas e informes financieros', 1
                WHERE NOT EXISTS (SELECT 1 FROM tblrol WHERE nombre = 'Contador');");

            _db.Ejecutar(@"
                INSERT INTO tblrol (nombre, descripcion, activo)
                SELECT 'Inventario', 'Gestión de productos, categorías e inventario', 1
                WHERE NOT EXISTS (SELECT 1 FROM tblrol WHERE nombre = 'Inventario');");
        }
    }
}