using Microsoft.EntityFrameworkCore;
using QlySach_API.Model.Entity;
using System.Security.Cryptography;
using System.Text;

namespace QlySach_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Sach> ListSach { get; set; }
        public DbSet<DanhMuc> DanhMuc { get; set; }
        public DbSet<SachDanhMuc> SachDanhMucs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, nameRole = "Admin" },
                new Role { Id = 2, nameRole = "User" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, userName = "admin", Password = "admin", RoleId = 1 },
                new User { Id = 2, userName = "user", Password = "user", RoleId = 2 }
            );

            //danhmuc
            modelBuilder.Entity<DanhMuc>()
            .HasMany(dm => dm.listDanhMucCon)
            .WithOne(dm => dm.parentDanhMuc)
            .HasForeignKey(dm => dm.parentDanhMucId)
            .OnDelete(DeleteBehavior.Restrict);

            //Sach danh muc ><
            modelBuilder.Entity<SachDanhMuc>()
                .HasKey(sdm => new { sdm.sachId, sdm.danhMucId });

            modelBuilder.Entity<SachDanhMuc>()
                .HasOne(sdm => sdm.Sach)
                .WithMany(s => s.sachDanhMucs)
                .HasForeignKey(sdm => sdm.sachId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SachDanhMuc>()
                .HasOne(sdm => sdm.DanhMuc)
                .WithMany(dm => dm.SachDanhMucs)
                .HasForeignKey(sdm => sdm.danhMucId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

        }
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

    }
}
