
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class DbContextDiego : DbContext
    {
        public DbContextDiego(DbContextOptions<DbContextDiego> options)
            : base(options)
        {
        }

        public DbSet<ConteoResponseDTO> TBL_Conteo { get; set; }
        public DbSet<RecaudoResponseDTO> TBL_Recaudo { get; set; }
    }
}