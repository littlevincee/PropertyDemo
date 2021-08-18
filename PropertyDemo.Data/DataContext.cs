using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using PropertyDemo.Service;

namespace PropertyDemo.Data
{
    public interface IDataContext
    {
        DbSet<Model.Property> Properties { get; }
        
        DbSet<Model.Transaction> Transactions { get; }

        DbSet<Model.OwnerDetail> OwnerDetails { get; }

        DbSet<Model.ApplicationUser> ApplicationUsers { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
        int SaveChanges();
    }

    public class DataContext : IdentityDbContext<Model.ApplicationUser>, IDataContext
    {
        public DbSet<Model.Property> Properties { get; set; }
        
        public DbSet<Model.Transaction> Transactions { get; set; }

        public DbSet<Model.OwnerDetail> OwnerDetails { get; set; }

        public DbSet<Model.ApplicationUser> ApplicationUsers { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}
