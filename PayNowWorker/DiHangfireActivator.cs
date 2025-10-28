using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PayNowWorker
{
    internal class DiHangfireActivator: JobActivator
    {
        private IServiceProvider _serviceProvider;

        public DiHangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            throw new InvalidOperationException("Use BeginScope to resolve jobs.");
        }

        public override JobActivatorScope BeginScope()
        {
            return new DiJobActivatorScope(_serviceProvider.CreateScope());
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new DiJobActivatorScope(_serviceProvider.CreateScope());
        }

        private class DiJobActivatorScope : JobActivatorScope
        {
            private IServiceScope _scope;

            internal DiJobActivatorScope(IServiceScope scope)
            {
                _scope = scope;
            }
            
            public override object Resolve(Type type)
            {
                return _scope.ServiceProvider.GetRequiredService(type);
            }

            public override void DisposeScope()
            {
                _scope.Dispose();
            }
        }
    }
}


//using Hangfire;
//using Microsoft.Extensions.DependencyInjection;
//using System;

//namespace PayNowWorker
//{
//    internal class DiHangfireActivator : JobActivator
//    {
//        private IServiceScopeFactory _serviceProvider;

//        public DiHangfireActivator(IServiceScopeFactory serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public override object ActivateJob(Type jobType)
//        {
//            throw new InvalidOperationException("Use BeginScope to resolve jobs.");
//        }

//        public override JobActivatorScope BeginScope()
//        {
//            return new DiJobActivatorScope(_serviceProvider.CreateScope());
//        }

//        public override JobActivatorScope BeginScope(JobActivatorContext context)
//        {
//            return new DiJobActivatorScope(_serviceProvider.CreateScope());
//        }

//        private class DiJobActivatorScope : JobActivatorScope
//        {
//            private IServiceScope _scope;

//            internal DiJobActivatorScope(IServiceScope scope)
//            {
//                _scope = scope;
//            }

//            public override object Resolve(Type type)
//            {
//                object v = _scope.ServiceProvider.GetRequiredService(type);
//                return v;
//            }

//            public override void DisposeScope()
//            {
//                _scope.Dispose();
//            }
//        }
//    }
//}