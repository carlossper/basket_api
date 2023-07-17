using Microsoft.EntityFrameworkCore;

namespace BasketApi.Models.Contexts.Implementations
{
    public class BasketContext : DbContext
    {
        public BasketContext()
        {
            this.Database.EnsureCreated();
        }

        public DbSet<BasketModel> Baskets { get; set; }
    }
}
