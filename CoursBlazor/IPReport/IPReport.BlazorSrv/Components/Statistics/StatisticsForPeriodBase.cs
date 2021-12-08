using Microsoft.AspNetCore.Components;

namespace IPReport.BlazorSrv.Components.Statistics;

public abstract class StatisticsForPeriodBase : ComponentBase
{
    #region Properties

    [Parameter]
    public double PeriodDurationInHours { get; set; }

    [Parameter]
    public string? PeriodString { get; set; }

    #endregion



}
