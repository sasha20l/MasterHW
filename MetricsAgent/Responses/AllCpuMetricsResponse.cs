﻿
using System.Collections.Generic;
using MetricsAgent.DTO;

namespace MetricsAgent.DAL.Responses
{
    public class AllCpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }

    }
}
