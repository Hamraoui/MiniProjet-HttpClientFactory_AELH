using Microsoft.EntityFrameworkCore;
using myApiV1._0.Models;

namespace myApiV1._0.Data
{
    public class UserDbContext : DbContext

    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
        {
        }

        public DbSet<user> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("myApiV1._0.Models.user", b =>
            {
                b.Property<int>("id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                b.Property<string>("adresse")
                    .IsRequired(false)
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("createdAt")
                    .HasColumnType("datetime2");

                b.Property<string>("email")
                    .IsRequired(false)
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("fullName")
                    .IsRequired(false)
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("password")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");


                b.Property<string>("tel")
                    .IsRequired(false)
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime?>("updatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("username")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("id");

                b.ToTable("Users");
            });
        }
    }
}
