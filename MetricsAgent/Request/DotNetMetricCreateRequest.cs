﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Request
{
    public class DotNetMetricCreateRequest
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
    }
}
