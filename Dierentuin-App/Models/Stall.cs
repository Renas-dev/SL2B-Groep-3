using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dierentuin_App.Models
{
    public class Stall
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Climate { get; set; } = string.Empty;
        public string HabitatType { get; set; } = string.Empty;
        public string SecurityLevel { get; set; } = string.Empty;
        public double Size { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }

        public Stall()
        {
            Animals = new HashSet<Animal>();
        }
    }

}
