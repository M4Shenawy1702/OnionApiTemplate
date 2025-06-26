using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiTemplate.Domain.Exceptions
{
    public class NotFoundException(string id)
        : Exception($"{id} not found");
}
