namespace MyUserApi.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Email { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string Password { get; set; } = null!;
    }
}
