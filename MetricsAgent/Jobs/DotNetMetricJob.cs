
using MetricsAgent.DAL.interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob : IJob
    {

        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private IDotNetMetricsRepository _repository;

        private PerformanceCounter _dotnetCounter;


        public DotNetMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IDotNetMetricsRepository>();
            _dotnetCounter = new PerformanceCounter("DotNet", "% DotNet Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости DotNet
            var dotnetUsageInPercents = Convert.ToInt32(_dotnetCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new Models.DotNetMetric { Time = time, Value = dotnetUsageInPercents });

            return Task.CompletedTask;

        }
    }

}


