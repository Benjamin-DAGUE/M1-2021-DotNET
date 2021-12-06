using System.Web;

namespace FirstBlazorApp.Pages;

public partial class Index
{
    private string? IpAddress { get; set; }

    private async Task CheckIpAddress()
    {
        if (string.IsNullOrWhiteSpace(IpAddress) == false)
        {
            //HttpUtiliy.UrlEncode permet de s'assurer d'encoder proprement les caractères spéciaux lors de la construction d'une Url.
            NavigationManager.NavigateTo($"/ip/{HttpUtility.UrlEncode(IpAddress)}", false);
        }
        else
        {
            await NotificationService.Error("Veuillez saisir une adresse IP", "Erreur");
        }
    }
}
