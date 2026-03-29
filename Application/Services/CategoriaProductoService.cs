using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class CategoriaProductoService
    {
        private readonly ICategoriaProductoRepository _categoriaRepository;

        public CategoriaProductoService(ICategoriaProductoRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public List<CategoriaProducto> Listar(string filtro = "")
        {
            return _categoriaRepository.ObtenerTodos(filtro);
        }

        public CategoriaProducto? ObtenerPorId(int id)
        {
            return _categoriaRepository.ObtenerPorId(id);
        }

        public void Guardar(CategoriaProducto categoria)
        {
            if (categoria.Id == 0)
            {
                _categoriaRepository.Crear(categoria);
            }
            else
            {
                _categoriaRepository.Actualizar(categoria);
            }
        }

        public void Eliminar(int id)
        {
            _categoriaRepository.Eliminar(id);
        }
    }
}