using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class RolService
    {
        private readonly IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public List<Rol> Listar(string filtro = "")
        {
            return _rolRepository.ObtenerTodos(filtro);
        }

        public List<Rol> ListarActivos()
        {
            return _rolRepository.ObtenerActivos();
        }

        public Rol? ObtenerPorId(int id)
        {
            return _rolRepository.ObtenerPorId(id);
        }

        public Rol? ObtenerPorNombre(string nombre)
        {
            return _rolRepository.ObtenerPorNombre(nombre);
        }

        public void Guardar(Rol rol)
        {
            if (rol.Id == 0)
            {
                _rolRepository.Crear(rol);
                return;
            }

            _rolRepository.Actualizar(rol);
        }

        public void Eliminar(int id)
        {
            _rolRepository.Eliminar(id);
        }
    }
}