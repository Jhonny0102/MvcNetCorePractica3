using Microsoft.EntityFrameworkCore;
using MvcNetCorePractica3.Models;

namespace MvcNetCorePractica3.Data
{
    public class ZapatosContext : DbContext
    {
        public ZapatosContext(DbContextOptions<ZapatosContext> options)
            :base(options)
        {

        }

        public DbSet<Zapato> Zapatos { get; set; }
        public DbSet<ImagenZapatos> ImagenZapatos { get; set; }
    }
}
