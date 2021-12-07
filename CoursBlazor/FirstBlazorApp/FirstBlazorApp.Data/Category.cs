namespace FirstBlazorApp.Data;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Report> Reports { get; set; } = new List<Report>();
}
