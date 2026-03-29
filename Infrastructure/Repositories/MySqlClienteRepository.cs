using System;
using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;
using Pantallas_Sistema_facturacion.Infrastructure.Data;
using MySqlConnector;

namespace Pantallas_Sistema_facturacion.Infrastructure.Repositories
{
    public class MySqlClienteRepository : IClienteRepository
    {
        private readonly AccesoDatos _db;

        public MySqlClienteRepository(AccesoDatos db)
        {
            _db = db;
        }

        public List<Cliente> ObtenerTodos(string filtro)
        {
            var clientes = new List<Cliente>();
            var sql = string.IsNullOrWhiteSpace(filtro)
                ? "SELECT id, nombre, cedularuc, telefono, email, direccion FROM tblcliente ORDER BY nombre"
                : "SELECT id, nombre, cedularuc, telefono, email, direccion FROM tblcliente WHERE nombre LIKE @filtro OR cedularuc LIKE @filtro ORDER BY nombre";

            var dt = string.IsNullOrWhiteSpace(filtro)
                ? _db.Consultar(sql)
                : _db.Consultar(sql, new MySqlParameter("@filtro", $"%{filtro}%"));

            foreach (System.Data.DataRow row in dt.Rows)
            {
                clientes.Add(new Cliente
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nombre = row["nombre"].ToString() ?? string.Empty,
                    CedulaRuc = row["cedularuc"].ToString() ?? string.Empty,
                    Telefono = row["telefono"].ToString() ?? string.Empty,
                    Email = row["email"].ToString() ?? string.Empty,
                    Direccion = row["direccion"].ToString() ?? string.Empty
                   
                });
            }

            return clientes;
        }

        public Cliente? ObtenerPorId(int id)
        {
            var dt = _db.Consultar(
                "SELECT id, nombre, cedularuc, telefono, email, direccion FROM tblcliente WHERE id = @id",
                new MySqlParameter("@id", id));

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            var row = dt.Rows[0];
            return new Cliente
            {
                Id = Convert.ToInt32(row["id"]),
                Nombre = row["nombre"].ToString() ?? string.Empty,
                CedulaRuc = row["cedularuc"].ToString() ?? string.Empty,
                Telefono = row["telefono"].ToString() ?? string.Empty,
                Email = row["email"].ToString() ?? string.Empty,
                Direccion = row["direccion"].ToString() ?? string.Empty
            };
        }

        public void Crear(Cliente cliente)
        {
            _db.Ejecutar(
                "INSERT INTO tblcliente (nombre, cedularuc, telefono, email, direccion) VALUES (@nombre, @cedularuc, @telefono, @email, @direccion)",
                new MySqlParameter("@nombre", cliente.Nombre),
                new MySqlParameter("@cedularuc", cliente.CedulaRuc),
                new MySqlParameter("@telefono", cliente.Telefono),
                new MySqlParameter("@email", cliente.Email),
                new MySqlParameter("@direccion", cliente.Direccion));
        }

        public void Actualizar(Cliente cliente)
        {
            _db.Ejecutar(
                "UPDATE tblcliente SET nombre = @nombre, cedularuc = @cedularuc, telefono = @telefono, email = @email, direccion = @direccion WHERE id = @id",
                new MySqlParameter("@nombre", cliente.Nombre),
                new MySqlParameter("@cedularuc", cliente.CedulaRuc),
                new MySqlParameter("@telefono", cliente.Telefono),
                new MySqlParameter("@email", cliente.Email),
                new MySqlParameter("@direccion", cliente.Direccion),
                new MySqlParameter("@id", cliente.Id));
        }

        public void Eliminar(int id)
        {
            _db.Ejecutar("DELETE FROM tblcliente WHERE id = @id", new MySqlParameter("@id", id));
        }
    }
}