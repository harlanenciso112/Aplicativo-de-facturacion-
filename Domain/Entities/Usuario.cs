namespace Pantallas_Sistema_facturacion.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UsuarioLogin { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public string Estado => Activo ? "Activo" : "Inactivo";
        public string UltimoAcceso { get; set; } = string.Empty;
    }
}