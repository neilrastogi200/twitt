using System;
using Autofac;
using Twitter2.Infrastructure;

namespace Twitter2
{
    class Program
    {
        private static void Main(string[] args)
        {

            var container = ContainerConfig.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var application = scope.Resolve<IApplication>();
                var result = application.Run();
            }


        }
    }
}
