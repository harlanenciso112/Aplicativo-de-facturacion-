using System.Collections.Generic;
using System.Linq;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class ResultadoRegistroEmpleado
    {
        public bool UsuarioCreado { get; set; }
        public string UsuarioLogin { get; set; } = string.Empty;
        public string PasswordTemporal { get; set; } = string.Empty;
    }

    public class EmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UsuarioService _usuarioService;
        private readonly IRolRepository _rolRepository;

        public EmpleadoService(
            IEmpleadoRepository empleadoRepository,
            IUsuarioRepository usuarioRepository,
            UsuarioService usuarioService,
            IRolRepository rolRepository)
        {
            _empleadoRepository = empleadoRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _rolRepository = rolRepository;
        }

        public List<Empleado> Listar(string filtro = "")
        {
            return _empleadoRepository.ObtenerTodos(filtro);
        }

        public Empleado? ObtenerPorId(int id)
        {
            return _empleadoRepository.ObtenerPorId(id);
        }

        public ResultadoRegistroEmpleado Guardar(Empleado empleado)
        {
            if (empleado.Id == 0)
            {
                _empleadoRepository.Crear(empleado);
                return CrearUsuarioDeEmpleado(empleado);
            }

            _empleadoRepository.Actualizar(empleado);
            return new ResultadoRegistroEmpleado();
        }

        public void Eliminar(int id)
        {
            _empleadoRepository.Eliminar(id);
        }

        private ResultadoRegistroEmpleado CrearUsuarioDeEmpleado(Empleado empleado)
        {
            var baseUsuario = ObtenerBaseUsuario(empleado);
            var usuarioLogin = _usuarioService.GenerarUsuarioDisponible(baseUsuario);
            var passwordTemporal = _usuarioService.GenerarPasswordSimpleNoRepetida();
            var rolId = ObtenerRolIdParaEmpleado(empleado.Rol);

            _usuarioRepository.Crear(new Usuario
            {
                UsuarioLogin = usuarioLogin,
                Password = passwordTemporal,
                Nombre = empleado.Nombre,
                RolId = rolId,
                Activo = empleado.Activo
            });

            return new ResultadoRegistroEmpleado
            {
                UsuarioCreado = true,
                UsuarioLogin = usuarioLogin,
                PasswordTemporal = passwordTemporal
            };
        }

        private static string ObtenerBaseUsuario(Empleado empleado)
        {
            if (!string.IsNullOrWhiteSpace(empleado.Cedula))
            {
                return empleado.Cedula.Trim();
            }

            var nombre = empleado.Nombre ?? string.Empty;
            var primeraPalabra = nombre.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault() ?? "empleado";

            return primeraPalabra;
        }

        private int ObtenerRolIdParaEmpleado(string nombreRol)
        {
            if (!string.IsNullOrWhiteSpace(nombreRol))
            {
                var rol = _rolRepository.ObtenerPorNombre(nombreRol.Trim());
                if (rol != null)
                {
                    return rol.Id;
                }
            }

            var rolDefault = _rolRepository.ObtenerPorNombre("Vendedor")
                ?? _rolRepository.ObtenerPorNombre("Administrador")
                ?? _rolRepository.ObtenerActivos().FirstOrDefault();

            return rolDefault?.Id ?? 1;
        }
    }
}