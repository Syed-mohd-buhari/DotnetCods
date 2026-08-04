using CAM.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Models
{
    public class AuthResult
    {
        public string Token { get; set; }
        public RefreshTokenModel RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
