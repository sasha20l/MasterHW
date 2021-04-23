
using MetricsAgent.DAL.interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private IHddMetricsRepository _repository;

        private PerformanceCounter _hddCounter;


        public HddMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IHddMetricsRepository>();
            _hddCounter = new PerformanceCounter("Hdd", "% Hdd Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости Hdd
            var hddUsageInPercents = Convert.ToInt32(_hddCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new Models.HddMetric { Time = time, Value = hddUsageInPercents });

            return Task.CompletedTask;

        }
    }
}
