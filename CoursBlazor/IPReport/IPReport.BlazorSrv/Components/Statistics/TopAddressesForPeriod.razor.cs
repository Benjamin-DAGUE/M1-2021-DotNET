using IPReport.BlazorSrv.Services;
using IPReport.Data;
using Microsoft.AspNetCore.Components;

namespace IPReport.BlazorSrv.Components.Statistics;

public partial class TopAddressesForPeriod : StatisticsForPeriodBase
{
    #region Properties

    private List<IP> Addresses { get; set; } = new List<IP>();

    [Inject]
    private IpsDataService? IpsDataService { get; set; }

    #endregion

    #region Methods

    protected override async Task OnParametersSetAsync()
    {
        if (IpsDataService == null)
        {
            throw new Exception($"Le service {nameof(IpsDataService)} n'est pas initialisé.");
        }

        Addresses = await IpsDataService.GetTopReportedIpsOverPeriod(PeriodDurationInHours);
    }

    #endregion
}
