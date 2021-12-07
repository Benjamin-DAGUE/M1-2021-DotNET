namespace IPReport.Data;

/// <summary>
///     Représente une adresse IP.
/// </summary>
public class IP
{
    #region Properties

    /// <summary>
    ///     Obtient ou définit l'identifiant de l'adresse IP.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    ///     Obtient ou définit l'adresse IP.
    /// </summary>
    public string IPAddress { get; set; } = string.Empty;
    
    /// <summary>
    ///     Obtient le sous-réseau de l'adresse.
    /// </summary>
    public string IPSubnet => IPAddress[0..IPAddress.LastIndexOf('.')] + ".0/24";

    /// <summary>
    ///     Obtient ou définit le numéro de l'IP dans le sous-réseau.
    /// </summary>
    public int IPNumberInSubnet => int.Parse(IPAddress[(IPAddress.LastIndexOf('.') + 1)..]);

    /// <summary>
    ///     Obtient ou définit la liste des rapports
    /// </summary>
    public List<Report> Reports { get; set; } = new List<Report>();

    #endregion
}
