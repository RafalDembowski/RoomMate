using System.Data.Entity;
using RoomMate.Entities.Users;
using RoomMate.Entities.Rooms;
using RoomMate.Entities.Bookings;

namespace RoomMate.Data.Context
{
    public class RoomMateDbContext : DbContext
    {
        public RoomMateDbContext() : base("Name=RoomMateDbConString")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserImage> UserImages {get; set;}
        public DbSet<Room> Room { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().Property(Addresses => Addresses.Lat).HasPrecision(9, 6);
            modelBuilder.Entity<Address>().Property(Addresses => Addresses.Lon).HasPrecision(8, 6);
            base.OnModelCreating(modelBuilder);
        }

    }
}