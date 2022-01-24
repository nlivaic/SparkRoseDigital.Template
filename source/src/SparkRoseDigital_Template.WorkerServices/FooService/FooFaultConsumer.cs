using System.Threading.Tasks;
using MassTransit;
using SparkRoseDigital_Template.Core.Events;

namespace SparkRoseDigital_Template.WorkerServices.PointService
{
    /// <summary>
    /// This is here only for show.
    /// I have not thought through a proper error handling strategy.
    /// Make VoteCastConsumer throw in order to kick error handling off.
    /// </summary>
    public class FooFaultConsumer : IConsumer<Fault<IFooEvent>>
    {
        public Task Consume(ConsumeContext<Fault<IFooEvent>> context) =>
            Task.CompletedTask;
    }
}
