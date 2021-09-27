using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models;

namespace PruebaRegister.Data
{
    public class PruebaRegisterContext : DbContext
    {
        public PruebaRegisterContext (DbContextOptions<PruebaRegisterContext> options)
            : base(options)
        {
        }

        public DbSet<PruebaTecnica.Models.users> users { get; set; }
    }
}
