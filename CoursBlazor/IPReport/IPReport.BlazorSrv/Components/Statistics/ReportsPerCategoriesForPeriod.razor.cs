using Blazorise.Charts;
using IPReport.BlazorSrv.Services;
using IPReport.Data;
using Microsoft.AspNetCore.Components;

namespace IPReport.BlazorSrv.Components.Statistics;

public partial class ReportsPerCategoriesForPeriod
{
    #region Fields

    /// <summary>
    ///     Composant du graphique.
    /// </summary>
    private PolarAreaChart<int>? _Chart;

    #endregion

    #region Properties

    [Inject]
    private CategoriesDataService? CategoriesDataService { get; set; }

    #endregion

    #region Methods

    protected override async Task OnParametersSetAsync() => await RefreshChart();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RefreshChart();
        }
    }

    private async Task RefreshChart()
    {
        if (_Chart != null)
        {
            if (CategoriesDataService == null)
            {
                throw new Exception($"Le service {nameof(CategoriesDataService)} n'est pas initialisé.");
            }

            await _Chart.Clear();

            List<string> categories = (await CategoriesDataService.GetCategories()).Select(c => c.Name).ToList();
            List<int> counts = await CategoriesDataService.CountReportsForCategoriesOverPeriod(PeriodDurationInHours);

            PolarAreaChartDataset<int> dataSet = new PolarAreaChartDataset<int>()
            {
                Label = "Nombre de rapports",
                Data = counts,
                BackgroundColor = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) },
                BorderColor = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) },
            };

            await _Chart.AddLabelsDatasetsAndUpdate(categories, dataSet);
        }
    }

    #endregion
}
