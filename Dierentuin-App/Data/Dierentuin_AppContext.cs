using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dierentuin_App.Models;

namespace Dierentuin_App.Data
{
    public class Dierentuin_AppContext : DbContext
    {
        public Dierentuin_AppContext (DbContextOptions<Dierentuin_AppContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animal { get; set; } = default!;
        public DbSet<Dierentuin_App.Models.UpdatedAnimal> UpdatedAnimal { get; set; } = default!;
    }
}
