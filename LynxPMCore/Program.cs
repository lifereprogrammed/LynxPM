using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LynxPMCore.Controllers;

namespace LynxPMCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);//.Run();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                //SeedDatabase.InitializeAreas(services);
                //SeedDatabase.InitializeEquipmentArea(services);
                //SeedDatabase.InitializeConditions(services);
                //SeedDatabase.InitializeDueStatus(services);
                //SeedDatabase.InitializeEquipment(services);
                //SeedDatabase.InitializeTaskTypes(services);
                //SeedDatabase.InitializeTerm(services);
                //SeedDatabase.InitializeLTasks(services);
                //SeedDatabase.InitializeTaskTrackerStages(services);
                //SeedDatabase.InitializeTaskTracker(services);
                
                
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
