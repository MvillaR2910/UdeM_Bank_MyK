using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdeM_Bank_MyK
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public float Saldo { get; set; }
        public int NroGruposPertenecientes { get; set; }
        public List<GrupoAhorro> GruposDeAhorros { get; set; }
        public bool ComisionReducida { get; set; }
    }
}
