using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdeM_Bank_MyK
{
    internal class GrupoAhorroController
    {
        public static void AñadirGrupoDeAhorro()
        {
            GrupoAhorroService.AñadirGrupoDeAhorro();
        }

        public static void EliminarGrupoDeAhorro()
        {
            GrupoAhorroService.DisolverGrupoAhorro();
        }

        public static GrupoAhorro GetGrupoAhorroById(int Id)
        {
            using var db = new UdemBankContext();
            var grupo_ahorro = db.GruposAhorros.SingleOrDefault(b => b.IdGrupoAhorro == Id);
            return grupo_ahorro;
        }
        public static List<GrupoAhorro> GetGrupoAhorros()
        {
            using var db = new UdemBankContext();
            var grupo_ahorros = db.GruposAhorros.ToList();
            return grupo_ahorros;
        }
    }
}
