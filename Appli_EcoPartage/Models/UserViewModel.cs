namespace Appli_EcoPartage.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int Points { get; set; }

        public List<UserCommentDisplayModel> Comments { get; set; } = new();
    }

    public class UserCommentDisplayModel
    {
        public int IdComment { get; set; }
        public required string GiverUserName { get; set; }
        public required string Notice { get; set; }
        public DateTime Date { get; set; }
    }
}
