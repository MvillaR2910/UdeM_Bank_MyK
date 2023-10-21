using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdeM_Bank_MyK
{
    internal class GrupoAhorroXClienteController
    {
        internal static void AñadirUsuarioAUnGrupoDeAhorro()
        {
            var grupoAhorro = GrupoAhorroService.GetGrupoAhorroOptionInput();

            if (grupoAhorro != null)
            {
                var cliente = ClienteService.GetClienteOptionInput();

                if (cliente != null)
                {
                    // Verificar que el cliente no pertenezca a más de 3 grupos
                    if (cliente.NroGruposPertenecientes < 3)
                    {
                        using var db = new UdemBankContext();

                        // Crear la relación entre el cliente y el grupo de ahorro
                        var relacion = new GrupoAhorroXCliente { IdGrupoAhorro = grupoAhorro.IdGrupoAhorro, IdCliente = cliente.IdCliente, AporteCliente = 0 };
                        db.Add(relacion);
                        db.SaveChanges();

                        // Incrementar el número de grupos a los que pertenece el cliente
                        cliente.NroGruposPertenecientes++;
                        db.Update(cliente);
                        db.SaveChanges();

                        AnsiConsole.MarkupLine($"El usuario {cliente.Nombre} ha sido agregado al grupo {grupoAhorro.NombreGrupo}.");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"El usuario {cliente.Nombre} ya pertenece a 3 grupos y no se puede agregar a más.");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("No se seleccionó un cliente o el cliente no existe.");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("No se seleccionó un grupo de ahorro o el grupo no existe.");
            }
        }
    }
}
