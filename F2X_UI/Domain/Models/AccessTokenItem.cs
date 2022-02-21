using System;

namespace Domain.Models
{
    public class AccessTokenItem
    {
        public string token { get; set; } = string.Empty;
        public DateTime expiration { get; set; }
    }
}
