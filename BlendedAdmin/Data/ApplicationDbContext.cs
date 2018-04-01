using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlendedAdmin.DomainModel.Items;
using BlendedAdmin.DomainModel.Variables;
using System;
using Microsoft.EntityFrameworkCore.Metadata;
using BlendedAdmin.DomainModel.Users;

namespace BlendedAdmin.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
            : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .HasAnnotation("ProductVersion", "1.0.0-rc3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Entity("BlendedAdmin.DomainModel.Users.ApplicationUser", b =>
            {
                b.Property<string>("Id");
                b.Property<int>("AccessFailedCount");
                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();
                b.Property<string>("Email")
                    .HasAnnotation("MaxLength", 256);
                b.Property<bool>("EmailConfirmed");
                b.Property<bool>("LockoutEnabled");
                b.Property<DateTimeOffset?>("LockoutEnd");
                b.Property<string>("NormalizedEmail")
                    .HasAnnotation("MaxLength", 256);
                b.Property<string>("NormalizedUserName")
                    .HasAnnotation("MaxLength", 256);
                b.Property<string>("PasswordHash");
                b.Property<string>("PhoneNumber");
                b.Property<bool>("PhoneNumberConfirmed");
                b.Property<string>("SecurityStamp");
                b.Property<bool>("TwoFactorEnabled");
                b.Property<string>("UserName")
                    .HasAnnotation("MaxLength", 256);
                b.Property<string>("TenantId")
                    .HasAnnotation("MaxLength", 256);
                b.HasKey("Id");
                b.ToTable("Users");
            });

            builder.Entity<Item>()
                .ToTable("Items").HasKey(x => x.Id);

            builder.Entity<BlendedAdmin.DomainModel.Environments.Environment>()
                .ToTable("Environments").HasKey(x => x.Id);

            builder.Entity<Variable>()
                .ToTable("Variables").HasKey(x => x.Id);
            builder.Entity<Variable>()
                .HasMany(x => x.Values).WithOne(x => x.Variable)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<VariableEnvironment>()
                .ToTable("VariablesEnvironments").HasKey(x => x.Id);
            builder.Entity<VariableEnvironment>()
                .HasOne(x => x.Variable);
            builder.Entity<VariableEnvironment>()
                .HasOne(x => x.Environment);

        }

        public DbSet<Item> Items { get; set; }
        public DbSet<BlendedAdmin.DomainModel.Environments.Environment> Environments { get; set; }
        public DbSet<Variable> Variables { get; set; }
        public DbSet<VariableEnvironment> VariablesEnvironments { get; set; }
    }
}
