using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Infrastructure.Data;
using Pantallas_Sistema_facturacion.Infrastructure.Repositories;
using Pantallas_Sistema_facturacion.Infrastructure.Reports;

namespace Pantallas_Sistema_facturacion.Composition
{
    public static class AppContainer
    {
        private static AccesoDatos? _db;
        private static AuthService? _authService;
        private static EmpleadoService? _empleadoService;
        private static UsuarioService? _usuarioService;
        private static ProductoService? _productoService;
        private static CategoriaProductoService? _categoriaProductoService;
        private static ClienteService? _clienteService;
        private static FacturaService? _facturaService;
        private static InformeFacturasService? _informeFacturasService;
        private static RolService? _rolService;

        public static void Initialize()
        {
            _db = new AccesoDatos();
            var rolRepository = new MySqlRolRepository(_db);
            var usuarioRepository = new MySqlUsuarioRepository(_db);

            _authService = new AuthService(new MySqlAuthRepository(_db));
            _rolService = new RolService(rolRepository);
            _usuarioService = new UsuarioService(usuarioRepository);
            _empleadoService = new EmpleadoService(new MySqlEmpleadoRepository(_db), usuarioRepository, _usuarioService, rolRepository);
            _productoService = new ProductoService(new MySqlProductoRepository(_db));
            _categoriaProductoService = new CategoriaProductoService(new MySqlCategoriaProductoRepository(_db));
            _clienteService = new ClienteService(new MySqlClienteRepository(_db));
            _facturaService = new FacturaService(new MySqlFacturaRepository(_db));
            _informeFacturasService = new InformeFacturasService(_facturaService, new PdfSharpFacturaPdfExporter());
        }

        public static AuthService AuthService => _authService ?? throw new System.InvalidOperationException("Container no inicializado");
        public static EmpleadoService EmpleadoService => _empleadoService ?? throw new System.InvalidOperationException("Container no inicializado");
        public static UsuarioService UsuarioService => _usuarioService ?? throw new System.InvalidOperationException("Container no inicializado");
        public static RolService RolService => _rolService ?? throw new System.InvalidOperationException("Container no inicializado");
        public static ProductoService ProductoService => _productoService ?? throw new System.InvalidOperationException("Container no inicializado");
        public static CategoriaProductoService CategoriaProductoService => _categoriaProductoService ?? throw new System.InvalidOperationException("Container no inicializado");
        public static ClienteService ClienteService => _clienteService ?? throw new System.InvalidOperationException("Container no inicializado");
        public static FacturaService FacturaService => _facturaService ?? throw new System.InvalidOperationException("Container no inicializado");
        public static InformeFacturasService InformeFacturasService => _informeFacturasService ?? throw new System.InvalidOperationException("Container no inicializado");
    }
}