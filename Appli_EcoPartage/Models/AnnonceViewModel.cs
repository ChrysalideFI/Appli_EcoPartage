namespace Appli_EcoPartage.Models
{
    public class AnnonceViewModel
    {
        public int IdAnnonce { get; set; }
        public required string Titre { get; set; }
        public required string Description { get; set; }
        public int Points { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public required string UserName { get; set; }
        public List<string> Tags { get; set; } = new();
        public List<string> GeographicalSectors { get; set; } = new();
    }
}
