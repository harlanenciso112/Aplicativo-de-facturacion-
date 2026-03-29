using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public List<Cliente> Listar(string filtro = "")
        {
            return _clienteRepository.ObtenerTodos(filtro);
        }

        public Cliente? ObtenerPorId(int id)
        {
            return _clienteRepository.ObtenerPorId(id);
        }

        public void Guardar(Cliente cliente)
        {
            if (cliente.Id == 0)
            {
                _clienteRepository.Crear(cliente);
                return;
            }

            _clienteRepository.Actualizar(cliente);
        }

        public void Eliminar(int id)
        {
            _clienteRepository.Eliminar(id);
        }
    }
}