using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageResizer.Svc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                    logging.AddEventLog(configuration =>
                    {
                        configuration.SourceName = "ImageResizer";
                        configuration.LogName = "ImageResizer";
                    })
                )
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseWindowsService(c => c.ServiceName = "ImageResizer");
        //Script Powershell pour inscrire un nouveau service Windows :
        //  New-Service -Name "MyService" -DisplayName "Nom du service" -BinaryPathName "C:\Path\To\exe\File.exe"

        //Script Powershell pour cr?er un journal et une source de journal :
        //  New-EventLog -LogName "ImageResizer" -Source "ImageResizer"
    }
}
