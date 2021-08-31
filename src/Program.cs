using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Builds the webhost
            var host = BuildWebHost(args);

            // Run the Host
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SetupConfiguration)  // Add app config
                .UseStartup<Startup>()
            .Build();

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder bldr)
        {
            // To clear all existing sources of config files. [Learning purposes]
            bldr.Sources.Clear();

            // Root dir for config files
            bldr.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", false, true)
                .AddEnvironmentVariables();


        }
    }
}
