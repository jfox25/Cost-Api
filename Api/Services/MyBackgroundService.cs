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
                    // DateTime today = DateTime.Now;
                    DateTime today = new DateTime(2022, 8, 1); 
                    if(today.Day == 1)
                    {
                        Console.WriteLine("Starting Background Services");
                        var _userBackgroundService = scope.ServiceProvider.GetRequiredService<UserBackgroundService>();
                        var _frequentBackgroundService = scope.ServiceProvider.GetRequiredService<FrequentBackgroundService>();
                        await _userBackgroundService.UpdateUserStatus();
                        Console.WriteLine("Updated Users");
                        Console.WriteLine("Starting to update Frequents");
                        await _frequentBackgroundService.CreateFrequentExpenses(today);
                        Console.WriteLine("Updated Frequents");
                        Console.WriteLine("Finished Background Services");
                    }
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
            }
        }
    }
}