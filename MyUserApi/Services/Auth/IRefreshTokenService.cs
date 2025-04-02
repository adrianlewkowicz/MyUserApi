namespace MyUserApi.Services
{
    public interface IRefreshTokenService
    {
        string GenerateToken(string email);
    }
}
