using IPReport.BlazorSrv.Services;
using IPReport.Data;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace IPReport.BlazorSrv.Pages;

public partial class IPPage
{
    #region Properties

    /// <summary>
    ///     Obtient ou définit le service de données pour les adresses ip.
    /// </summary>
    [Inject]
    private IpsDataService? IpsDataService { get; set; }

    /// <summary>
    ///     Obtient ou définit l'IP à tester.
    /// </summary>
    [Parameter]
    public string? IPToCheck { get; set; }

    /// <summary>
    ///     Obtient ou définit si l'IP à tester est valide.
    /// </summary>
    private bool? IsIPValid { get; set; }

    /// <summary>
    ///     Obtient ou définit l'IP.
    /// </summary>
    private IP? Ip { get; set; }

    /// <summary>
    ///     Obtient ou définit le nombre d'éléments.
    /// </summary>
    private int ItemsCount { get; set; }

    #endregion

    #region Methods

    /// <summary>
    ///     Méthode déclenchée en asynchrone lorsqu'un paramètre change.
    /// </summary>
    /// <returns>Tâche pouvant être attendue.</returns>
    protected override async Task OnParametersSetAsync()
    {
        if (IpsDataService == null)
        {
            throw new Exception($"Le service {nameof(IpsDataService)} n'est pas initialisé.");
        }

        IsIPValid = null;

        IPToCheck = IPToCheck?.Replace("_", ".") ?? string.Empty;
        IsIPValid = Regex.IsMatch(IPToCheck, @"^(?>\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(?>\.(?>25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$");

        if (IsIPValid == true)
        {
            Ip = await IpsDataService.GetIpAsync(IPToCheck);
        }
    }

    #endregion
}
