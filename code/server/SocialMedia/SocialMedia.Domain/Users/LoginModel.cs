using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Domain.Users
{
    internal class LoginModel
    {
        public string Tag { get; set; } = string.Empty;
        public string Password { get; set; }

    }
}
