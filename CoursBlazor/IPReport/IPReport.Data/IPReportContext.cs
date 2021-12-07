using Microsoft.EntityFrameworkCore;

namespace IPReport.Data;

public class IPReportContext : DbContext
{
    #region Properties

    /// <summary>
    ///     Obtient ou définit le jeux de données des <see cref="IP"/>.
    /// </summary>
    public DbSet<IP> IPs { get; set; }

    /// <summary>
    ///     Obtient ou définit le jeux de données des <see cref="Report"/>.
    /// </summary>
    public DbSet<Report> Reports { get; set; }

    /// <summary>
    ///     Obtient ou définit le jeux de données des <see cref="Category"/>.
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="IPReportContext"/>.
    /// </summary>
    public IPReportContext()
    {
        IPs = IPs ?? throw new Exception($"Le jeux de données {nameof(IPs)} n'est pas initialisé.");
        Reports = Reports ?? throw new Exception($"Le jeux de données {nameof(Reports)} n'est pas initialisé.");
        Categories = Categories ?? throw new Exception($"Le jeux de données {nameof(Categories)} n'est pas initialisé.");
    }

    public IPReportContext(DbContextOptions<IPReportContext> options)
    : base(options)
    {
        IPs = IPs ?? throw new Exception($"Le jeux de données {nameof(IPs)} n'est pas initialisé.");
        Reports = Reports ?? throw new Exception($"Le jeux de données {nameof(Reports)} n'est pas initialisé.");
        Categories = Categories ?? throw new Exception($"Le jeux de données {nameof(Categories)} n'est pas initialisé.");
    }

    #endregion

    #region Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IP>(e =>
       {
           e.HasKey(e => e.Id);

           e.Ignore(e => e.IPSubnet);
       });

        modelBuilder.Entity<Report>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasOne(p => p.IP)
                .WithMany(d => d.Reports)
                .HasForeignKey(p => p.IPId);

            e.HasMany(p => p.Categories)
                .WithMany(d => d.Reports);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(e => e.Id);
        });
    }

    #endregion
}
