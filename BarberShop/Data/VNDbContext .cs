using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BarberShop.Data
{
    public class VNDbContext : DbContext
    {
        public VNDbContext(DbContextOptions<VNDbContext> options) : base(options) { }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<Ward> Wards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Province>()
                .HasKey(p => p.Code);

            modelBuilder.Entity<Ward>()
                .HasKey(w => w.Code);

            modelBuilder.Entity<Ward>()
                .HasOne(w => w.Province)
                .WithMany(p => p.Wards)
                .HasForeignKey(w => w.ProvinceCode);
        }
    }
        
    public class Province
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public string NameEn { get; set; }

        public string FullName { get; set; }

        public string FullNameEn { get; set; }

        public string CodeName { get; set; }

        public int? AdministrativeUnitId { get; set; }

        public ICollection<Ward> Wards { get; set; }
    }

    public class Ward
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public string NameEn { get; set; }

        public string FullName { get; set; }

        public string FullNameEn { get; set; }

        public string CodeName { get; set; }

        [Column("province_code")]  // ánh xạ chính xác với cột DB
        public string ProvinceCode { get; set; }

        public Province Province { get; set; }
    }

}
