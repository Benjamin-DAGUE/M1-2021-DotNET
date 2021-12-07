namespace IPReport.Data;

/// <summary>
///     Représente une catégorie de rapport d'abus.
/// </summary>
public class Category
{
    /// <summary>
    ///     Obtient ou définit l'identifiant de la catégorie.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    ///     Obtient ou définit le nom de la catégorie.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Obtient ou définit la liste des rapports d'abus associés à la catégorie.
    /// </summary>
    public List<Report> Reports { get; set; } = new List<Report>();
}
