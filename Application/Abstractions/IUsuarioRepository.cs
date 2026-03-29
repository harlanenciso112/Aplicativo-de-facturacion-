using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface IUsuarioRepository
    {
        List<Usuario> ObtenerTodos();
        bool ExisteUsuario(string usuarioLogin);
        bool ExistePassword(string password);
        void Crear(Usuario usuario);
        void Actualizar(Usuario usuario);
        void Eliminar(int id);
        void ActualizarPassword(int id, string password);
    }
}