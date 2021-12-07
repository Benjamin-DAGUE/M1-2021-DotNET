using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.RegularExpressions;

namespace IPReport.BlazorSrv.Components;

/// <summary>
///     Le composant <see cref="Checker"/> peremet à l'utilisateur de saisir une IP dans une zone de saisie
///     et de rediriger l'utilisateur vers la page correspondante.
///     Le composant vérifie la validité de l'adresse IP et permet la redirection uniquement si la saisie est valide.
/// </summary>
public partial class Checker
{
    #region Properties

    /// <summary>
    ///     Obtient ou définit le gestionnaire de navigation.
    /// </summary>
    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    /// <summary>
    ///     Obtient ou définit l'adresse IP à tester.
    /// </summary>
    private string? IpToCheck { get; set; }

    /// <summary>
    ///     Obtient ou définit si l'adresse IP à tester est valide.
    /// </summary>
    private bool IsValid { get; set; }

    #endregion

    #region Methods

    /// <summary>
    ///     Méthode déclenchée lorsque l'utilisateur modifie l'adresse IP à tester.
    ///     Réaliser la vérification de la validité de l'adresse IP.
    /// </summary>
    /// <param name="ipToCheck">Adresse IP à tester.</param>
    private void IpToCheckChanged(string ipToCheck)
    {
        //On récupère la saisie utilisateur dans le paramètre.
        IpToCheck = ipToCheck;
        //L'expression régulière suivante vérifie si l'adresse IP est valide (IP ou subnet).
        IsValid = Regex.IsMatch(ipToCheck, @"^(?>\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(?>\.(?>25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}(?>/24)?$");
    }

    private void OnKeyPressed(KeyboardEventArgs kbd)
    {
        if (NavigationManager == null)
        {
            throw new Exception($"Le service {nameof(NavigationManager)} n'est pas initialisé");
        }

        if (kbd.Key?.ToLower() == "enter" && IsValid)
        {
            NavigationManager.NavigateTo($"/{(IpToCheck?.EndsWith("/24") == true ? "subnet" : "ip")}/{System.Web.HttpUtility.UrlEncode(IpToCheck)}", false);
        }
    }

    #endregion
}
