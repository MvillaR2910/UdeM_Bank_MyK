using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdeM_Bank_MyK
{
    public class GrupoAhorro
    {
        [Key]
        public int IdGrupoAhorro { get; set; }
        public string NombreGrupo { get; set; }
        public float Capital { get; set; }
        public List<Cliente> Usuarios { get; set; } 
    }
}
