using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.Services
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MyBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Runninng ExecuteAsync");
            while (!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    DateTime today = DateTime.Now;
                    if(today.Day == 1)
                    {
                        var _userBackgroundService = scope.ServiceProvider.GetRequiredService<UserBackgroundService>();
                        var _frequentBackgroundService = scope.ServiceProvider.GetRequiredService<FrequentBackgroundService>();
                        await _userBackgroundService.UpdateUserStatus();
                        await _frequentBackgroundService.CreateFrequentExpenses(today);
                    }
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
            }
        }
    }
}