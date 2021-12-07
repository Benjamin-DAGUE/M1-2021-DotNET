using IPReport.BlazorSrv.Services;
using IPReport.Data;
using Microsoft.AspNetCore.Components;

namespace IPReport.BlazorSrv.Components;

/// <summary>
///     Le composant <see cref="LastReportsList"/> permet d'afficher une liste des derniers rapports.
///     La liste peut être contrainte à une adresse IP spécifique ou non.
/// </summary>
public partial class LastReportsList
{
    #region Properties

    /// <summary>
    ///     Obtient ou définit le service de données pour les rapports d'abus.
    /// </summary>
    [Inject]
    private ReportsDataService? ReportsDataService { get; set; }

    /// <summary>
    ///     Obtient ou définit l'adresse IP pour laquelle il faut récupérer les rapports.
    ///     Si la valeur est null, le composant charge les derniers rapports pour l'ensemble des adresses.
    /// </summary>
    [Parameter]
    public IP? Ip { get; set; }

    /// <summary>
    ///     Détermine si le tableau a un header fixe avec une hauteur fixe (scroll bar) ou non.
    /// </summary>
    [Parameter]
    public bool FixedHeader { get; set; } = true;

    /// <summary>
    ///     Méthode déclenchée par le composant lorsque le nombre de rapports change.
    ///     Permet de remonter au composant parent le nombre de rapports chargés.
    /// </summary>
    [Parameter]
    public EventCallback<int> ItemsCountChanged { get; set; }

    /// <summary>
    ///     Obtient ou définit la liste des rapports chargés.
    /// </summary>
    private List<Report>? Reports { get; set; }

    #endregion

    #region Methods

    /// <summary>
    ///     Méthode d'initialise asynchrone du composant.
    /// </summary>
    /// <returns>Tâche pouvant être attendue.</returns>
    protected override async Task OnInitializedAsync()
    {
        if (ReportsDataService == null)
        {
            throw new Exception($"Le service {nameof(ReportsDataService)} n'est pas initialisé.");
        }

        //Chargement des données.
        Reports = (await ReportsDataService.GetLastReportsAsync(Ip)).ToList();

        //Indique au composant parent le nombre d'éléments chargés.
        await ItemsCountChanged.InvokeAsync(Reports.Count);
    }

    #endregion
}
