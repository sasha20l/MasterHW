
using MetricsAgent.DAL.interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private IRamMetricsRepository _repository;

        private PerformanceCounter _RamCounter;


        public RamMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IRamMetricsRepository>();
            _RamCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости Network
            var RamUsageInPercents = Convert.ToInt32(_RamCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new Models.RamMetric { Time = time, Value = RamUsageInPercents });

            return Task.CompletedTask;

        }
    }
}
