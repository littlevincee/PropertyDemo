using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PropertyDemo.Data;

namespace PropertyDemo.Migrations
{
    public static class MigrationHelper
    {
        public static async Task MigrateAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(MigrationHelper));
                logger.LogError(ex, "Error during database migration");
                throw;
            }
        }
    }
}
