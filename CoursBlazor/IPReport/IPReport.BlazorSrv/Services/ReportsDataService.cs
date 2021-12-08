using IPReport.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

    /// <summary>
    ///     Service de données pour les IP.
    /// </summary>
    private readonly IpsDataService _IpsDataService;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="ReportsDataService"/>.
    /// </summary>
    /// <param name="context">Contexte de données à utiliser.</param>
    /// <exception cref="ArgumentNullException">Exception déclenchée si un argument obligatoire n'est pas fourni.</exception>
    public ReportsDataService(IPReportContext context, IpsDataService ipsDataService)
    {
        _Context = context ?? throw new ArgumentNullException(nameof(context));
        _IpsDataService = ipsDataService ?? throw new ArgumentNullException(nameof(ipsDataService));
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
        .AsNoTracking() //AsNoTracking permet d'indiquer au contexte de données qu'il ne doit pas "suivre" les objets retournés (nécessaire pour la détection automatique des modifications lors du SaveChanges). Ceci a pour effet de ne pas mettre dans un cache local les objets chargés ainsi que de forcer le contexte à ne pas lire dans le cache pour l'exécution de cette requête.
        .Include(r => r.IP)
        .Include(r => r.Categories)
        .Where(r => ip == null || r.IPId == ip.Id)
        .OrderByDescending(r => r.DateTime)
        .Take(count)
        .ToListAsync();

    /// <summary>
    ///     Ajoute des rapports de manière asynchrone.
    /// </summary>
    /// <param name="reports">Rapports à ajouter.</param>
    /// <returns>Tâche pouvant être attendue.</returns>
    /// <exception cref="Exception">Peut être levée si le rapport n'a pas d'adresse IP valide associée.</exception>
    public async Task AddReportsAsync(params Report[] reports)
    {
        foreach (Report report in reports)
        {
            if (report.IP == null)
            {
                throw new Exception("Impossible d'ajouter un rapport sans adresse IP.");
            }

            IP ip = await _IpsDataService.GetIpAsync(report.IP.Id) ?? await _IpsDataService.GetIpAsync(report.IP.IPAddress) ?? report.IP;

            if (Regex.IsMatch(ip.IPAddress, @"^(?>\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(?>\.(?>25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$") == false)
            {
                throw new Exception("Le format de l'adresse IP n'est pas valide.");
            }

            report.IP = ip;

            _Context.Reports.Add(report);
        }

        _Context.SaveChanges();
    }

    #endregion
}
