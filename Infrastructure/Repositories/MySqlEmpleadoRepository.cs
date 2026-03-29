using System;
using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;
using Pantallas_Sistema_facturacion.Infrastructure.Data;
using MySqlConnector;

namespace Pantallas_Sistema_facturacion.Infrastructure.Repositories
{
    public class MySqlEmpleadoRepository : IEmpleadoRepository
    {
        private readonly AccesoDatos _db;

        public MySqlEmpleadoRepository(AccesoDatos db)
        {
            _db = db;
        }

        public List<Empleado> ObtenerTodos(string filtro)
        {
            var empleados = new List<Empleado>();
            var sql = string.IsNullOrWhiteSpace(filtro)
                ? "SELECT id, nombre, cedula, telefono, email, direccion, rol, activo FROM tblempleado ORDER BY nombre"
                : "SELECT id, nombre, cedula, telefono, email, direccion, rol, activo FROM tblempleado WHERE nombre LIKE @filtro OR cedula LIKE @filtro ORDER BY nombre";

            var dt = string.IsNullOrWhiteSpace(filtro)
                ? _db.Consultar(sql)
                : _db.Consultar(sql, new MySqlParameter("@filtro", $"%{filtro}%"));

            foreach (System.Data.DataRow row in dt.Rows)
            {
                empleados.Add(new Empleado
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nombre = row["nombre"].ToString() ?? string.Empty,
                    Cedula = row["cedula"].ToString() ?? string.Empty,
                    Telefono = row["telefono"].ToString() ?? string.Empty,
                    Email = row["email"].ToString() ?? string.Empty,
                    Direccion = row["direccion"].ToString() ?? string.Empty,
                    Rol = row["rol"].ToString() ?? string.Empty,
                    Activo = Convert.ToBoolean(row["activo"])
                });
            }

            return empleados;
        }

        public Empleado? ObtenerPorId(int id)
        {
            var dt = _db.Consultar(
                "SELECT id, nombre, cedula, telefono, email, direccion, rol, activo FROM tblempleado WHERE id = @id",
                new MySqlParameter("@id", id));

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            var row = dt.Rows[0];
            return new Empleado
            {
                Id = Convert.ToInt32(row["id"]),
                Nombre = row["nombre"].ToString() ?? string.Empty,
                Cedula = row["cedula"].ToString() ?? string.Empty,
                Telefono = row["telefono"].ToString() ?? string.Empty,
                Email = row["email"].ToString() ?? string.Empty,
                Direccion = row["direccion"].ToString() ?? string.Empty,
                Rol = row["rol"].ToString() ?? string.Empty,
                Activo = Convert.ToBoolean(row["activo"])
            };
        }

        public void Crear(Empleado empleado)
        {
            _db.Ejecutar(
                "INSERT INTO tblempleado (nombre, cedula, telefono, email, direccion, rol, activo) VALUES (@nombre, @cedula, @telefono, @email, @direccion, @rol, @activo)",
                new MySqlParameter("@nombre", empleado.Nombre),
                new MySqlParameter("@cedula", empleado.Cedula),
                new MySqlParameter("@telefono", empleado.Telefono),
                new MySqlParameter("@email", empleado.Email),
                new MySqlParameter("@direccion", empleado.Direccion),
                new MySqlParameter("@rol", empleado.Rol),
                new MySqlParameter("@activo", empleado.Activo ? 1 : 0));
        }

        public void Actualizar(Empleado empleado)
        {
            _db.Ejecutar(
                "UPDATE tblempleado SET nombre = @nombre, cedula = @cedula, telefono = @telefono, email = @email, direccion = @direccion, rol = @rol, activo = @activo WHERE id = @id",
                new MySqlParameter("@nombre", empleado.Nombre),
                new MySqlParameter("@cedula", empleado.Cedula),
                new MySqlParameter("@telefono", empleado.Telefono),
                new MySqlParameter("@email", empleado.Email),
                new MySqlParameter("@direccion", empleado.Direccion),
                new MySqlParameter("@rol", empleado.Rol),
                new MySqlParameter("@activo", empleado.Activo ? 1 : 0),
                new MySqlParameter("@id", empleado.Id));
        }

        public void Eliminar(int id)
        {
            _db.Ejecutar("DELETE FROM tblempleado WHERE id = @id", new MySqlParameter("@id", id));
        }
    }
}