using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdeM_Bank_MyK
{
    public class GrupoAhorroXCliente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdGrupoAhorro { get; set; }
        [ForeignKey(nameof(IdGrupoAhorro))]

        public GrupoAhorro GrupoAhorro { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [ForeignKey(nameof(IdCliente))]

        public Cliente Cliente { get; set; }
        public float AporteCliente { get; set; }
    }
}
