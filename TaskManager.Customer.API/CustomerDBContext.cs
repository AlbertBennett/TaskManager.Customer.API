using Microsoft.EntityFrameworkCore;

namespace TaskManager.Customer.API
{
    public sealed class CustomerDBContext : DbContext
    {
        public DbSet<Customer.API.Models.Customer> Customer { get; private set; }

        public CustomerDBContext(DbContextOptions<CustomerDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapCountryCodes(modelBuilder);
            MapCustomer(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void MapCountryCodes(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Customer.API.Models.Country>().ToTable("CountryCodes");

            modelBuilder.Entity<Customer.API.Models.Country>()
                .Property(b => b.ID)
                .HasColumnName("ID")
                .IsRequired();

            modelBuilder.Entity<Customer.API.Models.Country>()
                .Property(b => b.ID)
                .HasColumnName("ISOCode")
                .HasMaxLength(4)
                .IsRequired();

            modelBuilder.Entity<Customer.API.Models.Country>()
                .Property(b => b.Name)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();
        }

        private void MapCustomer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer.API.Models.Customer>().ToTable("Customer");

            modelBuilder.Entity<Customer.API.Models.Customer>()
                .Property(b => b.ID)
                .HasColumnName("ID")
                .IsRequired();

            modelBuilder.Entity<Customer.API.Models.Customer>()
                .Property(b => b.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Customer.API.Models.Customer>()
                .Property(b => b.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Customer.API.Models.Customer>()
                .Property(b => b.Email)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Customer.API.Models.Customer>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryCodeId);
        }
    }
}
