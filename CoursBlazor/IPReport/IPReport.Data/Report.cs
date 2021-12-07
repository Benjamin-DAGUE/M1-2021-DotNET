namespace IPReport.Data;

/// <summary>
///     Représente un rapport d'abus d'une adresse IP.
/// </summary>
public class Report
{
    /// <summary>
    ///     Obtient ou définit l'identifiant du rapport d'abus.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Obtient ou définit la date et heure de détection de l'abus.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    ///     Obtient ou définit l'identifiant de l'adresse reporté.
    /// </summary>
    public int IPId { get; set; }

    /// <summary>
    ///     Obtient ou définit l'adresse reporté.
    /// </summary>
    public IP IP { get; set; } = new IP();

    /// <summary>
    ///     Obtient ou définit la liste des catégories associées.
    /// </summary>
    public List<Category> Categories { get; set; } = new List<Category>();

    /// <summary>
    ///     Obtient ou définit le commentaire du rapport.
    /// </summary>
    public string? Comment { get; set; }
}
