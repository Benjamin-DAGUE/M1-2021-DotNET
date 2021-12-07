using Microsoft.EntityFrameworkCore;

namespace FirstBlazorApp.Data;

public class DataContext : DbContext
{
    #region Properties

    public DbSet<IpAddress> IpAddresses { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Category> Categories { get; set; }

    #endregion

    #region Constructors

    public DataContext()
    {
        IpAddresses = IpAddresses ?? throw new Exception($"Le jeux de données {nameof(IpAddresses)} n'est pas initialisé.");
        Reports = Reports ?? throw new Exception($"Le jeux de données {nameof(Reports)} n'est pas initialisé.");
        Categories = Categories ?? throw new Exception($"Le jeux de données {nameof(Categories)} n'est pas initialisé.");
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        IpAddresses = IpAddresses ?? throw new Exception($"Le jeux de données {nameof(IpAddresses)} n'est pas initialisé.");
        Reports = Reports ?? throw new Exception($"Le jeux de données {nameof(Reports)} n'est pas initialisé.");
        Categories = Categories ?? throw new Exception($"Le jeux de données {nameof(Categories)} n'est pas initialisé.");
    }

    #endregion

    #region Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IpAddress>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Report>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasOne(p => p.IpAddress)
            .WithMany(d => d.Reports)
            .HasForeignKey(p => p.IdIpAddress);

            e.HasMany(p => p.Categories)
            .WithMany(d => d.Reports);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(e => e.Id);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(optionsBuilder.IsConfigured == false)
        {
            optionsBuilder.UseSqlite(@"Data Source=.\bin\ipreport.db;");
        }
    }

    #endregion
}
