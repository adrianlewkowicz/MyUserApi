
public class RegisterRequest
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string? Role { get; set; }
    public string Password { get; set; } = null!;
}
