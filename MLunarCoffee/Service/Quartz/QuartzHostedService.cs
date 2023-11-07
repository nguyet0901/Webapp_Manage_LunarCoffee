using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MLunarCoffee.Service.Quartz
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<DailyJob> _job;
        public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IEnumerable<DailyJob> jobs)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _job = jobs;
        }
        public IScheduler Scheduler { get; set; }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            foreach (var myjob in _job)
            {
                var job = CreateJob(myjob);
                var trigger = CreateTrigger(myjob);
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
            await Scheduler.Start(cancellationToken);
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);

        }
        private static IJobDetail CreateJob(DailyJob myjob)
        {
            var type = myjob.Type;
            return JobBuilder.Create(type).WithIdentity(type.FullName).WithDescription(type.Name).Build();
        }
        private static ITrigger CreateTrigger(DailyJob myjob)
        {
            return TriggerBuilder.Create().WithIdentity($"{myjob.Type.FullName}.trigger")
                .WithDailyTimeIntervalSchedule(
                    x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(6, 0))
                    .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 0))
                    .WithIntervalInMinutes(50))
                    .Build();
        }
    }
}
