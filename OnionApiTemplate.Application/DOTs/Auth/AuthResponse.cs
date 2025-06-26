using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiTemplate.Application.DOTs.Auth
{
    public record AuthResponse(string Email, string UserName, string Token);
}
