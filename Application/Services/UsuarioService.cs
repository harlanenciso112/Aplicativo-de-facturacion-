using System.Collections.Generic;
using System.Linq;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public List<Usuario> Listar()
        {
            return _usuarioRepository.ObtenerTodos();
        }

        public void Crear(Usuario usuario)
        {
            _usuarioRepository.Crear(usuario);
        }

        public void Actualizar(Usuario usuario)
        {
            _usuarioRepository.Actualizar(usuario);
        }

        public void Eliminar(int id)
        {
            _usuarioRepository.Eliminar(id);
        }

        public void CambiarPassword(int id, string password)
        {
            _usuarioRepository.ActualizarPassword(id, password);
        }

        public string GenerarUsuarioDisponible(string nombreBase)
        {
            var limpio = new string((nombreBase ?? string.Empty)
                .Where(char.IsLetterOrDigit)
                .ToArray())
                .ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(limpio))
            {
                limpio = "empleado";
            }

            if (limpio.Length > 20)
            {
                limpio = limpio.Substring(0, 20);
            }

            var candidato = limpio;
            var sufijo = 1;
            while (_usuarioRepository.ExisteUsuario(candidato))
            {
                candidato = $"{limpio}{sufijo}";
                sufijo++;
            }

            return candidato;
        }

        public string GenerarPasswordSimpleNoRepetida()
        {
            var valor = 1234;
            while (_usuarioRepository.ExistePassword(valor.ToString()))
            {
                valor++;
            }

            return valor.ToString();
        }
    }
}