using System;
using Ecubytes.Aws.Sqs;
using Ecubytes.Integration.Queue;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AwsQueueServiceExtensions
    {
        /// <summary>Add the Amazon Web Services (AWS) SQS Queue service </summary>
        /// <param name="configuration">IConfiguration</param>         
        public static IServiceCollection AddAwsSqsQueueService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IQueueSenderService,SenderService>();
            services.AddTransient<IQueueReceiverService,ReceiverService>();
            services.AddDefaultQueueConfigureProvider(configuration);                                    

            return services;
        }
    }
}
