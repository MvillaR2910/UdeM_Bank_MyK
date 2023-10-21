using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdeM_Bank_MyK
{
    internal class EquipoFidelizacion
    {
        static internal void PremiarGrupoMasRentable()
        {
            using var db = new UdemBankContext();

            // Encontrar el grupo de ahorro con el mayor capital
            var grupoMasRentable = db.GruposAhorros
                .OrderByDescending(g => g.Capital)
                .FirstOrDefault();

            if (grupoMasRentable != null)
            {
                // Calcular el 10% del saldo actual del grupo
                var saldoActual = grupoMasRentable.Capital;
                var inyeccion = saldoActual * 0.10;

                // Actualizar el saldo del grupo con la inyección
                grupoMasRentable.Capital += (float)inyeccion;
                db.Update(grupoMasRentable);
                db.SaveChanges();

                AnsiConsole.MarkupLine($"El grupo de ahorro '{grupoMasRentable.NombreGrupo}' ha sido premiado con una inyección de ${inyeccion}.");
            }
            else
            {
                AnsiConsole.MarkupLine("No hay grupos de ahorro disponibles para premiar.");
            }
        }

        static internal void PremiarUsuarioConMasAporte()
        {
            using var db = new UdemBankContext();
            var ganadores = new List<Tuple<string, string>>(); // Para almacenar los ganadores (cliente, grupo)

            // Obtener todos los grupos de ahorro
            var gruposAhorro = db.GruposAhorros.ToList();

            foreach (var grupoAhorro in gruposAhorro)
            {
                // Obtener información de usuarios y sus aportes en este grupo
                var usuariosEnGrupo = db.GrupoAhorroXCliente
                    .Where(x => x.IdGrupoAhorro == grupoAhorro.IdGrupoAhorro)
                    .ToList();

                // Encontrar al usuario con el aporte más alto
                var usuarioDestacado = usuariosEnGrupo.OrderByDescending(x => x.AporteCliente).FirstOrDefault();

                if (usuarioDestacado != null)
                {
                    // Actualizar el atributo ComisionReducida en la tabla Clientes
                    var cliente = db.Clientes.Find(usuarioDestacado.IdCliente);
                    if (cliente != null)
                    {
                        cliente.ComisionReducida = true;
                        db.Update(cliente);
                        ganadores.Add(new Tuple<string, string>(cliente.Nombre, grupoAhorro.NombreGrupo));
                    }
                }
            }

            db.SaveChanges();

            if (ganadores.Any())
            {
                Console.WriteLine("Ganadores del programa de fidelización:");
                foreach (var ganador in ganadores)
                {
                    Console.WriteLine($"Cliente: {ganador.Item1}, Grupo: {ganador.Item2}");
                }
            }
            else
            {
                Console.WriteLine("No hay ganadores en el programa de fidelización.");
            }
        }

    }
}
