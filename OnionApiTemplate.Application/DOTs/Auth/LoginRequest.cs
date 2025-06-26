using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiTemplate.Application.DOTs.Auth
{
    public class LoginRequest
    {
        public  string Email { get; set; } = null!;
        public  string Password { get; set; } = null!;
    }
}
