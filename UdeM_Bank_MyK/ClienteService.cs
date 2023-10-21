using Spectre.Console;

namespace UdeM_Bank_MyK
{
    internal class ClienteService
    {
        internal static void DeleteCliente()
        {
            var cliente = GetClienteOptionInput();
            ClienteController.EliminarCliente(cliente);
        }
        internal static void AñadirUsuario()
        {
            var nombre = AnsiConsole.Ask<string>("Nombre del cliente:");
            var saldo = AnsiConsole.Ask<float>("Saldo inicial:");
            var nro_grupos_pertenecientes = 0;
            var comisionReducida = false;

            using var db = new UdemBankContext();
            db.Add(new Cliente { Nombre = nombre, Saldo = saldo, NroGruposPertenecientes = nro_grupos_pertenecientes, ComisionReducida = comisionReducida });
            db.SaveChanges();
        }
        static internal Cliente GetClienteOptionInput()
        {
            var clientes = ClienteController.GetClientes();
            var opciones = clientes.Select(x => $"{x.IdCliente}: {x.Nombre}").ToArray();

            var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Escoge un cliente")
                .AddChoices(opciones));

            // Extraer el ID del cliente de la opción seleccionada
            var selectedClientId = int.Parse(option.Split(':')[0].Trim());

            var cliente = ClienteController.GetClienteById(selectedClientId);
            return cliente;
        }
    }
}
