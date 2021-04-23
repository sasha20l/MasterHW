﻿using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsAgent.DAL.interfaces;
using MetricsAgent.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace MetricsAgent.Controllers
{
    [Route("api/ramMetrics/available")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _repository;
        private readonly IMapper _mapper;

        public RamMetricsController(IRamMetricsRepository repository, ILogger<RamMetricsController> logger,
                                    IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "RamMetricsController created");
            _repository = repository;
            _mapper = mapper;
        }
        /// <summary>
        /// Возвращает по запросу размер доступной оперативной памяти
        /// </summary>
        /// <returns>Размер доступной оперативной памяти</returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetRamMetrics([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogTrace(1, $"Query GetRamMetrics with params: FromTime={fromTime}, ToTime={toTime}");
            var metrics = _repository.GetByTimePeriod(fromTime.ToUnixTimeSeconds(), toTime.ToUnixTimeSeconds());
            var response = new SelectByTimePeriodRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }
    }
}
