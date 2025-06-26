using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiTemplate.Domain.Exceptions
{
    public class UserAlreadyExistsException(string message)
        : BadRequestException(new List<string> { message });
}
