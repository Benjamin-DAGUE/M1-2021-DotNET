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
    #region Fake data
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

        context.IPs.Add(new IP()
        {
            IPAddress = "218.200.6.4",
            Reports = new List<Report>()
            {
                new Report()
                {
                    DateTime = DateTime.Now.AddDays(-5),
                    Categories = new List<Category>()
                    {
                        hackingCategory,
                        bruteCategory
                    },
                    Comment = "Tentative d'intrusion par brute force sur RDP."
                },
                new Report()
                {
                    DateTime = DateTime.Now.AddDays(-7),
                    Categories = new List<Category>()
                    {
                        botCategory,
                        portScanningCategory
                    }
                },
                new Report()
                {
                    DateTime = DateTime.Now.AddDays(-12),
                    Categories = new List<Category>()
                    {
                        botCategory,
                        portScanningCategory
                    }
                },
                new Report()
                {
                    DateTime = DateTime.Now.AddDays(-14),
                    Categories = new List<Category>()
                    {
                        botCategory,
                        portScanningCategory
                    }
                },
                new Report()
                {
                    DateTime = DateTime.Now.AddDays(-16),
                    Categories = new List<Category>()
                    {
                        botCategory,
                        portScanningCategory
                    }
                },
                new Report()
                {
                    DateTime = DateTime.Now.AddDays(-30),
                    Categories = new List<Category>()
                    {
                        botCategory,
                        portScanningCategory
                    }
                },
                new Report()
                {
                    DateTime = DateTime.Now.AddDays(-36),
                    Categories = new List<Category>()
                    {
                        botCategory,
                        portScanningCategory
                    }
                },
                new Report()
                {
                    DateTime = DateTime.Now.AddDays(-40),
                    Categories = new List<Category>()
                    {
                        botCategory,
                        portScanningCategory
                    }
                }
            }
        });


        context.SaveChanges();
    }
    #endregion
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
