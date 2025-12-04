using System.ComponentModel.DataAnnotations;

namespace Zoologico.Modelos
{
    public class Animal
    {
       [Key]
        public int Id { get; set; }
        public string Nombres { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }

        // FK
        public int EspecieCodigo { get; set; }
        public int RazaId { get; set; }

        // Navegacion
        public Especie? Especie { get; set; }
        public Raza? Raza { get; set; }
    }
}
