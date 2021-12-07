using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using IPReport.Data;
using IPReport.BlazorSrv.Services;

namespace IPReport.BlazorSrv.Pages;

public partial class SubnetPage
{
    #region Properties

    /// <summary>
    ///     Obtient ou définit le sous-réseau à tester.
    /// </summary>
    [Parameter]
    public string? SubnetToCheck { get; set; }

    /// <summary>
    ///     Obtient ou définit si le sous-réseau à tester est valide.
    /// </summary>
    private bool? IsSubnetValid { get; set; }

    /// <summary>
    ///     Obtient ou définit la liste des ips du sous-réseau.
    /// </summary>
    private List<IP>? IPs { get; set; }

    /// <summary>
    ///     Obtient ou définit le service de données des adresses IP.
    /// </summary>
    [Inject]
    private IpsDataService? IpsDataService { get; set; }

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

        IsSubnetValid = null;

        SubnetToCheck = SubnetToCheck?.Replace("_", ".") ?? string.Empty;
        IsSubnetValid = Regex.IsMatch(SubnetToCheck, @"^(?>\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(?>\.(?>25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}/24$");

        if (IsSubnetValid == true)
        {
            IPs = await IpsDataService.GetIpsInSubnetAsync(SubnetToCheck);
        }
    }

    #endregion
}
