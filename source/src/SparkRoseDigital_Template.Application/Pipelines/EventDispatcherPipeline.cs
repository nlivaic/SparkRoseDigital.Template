using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SparkRoseDigital.Infrastructure.MessageBroker;

namespace SparkRoseDigital_Template.Application.Pipelines
{
    public class EventDispatcherPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IRegisteredEventPublisher _eventPublisher;

        public EventDispatcherPipeline(IRegisteredEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            await _eventPublisher.PublishAll();
            return response;
        }
    }
}
