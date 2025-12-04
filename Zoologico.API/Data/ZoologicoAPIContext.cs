using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zoologico.Modelos;

    public class ZoologicoAPIContext : DbContext
    {
        public ZoologicoAPIContext (DbContextOptions<ZoologicoAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Raza> Razas { get; set; } = default!;

        public DbSet<Especie> Especies { get; set; } = default!;

        public DbSet<Animal> Animales { get; set; } = default!;
}
