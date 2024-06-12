namespace Dierentuin_App.Models
{
    public class UpdatedStall
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Climate { get; set; }
        public string HabitatType { get; set; }
        public string SecurityLevel { get; set; }
        public double Size { get; set; }

        }
}
