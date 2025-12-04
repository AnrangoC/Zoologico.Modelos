using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoologico.Modelos
{
    public class Raza
    {
        [Key] public int Id { get; set; }
        public string Nombre { get; set; }

        // Navegacion
        public List<Animal>? Animales { get; set; }
    }
}
