using Microsoft.Extensions.DependencyInjection;
using System;

namespace CAM.Mail
{
    public static class MailExtension
    {
        public static IServiceCollection AddMailContex(this IServiceCollection serviceCollection, Action<MailContextOptionsBuilder> optionsAction = null
            , ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            MailContextOptionsBuilder options = new MailContextOptionsBuilder();
            if (optionsAction != null)
            {
                optionsAction.Invoke(options);
            }
            serviceCollection.AddTransient<IMailManager, MailManager>(x => new MailManager(options));
            return serviceCollection;
        }
    }
}
