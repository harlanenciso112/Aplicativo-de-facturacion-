using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface IAuthRepository
    {
        bool ValidarCredenciales(string usuario, string password);
        Usuario? ObtenerUsuarioAutenticado(string usuario, string password);
    }
}