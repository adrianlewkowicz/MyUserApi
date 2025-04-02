using MyUserApi.Data;
using MyUserApi.Models;
using System;

namespace MyUserApi.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly MyDbContext _context;

        public RefreshTokenService(MyDbContext context)
        {
            _context = context;
        }

        public string GenerateToken(string email)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserEmail = email,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            return refreshToken.Token;
        }

        public bool ValidateToken(string token)
        {
            var storedToken = _context.RefreshTokens
                .FirstOrDefault(t => t.Token == token && !t.IsRevoked && t.ExpiryDate > DateTime.UtcNow);

            return storedToken != null;
        }

        public void RevokeToken(string token)
        {
            var storedToken = _context.RefreshTokens.FirstOrDefault(t => t.Token == token);
            if (storedToken != null)
            {
                storedToken.IsRevoked = true;
                _context.SaveChanges();
            }
        }
    }
}
