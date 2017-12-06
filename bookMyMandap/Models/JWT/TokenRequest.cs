using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookMyMandap.Models.JWT
{
    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
