namespace FirstBlazorApp.Data;

public class IpAddress
{
    public int Id { get; set; }

    public string Address { get; set; } = string.Empty;

    public List<Report> Reports { get; set; } = new List<Report>();
}
