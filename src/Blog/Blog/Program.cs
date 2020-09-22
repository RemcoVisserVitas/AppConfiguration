using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog
{
   public class Program
   {
      public static void Main(string[] args)
      {
         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                 webBuilder
                    .ConfigureAppConfiguration(ConfigureAzureAppConfiguration)
                    .UseStartup<Startup>();
              });

      private static void ConfigureAzureAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
      {
         const string DefaultConfigurationSettingsLabel = "Default";
         string environmentName = hostingContext.HostingEnvironment.EnvironmentName;

         var settings = config.Build();
         config.AddAzureAppConfiguration(o => o.Connect(settings["connectionStrings.AppConfiguration"])
            .ConfigureKeyVault(kv => { kv.SetCredential(new DefaultAzureCredential()); })
            .Select(KeyFilter.Any, labelFilter: DefaultConfigurationSettingsLabel)
            .Select(KeyFilter.Any, environmentName)
            .ConfigureRefresh(refresh => { refresh.Register("BLOG:blogSection:configVersion", label: DefaultConfigurationSettingsLabel, refreshAll: true); }));
      }
   }
}
