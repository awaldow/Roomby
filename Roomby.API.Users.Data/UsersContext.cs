using Microsoft.EntityFrameworkCore;
using Roomby.API.Models;
using Roomby.API.Users.Data.EntityConfigurations;

namespace Roomby.API.Users.Data {
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyConfigurations(modelBuilder);
            Seed(modelBuilder);
        }

        private void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        private void Seed(ModelBuilder modelBuilder)
        {

        }
    }
}