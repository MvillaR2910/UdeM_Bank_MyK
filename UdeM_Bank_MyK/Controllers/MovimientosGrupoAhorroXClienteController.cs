using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace UdeM_Bank_MyK
{
    internal class MovimientosGrupoAhorroXClienteController
    {
        enum MovimientosOptions
        {
            AñadirFondosATuCuenta,
            AñadirFondosAUnGrupoAhorro,
            PedirPrestamo,
            PedirPrestamoDeOtroGrupo,
            PagarCuotas,
            Salir
        }
        internal static void AñadirMovimiento()
        {
            //Menú principal
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<MovimientosOptions>()
                .Title("¿Qué quieres hacer?")
                .AddChoices(
                    MovimientosOptions.AñadirFondosATuCuenta,
                    MovimientosOptions.AñadirFondosAUnGrupoAhorro,
                    MovimientosOptions.PedirPrestamo,
                    MovimientosOptions.PedirPrestamoDeOtroGrupo,
                    MovimientosOptions.PagarCuotas,
                    MovimientosOptions.Salir
                    ));
            switch (option)
            {
                case MovimientosOptions.AñadirFondosATuCuenta:
                    MovimientoService.AñadirFondosACuenta();
                    break;
                case MovimientosOptions.AñadirFondosAUnGrupoAhorro:
                    MovimientoService.AñadirFondosAGrupoAhorro();
                    break;
                case MovimientosOptions.PedirPrestamo:
                    PrestamoService.RealizarPrestamo();
                    break;
                case MovimientosOptions.PedirPrestamoDeOtroGrupo:
                    PrestamoService.RealizarPrestamoDeOtroGrupo();
                    break;
                case MovimientosOptions.PagarCuotas:
                    PrestamoService.PagoCuotas();
                    break;
                case MovimientosOptions.Salir:
                    break;
            }
        }

        internal static void VerHistorialDeUsuario()
        {
            UserInterface.VerTablaClientes();
        }
    }
}
