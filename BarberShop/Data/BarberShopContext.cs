using BarberShop.Entity;
using booking.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Data
{
    public class BarberShopContext : DbContext
    {
        public BarberShopContext(DbContextOptions<BarberShopContext> opt) : base(opt)
        { 

        }

        #region DbSet

        public DbSet<Store>? Stores { get; set; }
        public DbSet<Address>? Address { get; set; }
        public DbSet<Booking>? Bookings { get; set; }
        public DbSet<BookingStateDescription>? BookingStateDescription { get; set; }
        public DbSet<Category>? Category { get; set; }
        public DbSet<Customer>? Customer { get; set; }
        public DbSet<City>? City { get; set; }
        public DbSet<Country>? Country { get; set; }
        public DbSet<CustomerNotification>? CustomerNotification { get; set; }
        public DbSet<CustomerAddress>? CustomerAddress { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Evaluate>? Evaluate { get; set; }
        public DbSet<LocationStore>? LocationStore { get; set; }
        public DbSet<Notification>? Notifications { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Payment>? Payment { get; set; }
        public DbSet<Producer>? Producers { get; set; }
        public DbSet<Product>? Products { get; set;}
        public DbSet<ProductOrder>? ProductOrder { get; set; }
        public DbSet<Role>? Role { get; set; }
        public DbSet<Entity.Service>? Services { get; set; } // DbSet tên số nhiều
        public DbSet<ServiceCategory>? ServiceCategories { get; set; }
        public DbSet<ServiceManagement>? ServiceManagement { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Warehouse>? Warehouse { get; set; }
        public DbSet<WorkingHour>? WorkingHours { get; set; }
        public DbSet<RevenueStatistic> RevenueStatistics { get; set; }
        public DbSet<StoreRequest> StoreRequests { get; set; }


        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RevenueStatistic>(entity =>
            {
                entity.Property(e => e.TotalRevenue)
                      .HasPrecision(18, 2); // 18 chữ số tổng, 2 chữ số thập phân

            });
            modelBuilder.Entity<RevenueStatistic>()
       .ToTable("RevenueStatistic"); // Tên đúng trong SQL Server
        }

    }
}
