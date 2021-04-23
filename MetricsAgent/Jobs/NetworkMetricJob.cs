
using MetricsAgent.DAL.interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private INetworkMetricsRepository _repository;

        private PerformanceCounter _NetworkCounter;


        public NetworkMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<INetworkMetricsRepository>();
            _NetworkCounter = new PerformanceCounter("Network", "% Network Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости Network
            var NetworkUsageInPercents = Convert.ToInt32(_NetworkCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new Models.NetworkMetric { Time = time, Value = NetworkUsageInPercents });

            return Task.CompletedTask;

        }
    }
}
