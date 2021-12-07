namespace FirstBlazorApp.Data;

public class Report
{
    public int Id { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public int IdIpAddress { get; set; }

    public IpAddress IpAddress { get; set; } = new IpAddress();

    public List<Category> Categories { get; set; } = new List<Category>();
}
