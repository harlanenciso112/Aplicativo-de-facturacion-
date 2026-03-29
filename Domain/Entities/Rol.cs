namespace Pantallas_Sistema_facturacion.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public string Estado => Activo ? "Activo" : "Inactivo";
    }
}