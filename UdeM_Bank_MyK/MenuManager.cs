using Spectre.Console;

namespace UdeM_Bank_MyK
{
    internal class MenuManager
    {
        enum ClienteOptions
        {
            AñadirCliente,
            EliminarCliente,
            VerListaDeClientes,
            Salir
        }

        enum GrupoAhorroOptions
        {
            AñadirGrupoDeAhorro,
            EliminarGrupoDeAhorro,
            Salir
        }

        enum GrupoAhorroXClienteOptions
        {
            AñadirUsuarioAUnGrupoDeAhorro,
            Salir
        }

        enum MovimientosGrupoAhorroXClienteOptions
        {
            AñadirMovimiento,
            VerHistorialDeUsuario,
            Salir
        }
        
        enum MenuOpciones
        {
            ManejoDeClientes,
            ManejoDeGrupos,
            AsignarGrupoACliente,
            Movimientos,
            PremiarGrupoAhorro,
            PremiarClienteQueMasAporta
        }

        public static void MainMenuManagement()
        {
            //Menú principal
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOpciones>()
                .Title("¿Qué quieres hacer?")
                .AddChoices(
                    MenuOpciones.ManejoDeClientes,
                    MenuOpciones.ManejoDeGrupos,
                    MenuOpciones.AsignarGrupoACliente,
                    MenuOpciones.Movimientos,
                    MenuOpciones.PremiarGrupoAhorro,
                    MenuOpciones.PremiarClienteQueMasAporta
                    ));
            switch ( option ) 
            {
                case MenuOpciones.ManejoDeClientes:
                    ClienteMenuManegement();
                    break;

                case MenuOpciones.ManejoDeGrupos:
                    GruposAhorroMenuManegement();
                    break;

                case MenuOpciones.AsignarGrupoACliente:
                    GrupoAhorroXClienteMenuManegement();
                    break;

                case MenuOpciones.Movimientos:
                    MovimientosGrupoAhorroXClienteMenuManegement();
                    break;

                case MenuOpciones.PremiarGrupoAhorro:
                    EquipoFidelizacion.PremiarGrupoMasRentable();
                    break;

                case MenuOpciones.PremiarClienteQueMasAporta:
                    EquipoFidelizacion.PremiarUsuarioConMasAporte();
                    break;
            }
            // AnsiConsole.Clear();
        }

        public static void ClienteMenuManegement()
        {
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<ClienteOptions>()
            .Title("Qué quieres hacer?")
            .AddChoices(
                ClienteOptions.AñadirCliente,
                ClienteOptions.EliminarCliente,
                ClienteOptions.VerListaDeClientes,
                ClienteOptions.Salir));

            switch (option)
            {
                case ClienteOptions.AñadirCliente:
                    ClienteController.AñadirCliente();
                    break;
                case ClienteOptions.EliminarCliente:
                    ClienteService.DeleteCliente();
                    break;
                case ClienteOptions.VerListaDeClientes:
                    var clientes = ClienteController.GetClientes();
                    UserInterface.VerTablaClientes();
                    break;
                case ClienteOptions.Salir:
                    break;
            }
        }

        public static void GruposAhorroMenuManegement()
        {
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<GrupoAhorroOptions>()
            .Title("Qué quieres hacer?")
            .AddChoices(
                GrupoAhorroOptions.AñadirGrupoDeAhorro,
                GrupoAhorroOptions.EliminarGrupoDeAhorro,
                GrupoAhorroOptions.Salir));

            switch (option)
            {
                case GrupoAhorroOptions.AñadirGrupoDeAhorro:
                    GrupoAhorroController.AñadirGrupoDeAhorro();
                    break;
                case GrupoAhorroOptions.EliminarGrupoDeAhorro:
                    GrupoAhorroController.EliminarGrupoDeAhorro();
                    break;
                case GrupoAhorroOptions.Salir:
                    break;
            }
        }

        public static void GrupoAhorroXClienteMenuManegement()
        {
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<GrupoAhorroXClienteOptions>()
            .Title("Qué quieres hacer?")
            .AddChoices(
                GrupoAhorroXClienteOptions.AñadirUsuarioAUnGrupoDeAhorro,
                GrupoAhorroXClienteOptions.Salir));

            switch (option)
            {
                case GrupoAhorroXClienteOptions.AñadirUsuarioAUnGrupoDeAhorro:
                    GrupoAhorroXClienteController.AñadirUsuarioAUnGrupoDeAhorro();
                    break;
                case GrupoAhorroXClienteOptions.Salir:
                    break;
            }
        }

        public static void MovimientosGrupoAhorroXClienteMenuManegement()
        {
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<MovimientosGrupoAhorroXClienteOptions>()
            .Title("Qué quieres hacer?")
            .AddChoices(
                MovimientosGrupoAhorroXClienteOptions.AñadirMovimiento,
                MovimientosGrupoAhorroXClienteOptions.VerHistorialDeUsuario,
                MovimientosGrupoAhorroXClienteOptions.Salir));

            switch (option)
            {
                case MovimientosGrupoAhorroXClienteOptions.AñadirMovimiento:
                    MovimientosGrupoAhorroXClienteController.AñadirMovimiento();
                    break;
                case MovimientosGrupoAhorroXClienteOptions.VerHistorialDeUsuario:
                    MovimientosGrupoAhorroXClienteController.VerHistorialDeUsuario();
                    break;
                case MovimientosGrupoAhorroXClienteOptions.Salir:
                    break;
            }
        }
    }
}
