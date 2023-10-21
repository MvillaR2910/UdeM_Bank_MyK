using ConsoleTables;
using Spectre.Console;

namespace UdeM_Bank_MyK
{
    static internal class UserInterface
    {
        static internal void VerTablaClientes()
        {
            var cliente = ClienteService.GetClienteOptionInput();

            if (cliente != null)
            {
                using var db = new UdemBankContext();

                var movimientosCliente = db.Movimientos
                    .Where(x => x.IdCliente == cliente.IdCliente)
                    .OrderBy(x => x.Fecha)
                    .ToList();

                if (movimientosCliente.Count > 0)
                {
                    // Crear una tabla para mostrar el historial de movimientos
                    var table = new ConsoleTable("ID", "Tipo", "Fecha", "Hora", "Monto");

                    foreach (var movimiento in movimientosCliente)
                    {
                        table.AddRow(
                            movimiento.IdMovimiento,
                            movimiento.Tipo,
                            movimiento.Fecha.ToString(),
                            movimiento.Hora,
                            movimiento.Monto.ToString("C")); 
                    }
                    Console.WriteLine(table);
                }
                else
                {
                    AnsiConsole.MarkupLine($"No hay movimientos registrados para el cliente {cliente.Nombre}.");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("No se seleccionó un cliente o el cliente no existe.");
            }
        }
    }
}
