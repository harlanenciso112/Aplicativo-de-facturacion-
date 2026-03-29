using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class FacturaService
    {
        private readonly IFacturaRepository _facturaRepository;

        public FacturaService(IFacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
        }

        public List<Factura> Listar(string filtro = "")
        {
            return _facturaRepository.ObtenerTodos(filtro);
        }

        public Factura? ObtenerPorId(int id)
        {
            return _facturaRepository.ObtenerPorId(id);
        }

        public string ObtenerSiguienteNumero()
        {
            return _facturaRepository.ObtenerSiguienteNumero();
        }

        public void Guardar(Factura factura)
        {
            if (factura.Id == 0)
            {
                _facturaRepository.Crear(factura);
                return;
            }

            _facturaRepository.Actualizar(factura);
        }

        public void Eliminar(int id)
        {
            _facturaRepository.Eliminar(id);
        }
    }
}