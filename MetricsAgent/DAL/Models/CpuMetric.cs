﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    public class CpuMetric : Controller
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public long Time { get; set; }
    }
}
