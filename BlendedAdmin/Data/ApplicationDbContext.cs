using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlendedAdmin.DomainModel.Items;
using BlendedAdmin.DomainModel.Variables;

namespace BlendedAdmin.Data
{
    public class ApplicationDbContext : IdentityDbContext
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


            //ItemsModelSnapshot
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
            

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<BlendedAdmin.DomainModel.Environments.Environment> Environments { get; set; }
        public DbSet<Variable> Variables { get; set; }
        public DbSet<VariableEnvironment> VariablesEnvironments { get; set; }
    }
}
