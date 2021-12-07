using IPReport.Data;
using Microsoft.EntityFrameworkCore;

namespace IPReport.BlazorSrv.Services;

/// <summary>
///     Service d'accès aux données pour les rapports.
/// </summary>
public class ReportsDataService
{
    #region Fields

    /// <summary>
    ///     Contexte de données à utiliser.
    /// </summary>
    private readonly IPReportContext _Context;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="ReportsDataService"/>.
    /// </summary>
    /// <param name="context">Contexte de données à utiliser.</param>
    /// <exception cref="ArgumentNullException">Exception déclenchée si un argument obligatoire n'est pas fourni.</exception>
    public ReportsDataService(IPReportContext context)
    {
        _Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Obtient les derniers rapports d'abus.
    /// </summary>
    /// <param name="count">Nombre de rapport à récupérer, 20 par défaut.</param>
    /// <param name="ip">IP pour laquelle il faut récupérer les derniers rapports.</param>
    /// <returns>Retourne les derniers rapports d'abus.</returns>
    public async Task<List<Report>> GetLastReportsAsync(IP? ip = null, int count = 20) => await _Context
        .Reports
        .Include(r => r.IP)
        .Include(r => r.Categories)
        .Where(r => ip == null || r.IPId == ip.Id)
        .OrderByDescending(r => r.DateTime)
        .Take(count)
        .ToListAsync();

    #endregion
}
