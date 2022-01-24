using System.Threading.Tasks;
using MassTransit;
using SparkRoseDigital_Template.Core.Events;

namespace SparkRoseDigital_Template.WorkerServices.PointService
{
    public class FooConsumer : IConsumer<IFooEvent>
    {
        public Task Consume(ConsumeContext<IFooEvent> context) =>
            throw new System.NotImplementedException();
    }
}
