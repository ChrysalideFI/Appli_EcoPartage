namespace Appli_EcoPartage.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int Points { get; set; }

        public List<UserCommentDisplayModel> Comments { get; set; } = new();
        public List<UserAnnonceDisplayModel> Annonces { get; set; } = new();
    }

    public class UserCommentDisplayModel
    {
        public int IdComment { get; set; }
        public required string GiverUserName { get; set; }
        public required string Notice { get; set; }
        public DateTime Date { get; set; }
    }

    public class UserAnnonceDisplayModel
    {
        public int IdAnnonce { get; set; }
        public required string Titre { get; set; }
        public required string Description { get; set; }
        public int Points { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }

}
