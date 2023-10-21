using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdeM_Bank_MyK
{
    internal class ClienteController
    {
        public static void AñadirCliente()
        {
            ClienteService.AñadirUsuario();
        }

        public static void EliminarCliente(Cliente cliente)
        {
            using var db = new UdemBankContext();
            db.Remove(cliente);
            db.SaveChanges();
        }

        public static Cliente GetClienteById(int Id)
        {
            using var db = new UdemBankContext();
            var cliente = db.Clientes.SingleOrDefault(b => b.IdCliente == Id);
            return cliente;
        }
        public static List<Cliente> GetClientes()
        {
            using var db = new UdemBankContext();
            var clientes = db.Clientes.ToList();
            return clientes;
        }
    }
}
