﻿using IPReport.Data;
using Microsoft.EntityFrameworkCore;

namespace IPReport.BlazorSrv.Services;

/// <summary>
///     Service d'accès aux données pour les adresses IP.
/// </summary>
public class IpsDataService
{
    #region Fields

    /// <summary>
    ///     Contexte de données à utiliser.
    /// </summary>
    private readonly IPReportContext _Context;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="IpsDataService"/>.
    /// </summary>
    /// <param name="context">Contexte de données à utiliser.</param>
    /// <exception cref="ArgumentNullException">Exception déclenchée si un argument obligatoire n'est pas fourni.</exception>
    public IpsDataService(IPReportContext context)
    {
        _Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Obtient l'IP avec l'identifiant associé.
    /// </summary>
    /// <param name="id">Identifiant de l'IP à récupérer.</param>
    /// <returns>Retour l'IP à l'identifiant associé ou null.</returns>
    public async Task<IP?> GetIpAsync(int id) => await _Context.IPs.FindAsync(id);

    /// <summary>
    ///     Obtient l'IP avec l'identifiant associé.
    /// </summary>
    /// <param name="ipAddress">Identifiant de l'IP à récupérer.</param>
    /// <returns>Retour l'IP à l'identifiant associé ou null.</returns>
    public async Task<IP?> GetIpAsync(string ipAddress) => await _Context.IPs.FirstOrDefaultAsync(i => i.IPAddress == ipAddress);

    #endregion
}
