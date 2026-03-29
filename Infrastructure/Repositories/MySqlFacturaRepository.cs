using System;
using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;
using Pantallas_Sistema_facturacion.Infrastructure.Data;
using MySqlConnector;

namespace Pantallas_Sistema_facturacion.Infrastructure.Repositories
{
    public class MySqlFacturaRepository : IFacturaRepository
    {
        private readonly AccesoDatos _db;

        public MySqlFacturaRepository(AccesoDatos db)
        {
            _db = db;
            AsegurarTablasFacturas();
        }

        public List<Factura> ObtenerTodos(string filtro)
        {
            var facturas = new List<Factura>();
            var sql = string.IsNullOrWhiteSpace(filtro)
                ? @"SELECT f.id, f.numero, f.fecha, f.cliente_id, c.nombre AS cliente, f.total, f.estado
                    FROM tblfactura f
                    INNER JOIN tblcliente c ON c.id = f.cliente_id
                    ORDER BY f.id DESC"
                : @"SELECT f.id, f.numero, f.fecha, f.cliente_id, c.nombre AS cliente, f.total, f.estado
                    FROM tblfactura f
                    INNER JOIN tblcliente c ON c.id = f.cliente_id
                    WHERE f.numero LIKE @filtro OR c.nombre LIKE @filtro
                    ORDER BY f.id DESC";

            var dt = string.IsNullOrWhiteSpace(filtro)
                ? _db.Consultar(sql)
                : _db.Consultar(sql, new MySqlParameter("@filtro", $"%{filtro}%"));

            foreach (System.Data.DataRow row in dt.Rows)
            {
                facturas.Add(new Factura
                {
                    Id = Convert.ToInt32(row["id"]),
                    Numero = row["numero"].ToString() ?? string.Empty,
                    Fecha = Convert.ToDateTime(row["fecha"]),
                    ClienteId = Convert.ToInt32(row["cliente_id"]),
                    ClienteNombre = row["cliente"].ToString() ?? string.Empty,
                    Total = Convert.ToDecimal(row["total"]),
                    Estado = row["estado"].ToString() ?? "ACTIVA"
                });
            }

            return facturas;
        }

        public Factura? ObtenerPorId(int id)
        {
            var dtFactura = _db.Consultar(
                @"SELECT f.id, f.numero, f.fecha, f.cliente_id, c.nombre AS cliente, f.total, f.estado
                  FROM tblfactura f
                  INNER JOIN tblcliente c ON c.id = f.cliente_id
                  WHERE f.id = @id",
                new MySqlParameter("@id", id));

            if (dtFactura.Rows.Count == 0)
            {
                return null;
            }

            var row = dtFactura.Rows[0];
            var factura = new Factura
            {
                Id = Convert.ToInt32(row["id"]),
                Numero = row["numero"].ToString() ?? string.Empty,
                Fecha = Convert.ToDateTime(row["fecha"]),
                ClienteId = Convert.ToInt32(row["cliente_id"]),
                ClienteNombre = row["cliente"].ToString() ?? string.Empty,
                Total = Convert.ToDecimal(row["total"]),
                Estado = row["estado"].ToString() ?? "ACTIVA"
            };

            var dtDetalle = _db.Consultar(
                @"SELECT d.id, d.factura_id, d.producto_id, p.nombre AS producto, d.cantidad, d.precio_unitario
                  FROM tblfactura_detalle d
                  INNER JOIN tblproducto p ON p.id = d.producto_id
                  WHERE d.factura_id = @facturaId
                  ORDER BY d.id",
                new MySqlParameter("@facturaId", id));

            foreach (System.Data.DataRow det in dtDetalle.Rows)
            {
                factura.Detalles.Add(new FacturaDetalle
                {
                    Id = Convert.ToInt32(det["id"]),
                    FacturaId = Convert.ToInt32(det["factura_id"]),
                    ProductoId = Convert.ToInt32(det["producto_id"]),
                    ProductoNombre = det["producto"].ToString() ?? string.Empty,
                    Cantidad = Convert.ToInt32(det["cantidad"]),
                    PrecioUnitario = Convert.ToDecimal(det["precio_unitario"])
                });
            }

            return factura;
        }

        public string ObtenerSiguienteNumero()
        {
            var dt = _db.Consultar("SELECT IFNULL(MAX(id), 0) + 1 AS siguiente FROM tblfactura");
            var siguiente = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["siguiente"]);
            return $"FAC-{siguiente:000000}";
        }

        public void Crear(Factura factura)
        {
            if (factura.Detalles.Count == 0)
            {
                throw new InvalidOperationException("La factura debe tener al menos un detalle.");
            }

            var conexion = _db.AbrirConexion();
            using var transaccion = conexion.BeginTransaction();
            try
            {
                var cmdFactura = new MySqlCommand(
                    @"INSERT INTO tblfactura (numero, fecha, cliente_id, total, estado)
                      VALUES (@numero, @fecha, @cliente_id, @total, @estado);
                      SELECT LAST_INSERT_ID();",
                    conexion,
                    transaccion);

                cmdFactura.Parameters.AddWithValue("@numero", factura.Numero);
                cmdFactura.Parameters.AddWithValue("@fecha", factura.Fecha);
                cmdFactura.Parameters.AddWithValue("@cliente_id", factura.ClienteId);
                cmdFactura.Parameters.AddWithValue("@total", factura.Total);
                cmdFactura.Parameters.AddWithValue("@estado", factura.Estado);

                var idFactura = Convert.ToInt32(cmdFactura.ExecuteScalar());

                foreach (var detalle in factura.Detalles)
                {
                    var cmdDetalle = new MySqlCommand(
                        @"INSERT INTO tblfactura_detalle (factura_id, producto_id, cantidad, precio_unitario, subtotal)
                          VALUES (@factura_id, @producto_id, @cantidad, @precio_unitario, @subtotal)",
                        conexion,
                        transaccion);

                    cmdDetalle.Parameters.AddWithValue("@factura_id", idFactura);
                    cmdDetalle.Parameters.AddWithValue("@producto_id", detalle.ProductoId);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@precio_unitario", detalle.PrecioUnitario);
                    cmdDetalle.Parameters.AddWithValue("@subtotal", detalle.Subtotal);
                    cmdDetalle.ExecuteNonQuery();

                    var cmdStock = new MySqlCommand(
                        "UPDATE tblproducto SET stock = stock - @cantidad WHERE id = @producto_id",
                        conexion,
                        transaccion);
                    cmdStock.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmdStock.Parameters.AddWithValue("@producto_id", detalle.ProductoId);
                    cmdStock.ExecuteNonQuery();
                }

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
            finally
            {
                _db.CerrarConexion();
            }
        }

        public void Actualizar(Factura factura)
        {
            if (factura.Detalles.Count == 0)
            {
                throw new InvalidOperationException("La factura debe tener al menos un detalle.");
            }

            var conexion = _db.AbrirConexion();
            using var transaccion = conexion.BeginTransaction();
            try
            {
                var cmdRestore = new MySqlCommand(
                    @"UPDATE tblproducto p
                      INNER JOIN tblfactura_detalle d ON d.producto_id = p.id
                      SET p.stock = p.stock + d.cantidad
                      WHERE d.factura_id = @facturaId",
                    conexion,
                    transaccion);
                cmdRestore.Parameters.AddWithValue("@facturaId", factura.Id);
                cmdRestore.ExecuteNonQuery();

                var cmdDeleteDetalle = new MySqlCommand(
                    "DELETE FROM tblfactura_detalle WHERE factura_id = @facturaId",
                    conexion,
                    transaccion);
                cmdDeleteDetalle.Parameters.AddWithValue("@facturaId", factura.Id);
                cmdDeleteDetalle.ExecuteNonQuery();

                var cmdFactura = new MySqlCommand(
                    @"UPDATE tblfactura
                      SET numero = @numero, fecha = @fecha, cliente_id = @cliente_id, total = @total, estado = @estado
                      WHERE id = @id",
                    conexion,
                    transaccion);

                cmdFactura.Parameters.AddWithValue("@numero", factura.Numero);
                cmdFactura.Parameters.AddWithValue("@fecha", factura.Fecha);
                cmdFactura.Parameters.AddWithValue("@cliente_id", factura.ClienteId);
                cmdFactura.Parameters.AddWithValue("@total", factura.Total);
                cmdFactura.Parameters.AddWithValue("@estado", factura.Estado);
                cmdFactura.Parameters.AddWithValue("@id", factura.Id);
                cmdFactura.ExecuteNonQuery();

                foreach (var detalle in factura.Detalles)
                {
                    var cmdDetalle = new MySqlCommand(
                        @"INSERT INTO tblfactura_detalle (factura_id, producto_id, cantidad, precio_unitario, subtotal)
                          VALUES (@factura_id, @producto_id, @cantidad, @precio_unitario, @subtotal)",
                        conexion,
                        transaccion);

                    cmdDetalle.Parameters.AddWithValue("@factura_id", factura.Id);
                    cmdDetalle.Parameters.AddWithValue("@producto_id", detalle.ProductoId);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@precio_unitario", detalle.PrecioUnitario);
                    cmdDetalle.Parameters.AddWithValue("@subtotal", detalle.Subtotal);
                    cmdDetalle.ExecuteNonQuery();

                    var cmdStock = new MySqlCommand(
                        "UPDATE tblproducto SET stock = stock - @cantidad WHERE id = @producto_id",
                        conexion,
                        transaccion);
                    cmdStock.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmdStock.Parameters.AddWithValue("@producto_id", detalle.ProductoId);
                    cmdStock.ExecuteNonQuery();
                }

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
            finally
            {
                _db.CerrarConexion();
            }
        }

        public void Eliminar(int id)
        {
            var conexion = _db.AbrirConexion();
            using var transaccion = conexion.BeginTransaction();
            try
            {
                var cmdRestore = new MySqlCommand(
                    @"UPDATE tblproducto p
                      INNER JOIN tblfactura_detalle d ON d.producto_id = p.id
                      SET p.stock = p.stock + d.cantidad
                      WHERE d.factura_id = @facturaId",
                    conexion,
                    transaccion);
                cmdRestore.Parameters.AddWithValue("@facturaId", id);
                cmdRestore.ExecuteNonQuery();

                var cmdDeleteDetalle = new MySqlCommand(
                    "DELETE FROM tblfactura_detalle WHERE factura_id = @facturaId",
                    conexion,
                    transaccion);
                cmdDeleteDetalle.Parameters.AddWithValue("@facturaId", id);
                cmdDeleteDetalle.ExecuteNonQuery();

                var cmdDeleteFactura = new MySqlCommand(
                    "DELETE FROM tblfactura WHERE id = @facturaId",
                    conexion,
                    transaccion);
                cmdDeleteFactura.Parameters.AddWithValue("@facturaId", id);
                cmdDeleteFactura.ExecuteNonQuery();

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
            finally
            {
                _db.CerrarConexion();
            }
        }

        private void AsegurarTablasFacturas()
        {
            _db.Ejecutar(@"
                CREATE TABLE IF NOT EXISTS tblfactura (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    numero VARCHAR(30) NOT NULL UNIQUE,
                    fecha DATETIME NOT NULL,
                    cliente_id INT NOT NULL,
                    total DECIMAL(10,2) NOT NULL DEFAULT 0.00,
                    estado VARCHAR(20) NOT NULL DEFAULT 'ACTIVA',
                    INDEX ix_tblfactura_cliente (cliente_id),
                    CONSTRAINT fk_tblfactura_cliente
                        FOREIGN KEY (cliente_id) REFERENCES tblcliente(id)
                        ON UPDATE CASCADE
                ) ENGINE=InnoDB;");

            _db.Ejecutar(@"
                CREATE TABLE IF NOT EXISTS tblfactura_detalle (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    factura_id INT NOT NULL,
                    producto_id INT NOT NULL,
                    cantidad INT NOT NULL,
                    precio_unitario DECIMAL(10,2) NOT NULL,
                    subtotal DECIMAL(10,2) NOT NULL,
                    INDEX ix_tblfacturadetalle_factura (factura_id),
                    INDEX ix_tblfacturadetalle_producto (producto_id),
                    CONSTRAINT fk_tblfacturadetalle_factura
                        FOREIGN KEY (factura_id) REFERENCES tblfactura(id)
                        ON DELETE CASCADE
                        ON UPDATE CASCADE,
                    CONSTRAINT fk_tblfacturadetalle_producto
                        FOREIGN KEY (producto_id) REFERENCES tblproducto(id)
                        ON UPDATE CASCADE
                ) ENGINE=InnoDB;");
        }
    }
}