using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PropertyDemo.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            await host.Services.MigrateAsync();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
