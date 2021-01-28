using Microsoft.EntityFrameworkCore;
using Roomby.API.Models;
using Roomby.API.Rooms.Data.EntityConfigurations;

namespace Roomby.API.Rooms.Data {
    public class RoomsContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }

        public RoomsContext(DbContextOptions<RoomsContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyConfigurations(modelBuilder);
            Seed(modelBuilder);
        }

        private void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }

        private void Seed(ModelBuilder modelBuilder)
        {

        }
    }
}