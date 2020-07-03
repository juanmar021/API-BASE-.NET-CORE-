using DataContext.Model;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Context
{
    /// <summary>
    /// Context of de databae
    /// </summary>
    public class DBContext : DbContext
    {

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<General> Generals { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ElementTypeCategoryLeaflet>().HasKey(x => new
            //{
            //    x.ElementTypeId,
            //    x.ElementCategoryId
            //});
        }
    }
}
