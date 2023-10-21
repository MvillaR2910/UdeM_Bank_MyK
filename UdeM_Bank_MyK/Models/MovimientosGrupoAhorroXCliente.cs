using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdeM_Bank_MyK
{
    public class MovimientosGrupoAhorroXCliente
    {
        [Key]
        public int IdMovimiento { get; set; }

        [Required]
        public int IdCliente { get; set; }
        [ForeignKey(nameof(IdCliente))]

        public Cliente Cliente { get; set; }
        public int IdGrupoDestinatario { get; set; }
        public string Tipo { get; set; }
        public DateOnly Fecha { get; set; }
        public string Hora { get; set; }
        public float Monto { get; set; }
    }
}
