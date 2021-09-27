using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Models
{
    public interface Iusers<T>
    {
        T token { get; set; }
        string name { get; set; }
        string email { get; set; }
        string password { get; set; }
        DateTime? last_login { get; set; }
    }
}
