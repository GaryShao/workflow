using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFood.BackendService.Application.ServiceInterfaces;
using SFood.BackendService.Common.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFood.BackendService.Entry
{
    public class OrderHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IApplicationLifetime _appLifetime;
        private readonly OrderExpirationOptions _options;
        private Timer _timer;        

        public OrderHostedService(IServiceProvider services
            , ILoggerFactory loggerFactory
            , IApplicationLifetime appLifetime
            , IOptionsSnapshot<OrderExpirationOptions> options
            )
        {
            _logger = loggerFactory.CreateLogger(nameof(OrderHostedService));
            _appLifetime = appLifetime;
            Services = services;
            _options = options.Value;
        }

        public IServiceProvider Services { get; }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order Service Hosted Service is starting.");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order Service Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(cancellationToken);
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Order Service Hosted Service is working.");

            try
            {
                using (var scope = Services.CreateScope())
                {
                    var orderService =
                        scope.ServiceProvider
                            .GetRequiredService<IOrderService>();

                    await orderService.CancelOrdersAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
            }
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_options.Interval * 60));
            return Task.CompletedTask;
        }
    }
}
