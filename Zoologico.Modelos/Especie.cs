using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoologico.Modelos
{
    public class Especie
    {
        [Key] public int Codigo { get; set; }
        public string NombreComun { get; set; }

        // Navegacion
        public List<Animal>? Animales { get; set; }
    }
}
