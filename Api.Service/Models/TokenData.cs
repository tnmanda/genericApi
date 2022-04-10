using System;

namespace Api.Service.Models
{
    public class TokenData
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
