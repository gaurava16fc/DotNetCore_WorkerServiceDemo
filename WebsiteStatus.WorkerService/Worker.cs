using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebsiteStatus.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient _client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Dispose();
            _logger.LogInformation("The worker service has been stopped successfully.");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var result = await _client.GetAsync("https://www.interviewbit.com/");
                if (result!=null  && result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("{time}: The website is up and running. Status code: {StatusCode}", DateTimeOffset.Now, result.StatusCode);
                }
                else
                {
                    _logger.LogInformation("{time}: The website is down. Status code: {StatusCode}", DateTimeOffset.Now, result.StatusCode);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
