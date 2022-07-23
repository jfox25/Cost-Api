using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Api.Models
{
    public class ApiContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Frequent> Frequents { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Directive> Directives { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GeneralAnalytic> GeneralAnalytics { get; set; }
        public DbSet<CurrentDataAnalytic> CurrentDataAnalytics { get; set; }
        public DbSet<LookupType> LookupTypes { get; set; }
        public DbSet<LookupCount> LookupCounts { get; set; }
        public DbSet<LookupAnalytic> LookupAnalytics { get; set; }
       

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}