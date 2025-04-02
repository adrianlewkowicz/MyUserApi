using System;

namespace MyUserApi.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = default!;
        public string UserEmail { get; set; } = default!;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}
