using System.Threading.Tasks;
using MassTransit;
using SparkRoseDigital_Template.Core.Events;

namespace SparkRoseDigital_Template.WorkerServices.FooService
{
    public class FooConsumer : IConsumer<IFooEvent>
    {
        public Task Consume(ConsumeContext<IFooEvent> context) =>
            Task.CompletedTask;
    }
}
