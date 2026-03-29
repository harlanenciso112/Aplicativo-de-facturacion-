using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public static class SesionUsuario
    {
        public static Usuario? Actual { get; private set; }

        public static bool HaySesionActiva => Actual != null;

        public static void Iniciar(Usuario usuario)
        {
            Actual = usuario;
        }

        public static void Cerrar()
        {
            Actual = null;
        }
    }
}
