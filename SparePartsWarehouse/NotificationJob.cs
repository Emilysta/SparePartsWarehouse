using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparePartsWarehouse
{
    [DisallowConcurrentExecution]
    public class NotificationJob : IJob
    {
        private readonly ILogger<NotificationJob> _logger;
        public NotificationJob(ILogger<NotificationJob> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Tygodniowe zamówienie części");
            CheckStockSystem.CheckWeeklyStock();
            return Task.CompletedTask;
        }
    }

    public class NotificationJob2 : IJob
    {
        private readonly ILogger<NotificationJob> _logger;
        public NotificationJob2(ILogger<NotificationJob> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Codzienne sprawdzanie ilości potrzebnych części");
            CheckStockSystem.CheckTwiceADay();
            return Task.CompletedTask;
        }
    }
}
