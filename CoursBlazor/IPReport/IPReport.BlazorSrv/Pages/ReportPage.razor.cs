using Blazorise;
using IPReport.BlazorSrv.Services;
using IPReport.Data;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace IPReport.BlazorSrv.Pages;

public partial class ReportPage
{
    #region Properties

    /// <summary>
    ///     Obtient ou définit le service de données des catégories.
    /// </summary>
    [Inject]
    private CategoriesDataService? CategoriesDataService { get; set; }

    /// <summary>
    ///     Obtient ou définit le service de données des rapports.
    /// </summary>
    [Inject]
    private ReportsDataService? ReportsDataService { get; set; }

    /// <summary>
    ///     Obtient ou définit le service de notification.
    /// </summary>
    [Inject]
    private INotificationService? NotificationService { get; set; }

    /// <summary>
    ///     Obtient ou définit l'adresses ou les adresses du rapport d'abus.
    /// </summary>
    private string IPAddresses { get; set; } = string.Empty;

    /// <summary>
    ///     Obtient ou définit la date de rapport d'abus.
    /// </summary>
    private DateTime ReportDateTime { get; set; } = DateTime.Now;

    /// <summary>
    ///     Obtient ou définit les catégories disponibles.
    /// </summary>
    private List<Category>? Categories { get; set; }
    
    /// <summary>
    ///     Obtient ou définit les catégories sélectionnées.
    /// </summary>
    private List<Category> SelectedCategories { get; set; } = new List<Category>();

    /// <summary>
    ///     Obtient ou définit les noms des catégories sélectionnées.
    /// </summary>
    private List<string> SelectedNameCategories { get; set; } = new List<string>();

    /// <summary>
    ///     Obtient ou définit le commentaire du rapport d'abus.
    /// </summary>
    private string? ReportComment { get; set; }

    /// <summary>
    ///     Obtient ou définit si le formulaire est valide.
    /// </summary>
    private bool IsFormValid { get; set; } = false;

    #endregion

    #region Methods

    /// <summary>
    ///     Méthode déclenchée à la fin de l'initialisation de la page.
    /// </summary>
    /// <returns>Tâche pouvant être attendue.</returns>
    /// <exception cref="Exception">Peut être déclenchée si le service <see cref="CategoriesDataService"/> n'est pas initialisé.</exception>
    protected override async Task OnInitializedAsync()
    {
        if (CategoriesDataService == null)
        {
            throw new Exception($"Le service {nameof(CategoriesDataService)} n'est pas initialisé.");
        }

        Categories = await CategoriesDataService.GetCategories();
    }

    /// <summary>
    ///     Met à jour l'état de validation du formulaire.
    /// </summary>
    private void UpdateIsFormValid() => IsFormValid = IPAddresses
        .Split(';', StringSplitOptions.RemoveEmptyEntries)
        .Select(ip => ip.Trim())
        .All(ip => Regex.IsMatch(ip, @"^(?>\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(?>\.(?>25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$"));

    /// <summary>
    ///     Méthode déclenchée lors du clic sur le bouton de validation du formulaire.
    /// </summary>
    /// <returns>Tâche pouvant être attendue.</returns>
    /// <exception cref="Exception">Peut être déclenchée si les services <see cref="ReportsDataService"/> et <see cref="NotificationService"/> ne sont pas initialisés.</exception>
    private async Task ValidateFormButtonClickedAsync()
    {
        if (ReportsDataService == null)
        {
            throw new Exception($"Le service {nameof(ReportsDataService)} n'est pas initialisé.");
        }
        if (NotificationService == null)
        {
            throw new Exception($"Le service {nameof(NotificationService)} n'est pas initialisé.");
        }

        //On vérifie que le formulaire est bien valide au préalable.
        UpdateIsFormValid();

        if (IsFormValid)
        {
            try
            {
                IsFormValid = false;

                Report[] reports = IPAddresses
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(ip => new Report()
                    {
                        IP = new IP()
                        {
                            IPAddress = ip.Trim(),
                        },
                        DateTime = ReportDateTime,
                        Comment = ReportComment,
                        Categories = SelectedCategories
                    })
                    .ToArray();

                await ReportsDataService.AddReportsAsync(reports);

                await NotificationService.Success("Le rapport a bien été enregistré, merci pour votre participation.", "Rapport enregistré");

                //Nettoyage du formulaire.
                IPAddresses = string.Empty;
                ReportDateTime = DateTime.Now;
                SelectedCategories.Clear();
                SelectedNameCategories.Clear();
                ReportComment = String.Empty;
            }
            catch (Exception ex)
            {
                await NotificationService.Error("Désolé mais une erreur est survenue lors de l'enregistrement de votre rapport, vérifier vos informations et réessayer.", "Erreur");
            }
        }
    }

    #endregion
}
