using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public bool Ingresar(string usuario, string password)
        {
            return _authRepository.ValidarCredenciales(usuario, password);
        }

        public Usuario? Autenticar(string usuario, string password)
        {
            return _authRepository.ObtenerUsuarioAutenticado(usuario, password);
        }
    }
}