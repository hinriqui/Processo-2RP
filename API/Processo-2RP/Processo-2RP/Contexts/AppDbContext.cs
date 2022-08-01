using Processo_2RP.Domains;
using Microsoft.EntityFrameworkCore;

namespace Processo_2RP.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Tipo> Tipos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
    }
}
