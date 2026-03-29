using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        AppContainer.Initialize();
        System.Windows.Forms.Application.Run(new frmLogin());
    }
}