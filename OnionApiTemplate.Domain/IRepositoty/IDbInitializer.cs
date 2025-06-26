using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiTemplate.Domain.IRepositoty
{
    public interface IDbInitializer
    {
        Task InitializeDatabaseAsync();
        Task InitializeIdentityAsync();
    }
}
