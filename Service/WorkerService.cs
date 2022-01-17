using Core.Interfaces.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Service
{
    public class WorkerService : IWorkerService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public WorkerService(ILogger<WorkerService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("DoWork work");
                BackgroundService();
                await Task.Delay(1000 * 60 * 1, cancellationToken);
            }
        }

        private void BackgroundService()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            // var syncService = scope.ServiceProvider.GetService<ISyncService>();
            try
            {
                _logger.LogInformation("Background service working");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"can't update msfa lead, message: {e.Message}");
            }
        }
    }
}
