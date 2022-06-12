using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Api.Models
{
    public class ApiContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Frequent> Frequents { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Directive> Directives { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GeneralAnalytic> GeneralAnalytics { get; set; }
        public DbSet<CurrentDataAnalytic> CurrentDataAnalytics { get; set; }
        public DbSet<DirectiveAnalytic> DirectiveAnalytics { get; set; }
        public DbSet<DirectiveCount> DirectiveCounts { get; set; }
        public DbSet<LocationAnalytic> LocationAnalytics { get; set; }
        public DbSet<LocationCount> LocationCounts { get; set; }
        public DbSet<CategoryAnalytic> CategoryAnalytics { get; set; }
        public DbSet<CategoryCount> CategoryCounts { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}