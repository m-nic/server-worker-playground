using Hangfire.Common;
using Hangfire.Server;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayNowWorker.Hangfire
{
    internal class MiniProfilerAttribute : JobFilterAttribute, IServerFilter
    {
        public void OnPerforming(PerformingContext context)
        {
            MiniProfiler.StartNew(
                context.BackgroundJob.GetType().FullName + " - " + context.BackgroundJob.Id
                );
        }

        public void OnPerformed(PerformedContext context)
        {

            MiniProfiler.Current.Stop();
        }
    }
}
