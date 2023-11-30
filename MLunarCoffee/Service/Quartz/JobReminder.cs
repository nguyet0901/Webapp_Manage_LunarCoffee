using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace MLunarCoffee.Service.Quartz
{
    public class JobReminder:IJob
    {
        public JobReminder ()
        {

        }
        public Task Execute(IJobExecutionContext context)
        {
 
            Common.ExecuteDailyLog();
            return Task.CompletedTask;
        }
    }
}
