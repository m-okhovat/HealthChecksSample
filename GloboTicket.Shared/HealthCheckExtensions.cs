using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.Shared
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddAzureServiceBusTopicHealthCheck(this IHealthChecksBuilder builder,
            string connectionString,
            string topicName,
            string name = default,
            HealthStatus failureStatus = HealthStatus.Degraded,
            IEnumerable<string> tags = default,
            TimeSpan? timeout = default)
        {
            return builder.AddCheck(name ?? $"Azure Service Bus: {topicName}",
                new AzureServiceBusHealthCheck(connectionString, topicName), failureStatus, tags, timeout);
        }
    }
}
