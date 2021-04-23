﻿
using MetricsAgent.Controllers;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.DTO;

namespace MetricsAgent.DAL.interfaces
{
    public interface INetworkMetricsRepository : IRepository<NetworkMetric>
    {
    }
}
