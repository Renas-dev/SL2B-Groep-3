using System.ComponentModel.DataAnnotations;

namespace Dierentuin_App.Models
{
    public class UpdatedAnimal
    {
        [Key] // Primary Key
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
        public string Dietary { get; set; }
        public string ActivityPattern { get; set; }
        public string Prey { get; set; }
        public string Enclosure { get; set; }
        public string SpaceRequirement { get; set; }
        public string SecurityRequirement { get; set; }

    }
}
