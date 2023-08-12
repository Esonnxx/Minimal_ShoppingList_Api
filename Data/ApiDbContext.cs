

using Microsoft.EntityFrameworkCore;

namespace MinimalShoppingListApi.Data
{
    public class ApiDbContext :DbContext
    {
        public DbSet<Grocery> Groceries => Set<Grocery>();
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { 
        
        }
    }
}
