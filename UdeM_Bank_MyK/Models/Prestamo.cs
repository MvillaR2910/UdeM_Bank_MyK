using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UdeM_Bank_MyK
{
    public class Prestamo
    {
        [Key]
        public int IdPrestamo { get; set; }

        [Required]
        public int IdCliente { get; set; }
        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }

        [Required]
        public int IdGrupoAhorro { get; set; }
        [ForeignKey(nameof(IdGrupoAhorro))]
        public GrupoAhorro GrupoAhorro { get; set; }
        public string Interes { get; set; }
        public int MesesAPagar { get; set; }
        public float Cantidad { get; set; }
        public string Estado { get; set; }
    }
}
