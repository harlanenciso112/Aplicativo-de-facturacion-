using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;
using Pantallas_Sistema_facturacion.Infrastructure.Data;

namespace Pantallas_Sistema_facturacion.Infrastructure.Repositories
{
    public class MySqlCategoriaProductoRepository : ICategoriaProductoRepository
    {
        private readonly AccesoDatos _db;
        private HashSet<string>? _columnas;

        public MySqlCategoriaProductoRepository(AccesoDatos db)
        {
            _db = db;
        }

        public List<CategoriaProducto> ObtenerTodos(string filtro)
        {
            var columnas = ObtenerColumnas();
            var tieneDescripcion = columnas.Contains("descripcion");
            var tieneActivo = columnas.Contains("activo");
            var categorias = new List<CategoriaProducto>();
            var columnasSelect = "id, nombre";
            if (tieneDescripcion)
            {
                columnasSelect += ", descripcion";
            }

            if (tieneActivo)
            {
                columnasSelect += ", activo";
            }

            var sqlSinFiltro = $"SELECT {columnasSelect} FROM tblcategoriaproducto ORDER BY nombre";
            var sqlConFiltro = tieneDescripcion
                ? $"SELECT {columnasSelect} FROM tblcategoriaproducto WHERE nombre LIKE @filtro OR descripcion LIKE @filtro ORDER BY nombre"
                : $"SELECT {columnasSelect} FROM tblcategoriaproducto WHERE nombre LIKE @filtro ORDER BY nombre";

            var sql = string.IsNullOrWhiteSpace(filtro) ? sqlSinFiltro : sqlConFiltro;

            var dt = string.IsNullOrWhiteSpace(filtro)
                ? _db.Consultar(sql)
                : _db.Consultar(sql, new MySqlParameter("@filtro", $"%{filtro}%"));

            foreach (System.Data.DataRow row in dt.Rows)
            {
                categorias.Add(new CategoriaProducto
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nombre = row["nombre"].ToString() ?? string.Empty,
                    Descripcion = tieneDescripcion ? row["descripcion"].ToString() ?? string.Empty : string.Empty,
                    Activo = tieneActivo ? ConvertirABooleano(row["activo"]) : true
                });
            }

            return categorias;
        }

        public CategoriaProducto? ObtenerPorId(int id)
        {
            var columnas = ObtenerColumnas();
            var tieneDescripcion = columnas.Contains("descripcion");
            var tieneActivo = columnas.Contains("activo");

            var columnasSelect = "id, nombre";
            if (tieneDescripcion)
            {
                columnasSelect += ", descripcion";
            }

            if (tieneActivo)
            {
                columnasSelect += ", activo";
            }

            var dt = _db.Consultar(
                $"SELECT {columnasSelect} FROM tblcategoriaproducto WHERE id = @id",
                new MySqlParameter("@id", id));

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            var row = dt.Rows[0];
            return new CategoriaProducto
            {
                Id = Convert.ToInt32(row["id"]),
                Nombre = row["nombre"].ToString() ?? string.Empty,
                Descripcion = tieneDescripcion ? row["descripcion"].ToString() ?? string.Empty : string.Empty,
                Activo = tieneActivo ? ConvertirABooleano(row["activo"]) : true
            };
        }

        public void Crear(CategoriaProducto categoria)
        {
            var columnas = ObtenerColumnas();
            var tieneDescripcion = columnas.Contains("descripcion");
            var tieneActivo = columnas.Contains("activo");

            if (tieneDescripcion && tieneActivo)
            {
                _db.Ejecutar(
                    "INSERT INTO tblcategoriaproducto (nombre, descripcion, activo) VALUES (@nombre, @descripcion, @activo)",
                    new MySqlParameter("@nombre", categoria.Nombre),
                    new MySqlParameter("@descripcion", categoria.Descripcion),
                    new MySqlParameter("@activo", categoria.Activo ? "1" : "0"));
                return;
            }

            if (tieneDescripcion)
            {
                _db.Ejecutar(
                    "INSERT INTO tblcategoriaproducto (nombre, descripcion) VALUES (@nombre, @descripcion)",
                    new MySqlParameter("@nombre", categoria.Nombre),
                    new MySqlParameter("@descripcion", categoria.Descripcion));
                return;
            }

            _db.Ejecutar(
                "INSERT INTO tblcategoriaproducto (nombre) VALUES (@nombre)",
                new MySqlParameter("@nombre", categoria.Nombre));
        }

        public void Actualizar(CategoriaProducto categoria)
        {
            var columnas = ObtenerColumnas();
            var tieneDescripcion = columnas.Contains("descripcion");
            var tieneActivo = columnas.Contains("activo");

            if (tieneDescripcion && tieneActivo)
            {
                _db.Ejecutar(
                    "UPDATE tblcategoriaproducto SET nombre = @nombre, descripcion = @descripcion, activo = @activo WHERE id = @id",
                    new MySqlParameter("@nombre", categoria.Nombre),
                    new MySqlParameter("@descripcion", categoria.Descripcion),
                    new MySqlParameter("@activo", categoria.Activo ? "1" : "0"),
                    new MySqlParameter("@id", categoria.Id));
                return;
            }

            if (tieneDescripcion)
            {
                _db.Ejecutar(
                    "UPDATE tblcategoriaproducto SET nombre = @nombre, descripcion = @descripcion WHERE id = @id",
                    new MySqlParameter("@nombre", categoria.Nombre),
                    new MySqlParameter("@descripcion", categoria.Descripcion),
                    new MySqlParameter("@id", categoria.Id));
                return;
            }

            _db.Ejecutar(
                "UPDATE tblcategoriaproducto SET nombre = @nombre WHERE id = @id",
                new MySqlParameter("@nombre", categoria.Nombre),
                new MySqlParameter("@id", categoria.Id));
        }

        public void Eliminar(int id)
        {
            _db.Ejecutar("DELETE FROM tblcategoriaproducto WHERE id = @id",
                new MySqlParameter("@id", id));
        }

        private HashSet<string> ObtenerColumnas()
        {
            if (_columnas != null)
            {
                return _columnas;
            }

            var dt = _db.Consultar("SHOW COLUMNS FROM tblcategoriaproducto");
            _columnas = dt.Rows.Cast<System.Data.DataRow>()
                .Select(r => r["Field"].ToString() ?? string.Empty)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.ToLowerInvariant())
                .ToHashSet();

            return _columnas;
        }

        private static bool ConvertirABooleano(object valor)
        {
            if (valor == null || valor == DBNull.Value)
            {
                return false;
            }

            if (valor is bool b)
            {
                return b;
            }

            if (valor is byte by)
            {
                return by != 0;
            }

            if (valor is sbyte sby)
            {
                return sby != 0;
            }

            if (valor is short sh)
            {
                return sh != 0;
            }

            if (valor is ushort ush)
            {
                return ush != 0;
            }

            if (valor is int i)
            {
                return i != 0;
            }

            if (valor is uint ui)
            {
                return ui != 0;
            }

            if (valor is long l)
            {
                return l != 0;
            }

            if (valor is ulong ul)
            {
                return ul != 0;
            }

            var texto = valor.ToString()?.Trim().ToLowerInvariant() ?? string.Empty;
            return texto == "1" || texto == "true" || texto == "t" || texto == "si" || texto == "sí" || texto == "s" || texto == "y" || texto == "yes";
        }
    }
}