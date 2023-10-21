using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdeM_Bank_MyK
{
    internal class MovimientoService
    {
        internal static void AñadirFondosACuenta()
        {
            var cliente = ClienteService.GetClienteOptionInput();

            if (cliente != null)
            {
                var montoAAgregar = AnsiConsole.Ask<float>("Monto a agregar a la cuenta:");
                if (montoAAgregar > 0)
                {
                    using var db = new UdemBankContext();

                    // Crear un registro de movimiento para la adición de fondos
                    var movimiento = new MovimientosGrupoAhorroXCliente
                    {
                        IdCliente = cliente.IdCliente,
                        IdGrupoDestinatario = 0,  // Si no hay grupo destinatario, puedes establecerlo en 0 o manejarlo de otra forma
                        Tipo = "Fondos Añadidos",
                        Fecha = DateOnly.FromDateTime(DateTime.Now),
                        Hora = DateTime.Now.ToString("HH:mm:ss"),
                        Monto = montoAAgregar
                    };

                    // Actualizar el saldo del cliente en la base de datos
                    cliente.Saldo += montoAAgregar;

                    // Agregar el movimiento a la tabla de movimientos
                    db.Add(movimiento);

                    // Actualizar el cliente en la base de datos
                    db.Update(cliente);
                    db.SaveChanges();

                    AnsiConsole.MarkupLine($"Se agregaron ${montoAAgregar} a la cuenta de {cliente.Nombre}. Nuevo saldo: ${cliente.Saldo}");
                }
                else
                {
                    AnsiConsole.MarkupLine("El monto a agregar debe ser mayor que cero.");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("No se seleccionó un cliente o el cliente no existe.");
            }
        }

        static internal void AñadirFondosAGrupoAhorro()
        {
            var cliente = ClienteService.GetClienteOptionInput();

            if (cliente != null)
            {
                var gruposAhorroCliente = GrupoAhorroService.GetGruposAhorroDeCliente(cliente);

                if (gruposAhorroCliente.Count > 0)
                {
                    var grupoAhorro = GrupoAhorroService.GetGrupoAhorroOptionInputEspecifico(gruposAhorroCliente);

                    if (grupoAhorro != null)
                    {
                        var montoAAgregar = AnsiConsole.Ask<float>("Monto a agregar al grupo de ahorro:");

                        if (montoAAgregar > 0)
                        {
                            if (montoAAgregar <= cliente.Saldo)
                            {
                                using var db = new UdemBankContext();

                                // Crear un registro de movimiento para la adición de fondos
                                var movimiento = new MovimientosGrupoAhorroXCliente
                                {
                                    IdCliente = cliente.IdCliente,
                                    IdGrupoDestinatario = grupoAhorro.IdGrupoAhorro,
                                    Tipo = "Añadir Fondos a Grupo de Ahorro",
                                    Fecha = DateOnly.FromDateTime(DateTime.Now),
                                    Hora = DateTime.Now.ToString("HH:mm:ss"),
                                    Monto = montoAAgregar
                                };

                                // Actualizar el aporte del cliente en el grupo de ahorro en la base de datos
                                var relacion = db.GrupoAhorroXCliente.FirstOrDefault(x => x.IdCliente == cliente.IdCliente && x.IdGrupoAhorro == grupoAhorro.IdGrupoAhorro);
                                if (relacion != null)
                                {
                                    relacion.AporteCliente += montoAAgregar;
                                    db.Update(relacion);

                                    // Actualizar el atributo Capital en la tabla GrupoAhorros
                                    grupoAhorro.Capital += montoAAgregar;
                                    db.Update(grupoAhorro);

                                    // Actualizar el saldo del cliente
                                    cliente.Saldo -= montoAAgregar;
                                    db.Update(cliente);

                                    db.Add(movimiento);
                                    db.SaveChanges();

                                    AnsiConsole.MarkupLine($"Se agregaron ${montoAAgregar} al grupo de ahorro '{grupoAhorro.NombreGrupo}' de {cliente.Nombre}. Nuevo aporte: ${relacion.AporteCliente}. Saldo actual del cliente: ${cliente.Saldo}");
                                }
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("El monto a agregar excede el saldo disponible en la cuenta del cliente.");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("El monto a agregar debe ser mayor que cero.");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("No se seleccionó un grupo de ahorro o el grupo no existe para el cliente seleccionado.");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine($"El cliente {cliente.Nombre} no pertenece a ningún grupo de ahorro.");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("No se seleccionó un cliente o el cliente no existe.");
            }
        }
    }
}
