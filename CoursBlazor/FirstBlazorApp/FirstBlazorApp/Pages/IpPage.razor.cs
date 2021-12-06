using Microsoft.AspNetCore.Components;
using System.Net;
using System.Web;

namespace FirstBlazorApp.Pages;

public partial class IpPage
{
    #region Fields

    private bool _IsIpAddressValid;

    #endregion

    #region Properties

    [Parameter]
    public string? IpAddress { get; set; }

    #endregion

    #region Methods

    protected override Task OnParametersSetAsync()
    {
        //HttpUtility.UrlDecode permet de décoder les caractères spéciaux de l'url.
        IpAddress = HttpUtility.UrlDecode(IpAddress?.Replace("_", "."));

        if (IpAddress?.Contains("/") == true)
        {
            IpAddress = IpAddress.Substring(0, IpAddress.IndexOf("/"));
        }

        _IsIpAddressValid = IPAddress.TryParse(IpAddress, out IPAddress? address);

        if (_IsIpAddressValid == true && address != null)
        {
            IpAddress = address.ToString();
        }

        return Task.CompletedTask;
    }

    #endregion

}
