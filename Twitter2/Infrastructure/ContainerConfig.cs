using Autofac;
using Twitter2.Commands;
using Twitter2.Repository;
using Twitter2.Services;

namespace Twitter2.Infrastructure
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MessageRepository>().As<IMessageRepository>().InstancePerDependency();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerDependency();
            builder.RegisterType<TwitterUserFeedService>().As<ITwitterUserFeedService>().SingleInstance();
            builder.RegisterType<Application>().As<IApplication>().InstancePerDependency();
            builder.RegisterType<ParseCommand>().As<IParseCommand>().InstancePerDependency();
            builder.RegisterType<FollowCommand>().As<ICommand>().InstancePerDependency();
            builder.RegisterType<PostCommand>().As<ICommand>().InstancePerDependency();
            builder.RegisterType<ReadCommand>().As<ICommand>().InstancePerDependency();
            builder.RegisterType<WallCommand>().As<ICommand>().InstancePerDependency();
            builder.RegisterType<CommandFactory>().As<ICommandFactory>().InstancePerDependency();
            builder.RegisterType<DefaultConsole>().As<IConsole>().InstancePerDependency();

            return builder.Build();

        }
    }
}
