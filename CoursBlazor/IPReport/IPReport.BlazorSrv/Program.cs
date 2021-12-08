using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using IPReport.BlazorSrv.Services;
using IPReport.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Services configuration

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorise(options =>
{
    options.ChangeTextOnKeyPress = true;
})
      .AddBootstrap5Providers()
      .AddFontAwesomeIcons();
builder.Services.AddDbContext<IPReportContext>(options => options.UseSqlite(@"Data Source=.\bin\ipreport.db;"));
builder.Services.AddTransient<ReportsDataService>();
builder.Services.AddTransient<IpsDataService>();
builder.Services.AddTransient<CategoriesDataService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    bool generateFakeData = false;

    if (generateFakeData == true)
    {
        #region Fake data

        Random rnd = new Random();

        using (IServiceScope scope = app.Services.CreateScope())
        {
            IPReportContext context = scope.ServiceProvider.GetService<IPReportContext>() ?? throw new Exception($"Impossible d'initialiser le service {nameof(IPReportContext)}");

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Category hackingCategory = new Category()
            {
                Name = "Hacking"
            };
            Category bruteCategory = new Category()
            {
                Name = "Brute force"
            };
            Category ddosCategory = new Category()
            {
                Name = "DDoS"
            };
            Category portScanningCategory = new Category()
            {
                Name = "Port scanning"
            };
            Category botCategory = new Category()
            {
                Name = "Bot"
            };
            Category otherCategory = new Category()
            {
                Name = "Other"
            };

            context.Categories.Add(hackingCategory);
            context.Categories.Add(bruteCategory);
            context.Categories.Add(ddosCategory);
            context.Categories.Add(portScanningCategory);
            context.Categories.Add(botCategory);
            context.Categories.Add(otherCategory);

            Category[] categories = new Category[]
            {
                hackingCategory,
                bruteCategory,
                ddosCategory,
                portScanningCategory,
                botCategory,
                otherCategory
            };

            Func<Task<Report>> generateReportAsync = async () => await Task.Run(() =>
            {
                Report report = new Report()
                {
                    DateTime = DateTime.Now.AddDays(rnd.NextDouble() * -1 * rnd.Next(0, 365)),
                };

                for (int i = 0; i < rnd.Next(0, 3); i++)
                {
                    Category cat = categories[rnd.Next(0, categories.Length - 1)];

                    if (report.Categories.Contains(cat) == false)
                    {
                        report.Categories.Add(cat);
                    }
                }

                return report;
            });

            Func<int, Task<List<Report>>> generateReportsAsync = async (count) => await Task.Run(() => Enumerable.Range(0, count).Select(async i => await generateReportAsync()).Select(t => t.Result).ToList());

            Func<Task<IP>> generateIpAsync = async () =>
            {
                return await Task.Run(async () =>
                {
                    IP ip = new IP()
                    {
                        IPAddress = $"{rnd.Next(200, 201)}.{rnd.Next(112, 113)}.{rnd.Next(1, 254)}.{rnd.Next(1, 254)}"
                    };

                    ip.Reports = await generateReportsAsync(rnd.Next(1, 100));

                    return ip;
                });
            };

            Func<int, List<IP>> generateIps = (count) => Enumerable.Range(0, count).Select(async i => await generateIpAsync()).Select(t => t.Result).ToList();

            context.IPs.AddRange(generateIps(200));

            context.SaveChanges();
        }
        
        #endregion
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
