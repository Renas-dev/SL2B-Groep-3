namespace Dierentuin_App.Models
{
    public class UpdatedStall
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Climate { get; set; } = string.Empty;
        public string HabitatType { get; set; } = string.Empty;
        public string SecurityLevel { get; set; } = string.Empty;
        public double Size { get; set; }
    }
}
