using IPReport.BlazorSrv.Services;
using Microsoft.AspNetCore.Components;

namespace IPReport.BlazorSrv.Components.Statistics;

public partial class CounterForPeriod
{
    #region Properties

    private int NumberOfAddresses { get; set; }

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

        NumberOfAddresses = await IpsDataService.CountIpWithReportOverPeriod(PeriodDurationInHours);
    }

    #endregion

}
