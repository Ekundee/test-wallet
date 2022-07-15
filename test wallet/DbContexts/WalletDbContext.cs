using Microsoft.EntityFrameworkCore;
using test_wallet.Model;
namespace test_wallet.DbContexts
{
    public class WalletDbContext : DbContext
    {

        private IConfiguration _configuration;
        public DbSet<WalletModel> DbWallets { get; set; }

        public WalletDbContext(DbContextOptions<WalletDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }



        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  
            base.OnModelCreating(modelBuilder);

            // Map entities to tables  
            modelBuilder.Entity<WalletModel>().ToTable("users");
            // modelBuilder.Entity<WalletModel>();
        }
    }
}
