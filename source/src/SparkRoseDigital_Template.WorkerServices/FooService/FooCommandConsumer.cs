using System.Threading.Tasks;
using MassTransit;
using SparkRoseDigital_Template.Core.Events;

namespace SparkRoseDigital_Template.WorkerServices.FooService
{
    public class FooCommandConsumer : IConsumer<IFooCommand>
    {
        public Task Consume(ConsumeContext<IFooCommand> context) =>
            Task.CompletedTask;

        public class FooCommandConsumerDefinition : ConsumerDefinition<FooCommandConsumer>
        {
            public FooCommandConsumerDefinition()
            {
                EndpointName = $"{WorkerAssemblyInfo.ServiceName.ToLower()}-foo-command-queue";
            }

            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FooCommandConsumer> consumerConfigurator)
            {
                // Configure message retry with millisecond intervals.
                // endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
                // Creates only a queue, without topic and subscription.
                // endpointConfigurator.ConfigureConsumeTopology = false;

                // Use the outbox to prevent duplicate events from being published.
                // endpointConfigurator.UseInMemoryOutbox();
            }
        }
    }
}
