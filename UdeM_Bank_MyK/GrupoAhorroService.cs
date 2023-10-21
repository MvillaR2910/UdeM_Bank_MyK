using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdeM_Bank_MyK
{
    internal class GrupoAhorroService
    {
        static internal GrupoAhorro GetGrupoAhorroOptionInput()
        {
            var grupo_ahorros = GrupoAhorroController.GetGrupoAhorros();
            var opciones = grupo_ahorros.Select(x => $"{x.IdGrupoAhorro}: {x.NombreGrupo}").ToArray();

            var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Escoge un grupo de ahorro:")
                .AddChoices(opciones));

            // Extraer el ID del grupo de ahorro de la opción seleccionada
            var selectedGrupoAhorroId = int.Parse(option.Split(':')[0].Trim());

            var grupo_ahorro = GrupoAhorroController.GetGrupoAhorroById(selectedGrupoAhorroId);
            return grupo_ahorro;
        }

        static internal GrupoAhorro GetGrupoAhorroOptionInputEspecifico(List<GrupoAhorro> gruposAhorro)
        {
            if (gruposAhorro.Count > 0)
            {
                var opciones = gruposAhorro.Select(x => $"{x.IdGrupoAhorro}: {x.NombreGrupo}").ToArray();

                var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Escoge un grupo de ahorro:")
                    .AddChoices(opciones));

                // Extraer el ID del grupo de ahorro de la opción seleccionada
                var selectedGrupoAhorroId = int.Parse(option.Split(':')[0].Trim());

                var grupoAhorro = gruposAhorro.FirstOrDefault(x => x.IdGrupoAhorro == selectedGrupoAhorroId);
                return grupoAhorro;
            }
            else
            {
                AnsiConsole.MarkupLine("No hay grupos de ahorro disponibles.");
                return null;
            }
        }


        static internal List<GrupoAhorro> GetGruposAhorroDeCliente(Cliente cliente)
        {
            using var db = new UdemBankContext();
            var grupos = db.GrupoAhorroXCliente
                .Where(x => x.IdCliente == cliente.IdCliente)
                .Select(x => x.GrupoAhorro)
                .ToList();

            return grupos;
        }

        public static void AñadirGrupoDeAhorro()
        {
            var name = AnsiConsole.Ask<string>("Nombre del grupo:");
            var capital = AnsiConsole.Ask<float>("Ingrese el capital (Inicie en cero => 0):");
            using var db = new UdemBankContext();
            var nuevoGrupo = new GrupoAhorro { NombreGrupo = name, Capital = capital };
            db.Add(nuevoGrupo);
            db.SaveChanges();
            AnsiConsole.MarkupLine($"El grupo de ahorro '{name}' ha sido creado.");
            // Agregar usuario al grupo
            Console.WriteLine("Quién va a crear el grupo?");
            var cliente = ClienteService.GetClienteOptionInput();
            if (cliente != null)
            {
                // Verificar si el cliente ya está en 3 grupos
                if (cliente.NroGruposPertenecientes >= 3)
                {
                    AnsiConsole.MarkupLine($"El cliente {cliente.Nombre} ya pertenece a 3 grupos y no se puede agregar a más grupos.");
                }
                else
                {
                    // Crear la relación entre el usuario y el grupo de ahorro
                    var relacion = new GrupoAhorroXCliente { IdGrupoAhorro = nuevoGrupo.IdGrupoAhorro, IdCliente = cliente.IdCliente, AporteCliente = 0 };
                    db.Add(relacion);
                    // Incrementar el contador NroGruposPertenecientes del cliente
                    cliente.NroGruposPertenecientes++;
                    db.Update(cliente);
                    db.SaveChanges();
                    AnsiConsole.MarkupLine($"El usuario {cliente.Nombre} ha sido agregado al grupo {name}.");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("No se seleccionó un cliente o el cliente no existe.");
            }
        }

        static internal void DisolverGrupoAhorro()
        {
            var usuario = ClienteService.GetClienteOptionInput();

            if (usuario != null)
            {
                var gruposAhorroUsuario = GrupoAhorroService.GetGruposAhorroDeCliente(usuario);

                if (gruposAhorroUsuario.Count > 0)
                {
                    var grupoSeleccionado = GrupoAhorroService.GetGrupoAhorroOptionInputEspecifico(gruposAhorroUsuario);

                    if (grupoSeleccionado != null)
                    {
                        using var db = new UdemBankContext();

                        // Calcular la comisión del banco (5% del monto total)
                        var montoTotal = grupoSeleccionado.Capital;
                        var comisionBanco = montoTotal * 0.05;

                        // Obtener la lista de usuarios y sus aportes al grupo
                        var usuariosEnGrupo = db.GrupoAhorroXCliente
                            .Where(x => x.IdGrupoAhorro == grupoSeleccionado.IdGrupoAhorro)
                            .ToList();

                        // Calcular el monto restante después de la comisión del banco
                        var montoRestante = montoTotal - comisionBanco;

                        // Dividir el monto restante entre los usuarios según sus aportes
                        foreach (var usuarioEnGrupo in usuariosEnGrupo)
                        {
                            var porcentajeAporte = usuarioEnGrupo.AporteCliente / montoTotal;
                            var montoARecibir = montoRestante * porcentajeAporte;

                            var cliente = db.Clientes.Find(usuarioEnGrupo.IdCliente);
                            if (cliente != null)
                            {
                                cliente.NroGruposPertenecientes--; // Restar uno al atributo
                                db.Update(cliente);
                            }

                            // Actualizar el saldo del usuario
                            var usuarioActual = db.Clientes.Find(usuarioEnGrupo.IdCliente);
                            usuarioActual.Saldo += (float)montoARecibir;

                            // Eliminar la relación del usuario con el grupo
                            db.GrupoAhorroXCliente.Remove(usuarioEnGrupo);
                        }

                        // Eliminar el grupo de ahorro
                        db.GruposAhorros.Remove(grupoSeleccionado);
                        db.SaveChanges();

                        AnsiConsole.MarkupLine($"El grupo de ahorro '{grupoSeleccionado.NombreGrupo}' ha sido disuelto.");
                        AnsiConsole.MarkupLine($"El banco se queda con una comisión de ${comisionBanco}.");
                        AnsiConsole.MarkupLine("El monto restante se ha dividido entre los usuarios y se ha transferido a sus cuentas personales.");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("No se seleccionó un grupo de ahorro o el grupo no existe para el cliente seleccionado.");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine($"El cliente {usuario.Nombre} no pertenece a ningún grupo de ahorro.");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("No se seleccionó un cliente o el cliente no existe.");
            }
        }

    }
}
