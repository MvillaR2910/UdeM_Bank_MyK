using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdeM_Bank_MyK
{
    internal class PrestamoService
    {
        static internal Prestamo GetPrestamoOptionInput(List<Prestamo> prestamosSinPagar)
        {
            // Crear una lista de opciones para mostrar los préstamos sin pagar
            var opciones = prestamosSinPagar.Select(p => $"ID: {p.IdPrestamo}, Grupo: {p.IdGrupoAhorro}, Interés: {p.Interes}, Meses a Pagar: {p.MesesAPagar}, Cantidad: {p.Cantidad}").ToList();

            if (opciones.Count == 0)
            {
                AnsiConsole.MarkupLine("No tienes préstamos sin pagar.");
                return null;
            }

            // Preguntar al usuario que seleccione un préstamo
            var selectedOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Selecciona un préstamo sin pagar:")
                .PageSize(5) // Número de elementos a mostrar en cada página
                .AddChoices(opciones));

            // Analizar la opción seleccionada para obtener el ID del grupo
            var idGrupoAhorro = int.Parse(selectedOption.Split(',')[1].Split(':')[1].Trim());

            // Buscar el préstamo en la lista por su ID y devolverlo
            var prestamoSeleccionado = prestamosSinPagar.FirstOrDefault(p => p.IdGrupoAhorro == idGrupoAhorro);
            return prestamoSeleccionado;
        }

        static internal void RealizarPrestamo()
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
                        using var db = new UdemBankContext();

                        // Mostrar el saldo actual del cliente y el capital del grupo de ahorro
                        var relacion = db.GrupoAhorroXCliente.FirstOrDefault(x => x.IdCliente == cliente.IdCliente && x.IdGrupoAhorro == grupoAhorro.IdGrupoAhorro);
                        var saldoCliente = cliente.Saldo;
                        var capitalGrupo = grupoAhorro.Capital;

                        AnsiConsole.MarkupLine($"Saldo actual del cliente {cliente.Nombre}: ${saldoCliente}");
                        AnsiConsole.MarkupLine($"Capital del grupo de ahorro '{grupoAhorro.NombreGrupo}': ${capitalGrupo}");

                        var montoPrestamo = AnsiConsole.Ask<float>("Monto del préstamo a solicitar:");

                        if (montoPrestamo > 0)
                        {
                            // Verificar que el capital del grupo sea mayor o igual al monto del préstamo
                            if (capitalGrupo >= montoPrestamo)
                            {
                                var plazoPrestamo = AnsiConsole.Ask<int>("Plazo de pago en meses (entre 2 y 12 meses):");

                                if (plazoPrestamo >= 2 && plazoPrestamo <= 12)
                                {
                                    // Verificar si el cliente tiene comisión reducida
                                    if (cliente.ComisionReducida)
                                    {
                                        // Calcular el interés del préstamo con comisión reducida (2% mensual)
                                        var interes = montoPrestamo * 0.02 * plazoPrestamo;
                                    }
                                    else
                                    {
                                        // Calcular el interés del préstamo estándar (3% mensual)
                                        var interes = montoPrestamo * 0.03 * plazoPrestamo;
                                    }

                                    // Crear un registro de préstamo en la tabla Prestamo
                                    var prestamo = new Prestamo
                                    {
                                        IdCliente = cliente.IdCliente,
                                        IdGrupoAhorro = grupoAhorro.IdGrupoAhorro,
                                        Interes = cliente.ComisionReducida ? "2%" : "3%",
                                        MesesAPagar = plazoPrestamo,
                                        Estado = "Sin pagar",
                                        Cantidad = montoPrestamo
                                    };

                                    db.Add(prestamo);
                                    db.SaveChanges();

                                    // Actualizar el valor de la comision reducida
                                    cliente.ComisionReducida = false;
                                    db.Update(cliente);

                                    // Actualizar el capital del grupo de ahorro restando el monto del préstamo
                                    grupoAhorro.Capital -= montoPrestamo;
                                    db.Update(grupoAhorro);

                                    // Actualizar el saldo del cliente sumando el monto del préstamo
                                    cliente.Saldo += montoPrestamo;
                                    db.Update(cliente);

                                    AnsiConsole.MarkupLine($"El préstamo de ${montoPrestamo} ha sido aprobado.");
                                    AnsiConsole.MarkupLine($"El saldo actual del cliente {cliente.Nombre}: ${cliente.Saldo}");

                                    // Crear un registro de movimiento para el préstamo
                                    var movimientoPrestamo = new MovimientosGrupoAhorroXCliente
                                    {
                                        IdCliente = cliente.IdCliente,
                                        IdGrupoDestinatario = grupoAhorro.IdGrupoAhorro,
                                        Tipo = "Préstamo",
                                        Fecha = DateOnly.FromDateTime(DateTime.Now),
                                        Hora = DateTime.Now.ToString("HH:mm:ss"),
                                        Monto = montoPrestamo
                                    };

                                    db.Add(movimientoPrestamo);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("El plazo de pago debe estar entre 2 y 12 meses.");
                                }
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("El monto del préstamo supera el capital del grupo de ahorro. No es posible otorgar el préstamo.");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("El monto del préstamo debe ser mayor que cero.");
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

        static internal void RealizarPrestamoDeOtroGrupo()
        {
            var cliente = ClienteService.GetClienteOptionInput();

            if (cliente != null)
            {
                var gruposAhorroCliente = GrupoAhorroService.GetGruposAhorroDeCliente(cliente);

                if (gruposAhorroCliente.Count > 0)
                {
                    using var db = new UdemBankContext();
                    List<GrupoAhorro> gruposPosibles = new List<GrupoAhorro>();

                    // Buscar grupos de ahorro que contengan a otros usuarios de los grupos del cliente
                    foreach (var grupoAhorroCliente in gruposAhorroCliente)
                    {
                        var otrosUsuariosEnGrupo = db.GrupoAhorroXCliente
                            .Where(x => x.IdGrupoAhorro == grupoAhorroCliente.IdGrupoAhorro && x.IdCliente != cliente.IdCliente)
                            .Select(x => x.IdCliente)
                            .ToList();

                        var gruposConOtrosUsuarios = db.GrupoAhorroXCliente
                            .Where(x => x.IdCliente != cliente.IdCliente && otrosUsuariosEnGrupo.Contains(x.IdCliente))
                            .Select(x => x.IdGrupoAhorro)
                            .ToList();

                        var gruposPotenciales = db.GruposAhorros
                            .Where(x => gruposConOtrosUsuarios.Contains(x.IdGrupoAhorro) && x.IdGrupoAhorro != grupoAhorroCliente.IdGrupoAhorro)
                            .ToList();

                        gruposPosibles.AddRange(gruposPotenciales);
                    }

                    if (gruposPosibles.Count > 0)
                    {
                        var grupoAhorro = GrupoAhorroService.GetGrupoAhorroOptionInputEspecifico(gruposPosibles);

                        if (grupoAhorro != null)
                        {
                            // Mostrar el saldo actual del cliente y el capital del grupo de ahorro
                            var relacion = db.GrupoAhorroXCliente.FirstOrDefault(x => x.IdCliente == cliente.IdCliente && x.IdGrupoAhorro == grupoAhorro.IdGrupoAhorro);
                            var saldoCliente = cliente.Saldo;
                            var capitalGrupo = grupoAhorro.Capital;

                            AnsiConsole.MarkupLine($"Saldo actual del cliente {cliente.Nombre}: ${saldoCliente}");
                            AnsiConsole.MarkupLine($"Capital del grupo de ahorro '{grupoAhorro.NombreGrupo}': ${capitalGrupo}");

                            var montoPrestamo = AnsiConsole.Ask<float>("Monto del préstamo a solicitar:");
                            var plazoPrestamo = AnsiConsole.Ask<int>("Plazo de pago en meses:");

                            if (montoPrestamo > 0)
                            {
                                // Actualizar el capital del grupo de ahorro restando el monto del préstamo
                                grupoAhorro.Capital -= montoPrestamo;
                                db.Update(grupoAhorro);

                                // Actualizar el saldo del cliente sumando el monto del préstamo
                                cliente.Saldo += montoPrestamo;
                                db.Update(cliente);

                                // Verificar si el cliente tiene comisión reducida
                                if (cliente.ComisionReducida)
                                {
                                    // Calcular el interés del préstamo con comisión reducida (2% mensual)
                                    var interes = montoPrestamo * 0.04 * plazoPrestamo;
                                }
                                else
                                {
                                    // Calcular el interés del préstamo estándar (3% mensual)
                                    var interes = montoPrestamo * 0.05 * plazoPrestamo;
                                }

                                // Crear un registro de préstamo en la tabla Prestamo
                                var prestamo = new Prestamo
                                {
                                    IdCliente = cliente.IdCliente,
                                    IdGrupoAhorro = grupoAhorro.IdGrupoAhorro,
                                    Interes = cliente.ComisionReducida ? "4%" : "5%",
                                    MesesAPagar = plazoPrestamo,
                                    Estado = "Sin pagar",
                                    Cantidad = montoPrestamo
                                };

                                db.Add(prestamo);
                                db.SaveChanges();

                                // Actualizar el valor de la comision reducida
                                cliente.ComisionReducida = false;
                                db.Update(cliente);

                                // Crear un registro de movimiento para el préstamo
                                var movimientoPrestamo = new MovimientosGrupoAhorroXCliente
                                {
                                    IdCliente = cliente.IdCliente,
                                    IdGrupoDestinatario = grupoAhorro.IdGrupoAhorro,
                                    Tipo = "Préstamo de otro grupo",
                                    Fecha = DateOnly.FromDateTime(DateTime.Now),
                                    Hora = DateTime.Now.ToString("HH:mm:ss"),
                                    Monto = montoPrestamo
                                };

                                db.Add(movimientoPrestamo);
                                db.SaveChanges();

                                AnsiConsole.MarkupLine($"El préstamo de ${montoPrestamo} ha sido aprobado.");
                                AnsiConsole.MarkupLine($"El saldo actual del cliente {cliente.Nombre}: ${cliente.Saldo}");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("El monto del préstamo debe ser mayor que cero.");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("No se seleccionó un grupo de ahorro o el grupo no existe para el cliente seleccionado.");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("No hay grupos de ahorro disponibles para solicitar un préstamo.");
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

        static internal void PagoCuotas()
        {
            var cliente = ClienteService.GetClienteOptionInput();

            if (cliente != null)
            {
                using var db = new UdemBankContext();

                // Obtener los préstamos pendientes de pago para el cliente
                var prestamosSinPagar = db.Prestamos
                    .Where(p => p.IdCliente == cliente.IdCliente && p.Estado == "Sin pagar")
                    .ToList();

                if (prestamosSinPagar.Count > 0)
                {
                    var prestamoAPagar = GetPrestamoOptionInput(prestamosSinPagar);

                    if (prestamoAPagar != null)
                    {
                        var plazoPrestamo = prestamoAPagar.MesesAPagar;

                        float cantidadPrestamo = prestamoAPagar.Cantidad; // Obtener la cantidad del préstamo como float
                        // Determinar el interés según el atributo "Interes"
                        double interesMensual = 0.0;

                        switch (prestamoAPagar.Interes)
                        {
                            case "2%":
                                interesMensual = 0.02;
                                break;
                            case "3%":
                                interesMensual = 0.03;
                                break;
                            case "4%":
                                interesMensual = 0.04;
                                break;
                            case "5%":
                                interesMensual = 0.05;
                                break;
                        }

                        // Calcular el monto total a pagar
                        double montoTotal = prestamoAPagar.Cantidad + (prestamoAPagar.Cantidad * interesMensual * plazoPrestamo);

                        // Asegurarse de que el resultado sea de tipo float si es necesario
                        float montoTotalComoFloat = (float)montoTotal;

                        AnsiConsole.MarkupLine($"Monto total a pagar por el préstamo: ${montoTotal}");

                        // Preguntar al usuario si desea pagar el préstamo
                        var respuesta = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Desea pagar el prestamo? (Se descontara de tu saldo!)")
                        .AddChoices(
                            "SI",
                            "NO"));

                        if (respuesta == "SI")
                        {
                            // Verificar que el saldo del cliente sea suficiente
                            if (cliente.Saldo >= montoTotalComoFloat)
                            {
                                // Actualizar el saldo del cliente
                                cliente.Saldo -= montoTotalComoFloat;
                                db.Update(cliente);

                                // Actualizar el capital del grupo de ahorro
                                var grupoAhorro = db.GruposAhorros.FirstOrDefault(g => g.IdGrupoAhorro == prestamoAPagar.IdGrupoAhorro);
                                grupoAhorro.Capital += montoTotalComoFloat;
                                db.Update(grupoAhorro);

                                // Actualizar el estado del préstamo a "Pagado"
                                prestamoAPagar.Estado = "Pagado";
                                db.Update(prestamoAPagar);

                                // Registrar el movimiento del pago
                                var movimientoPago = new MovimientosGrupoAhorroXCliente
                                {
                                    IdCliente = cliente.IdCliente,
                                    IdGrupoDestinatario = prestamoAPagar.IdGrupoAhorro,
                                    Tipo = "Pago de préstamo",
                                    Fecha = DateOnly.FromDateTime(DateTime.Now),
                                    Hora = DateTime.Now.ToString("HH:mm:ss"),
                                    Monto = montoTotalComoFloat
                                };
                                db.Add(movimientoPago);
                                db.SaveChanges();

                                AnsiConsole.MarkupLine($"Pago exitoso de ${montoTotal}. Nuevo saldo del cliente {cliente.Nombre}: ${cliente.Saldo}");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("Saldo insuficiente para realizar el pago.");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("No se realizó el pago del préstamo.");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("No se seleccionó un préstamo o el préstamo no existe para el cliente seleccionado.");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("No hay préstamos pendientes de pago para el cliente.");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("No se seleccionó un cliente o el cliente no existe.");
            }
        }

    }
}
