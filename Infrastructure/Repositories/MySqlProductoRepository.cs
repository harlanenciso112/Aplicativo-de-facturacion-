using System;
using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;
using Pantallas_Sistema_facturacion.Infrastructure.Data;
using MySqlConnector;

namespace Pantallas_Sistema_facturacion.Infrastructure.Repositories
{
    public class MySqlProductoRepository : IProductoRepository
    {
        private readonly AccesoDatos _db;

        public MySqlProductoRepository(AccesoDatos db)
        {
            _db = db;
        }

        public List<Producto> ObtenerTodos(string filtro)
        {
            var productos = new List<Producto>();
            var sql = string.IsNullOrWhiteSpace(filtro)
                ? "SELECT id, nombre, codigo, precio, stock, descripcion, categoria FROM tblproducto ORDER BY nombre"
                : "SELECT id, nombre, codigo, precio, stock, descripcion, categoria FROM tblproducto WHERE nombre LIKE @filtro OR codigo LIKE @filtro ORDER BY nombre";

            var dt = string.IsNullOrWhiteSpace(filtro)
                ? _db.Consultar(sql)
                : _db.Consultar(sql, new MySqlParameter("@filtro", $"%{filtro}%"));

            foreach (System.Data.DataRow row in dt.Rows)
            {
                productos.Add(new Producto
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nombre = row["nombre"].ToString() ?? string.Empty,
                    Codigo = row["codigo"].ToString() ?? string.Empty,
                    Precio = Convert.ToDecimal(row["precio"]),
                    Stock = Convert.ToInt32(row["stock"]),
                    Descripcion = row["descripcion"].ToString() ?? string.Empty,
                    Categoria = row["categoria"].ToString() ?? string.Empty
                });
            }

            return productos;
        }

        public Producto? ObtenerPorId(int id)
        {
            var dt = _db.Consultar(
                "SELECT id, nombre, codigo, precio, stock, descripcion, categoria FROM tblproducto WHERE id = @id",
                new MySqlParameter("@id", id));

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new Producto
            {
                Id = Convert.ToInt32(row["id"]),
                Nombre = row["nombre"].ToString() ?? string.Empty,
                Codigo = row["codigo"].ToString() ?? string.Empty,
                Precio = Convert.ToDecimal(row["precio"]),
                Stock = Convert.ToInt32(row["stock"]),
                Descripcion = row["descripcion"].ToString() ?? string.Empty,
                Categoria = row["categoria"].ToString() ?? string.Empty
            };
        }

        public void Crear(Producto producto)
        {
            _db.Ejecutar(
                "INSERT INTO tblproducto (nombre, codigo, precio, stock, descripcion, categoria) VALUES (@nombre, @codigo, @precio, @stock, @descripcion, @categoria)",
                new MySqlParameter("@nombre", producto.Nombre),
                new MySqlParameter("@codigo", producto.Codigo),
                new MySqlParameter("@precio", producto.Precio),
                new MySqlParameter("@stock", producto.Stock),
                new MySqlParameter("@descripcion", producto.Descripcion),
                new MySqlParameter("@categoria", producto.Categoria));
        }

        public void Actualizar(Producto producto)
        {
            _db.Ejecutar(
                "UPDATE tblproducto SET nombre=@nombre, codigo=@codigo, precio=@precio, stock=@stock, descripcion=@descripcion, categoria=@categoria WHERE id=@id",
                new MySqlParameter("@nombre", producto.Nombre),
                new MySqlParameter("@codigo", producto.Codigo),
                new MySqlParameter("@precio", producto.Precio),
                new MySqlParameter("@stock", producto.Stock),
                new MySqlParameter("@descripcion", producto.Descripcion),
                new MySqlParameter("@categoria", producto.Categoria),
                new MySqlParameter("@id", producto.Id));
        }

        public void Eliminar(int id)
        {
            _db.Ejecutar("DELETE FROM tblproducto WHERE id = @id",
                new MySqlParameter("@id", id));
        }

        public List<string> ObtenerCategorias()
        {
            var categorias = new List<string>();

            System.Data.DataTable dt;
            try
            {
                dt = _db.Consultar(
                    "SELECT nombre FROM tblcategoriaproducto " +
                    "WHERE COALESCE(NULLIF(TRIM(LOWER(CAST(activo AS CHAR))), ''), '1') " +
                    "IN ('1','true','t','si','sí','s','y','yes') ORDER BY nombre");
            }
            catch
            {
                dt = _db.Consultar("SELECT nombre FROM tblcategoriaproducto ORDER BY nombre");
            }

            foreach (System.Data.DataRow row in dt.Rows)
                categorias.Add(row["nombre"].ToString() ?? string.Empty);
            return categorias;
        }
    }
}