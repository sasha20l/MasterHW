﻿
using MetricsAgent.Controllers;
using MetricsAgent.DAL.Repositories;

namespace MetricsAgent.DAL.interfaces
{
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {
    }
}
