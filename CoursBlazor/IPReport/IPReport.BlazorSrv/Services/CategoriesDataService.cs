using IPReport.Data;
using Microsoft.EntityFrameworkCore;

namespace IPReport.BlazorSrv.Services;

/// <summary>
///     Service d'accès aux données pour les catégories.
/// </summary>
public class CategoriesDataService
{
    #region Fields

    /// <summary>
    ///     Contexte de données à utiliser.
    /// </summary>
    private readonly IPReportContext _Context;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="CategoriesDataService"/>.
    /// </summary>
    /// <param name="context">Contexte de données à utiliser.</param>
    /// <exception cref="ArgumentNullException">Exception déclenchée si un argument obligatoire n'est pas fourni.</exception>
    public CategoriesDataService(IPReportContext context)
    {
        _Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Obtient l'ensemble des catégories existantes.
    /// </summary>
    /// <returns>Liste des catégories.</returns>
    public async Task<List<Category>> GetCategories() => await _Context.Categories.ToListAsync();

    #endregion
}
