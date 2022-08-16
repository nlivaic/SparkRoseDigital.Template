using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MediatR;
using SparkRoseDigital_Template.Application.Questions.Queries;
using SparkRoseDigital_Template.Common.Exceptions;
using SparkRoseDigital_Template.Common.Interfaces;
using SparkRoseDigital_Template.Core.Entities;
using SparkRoseDigital_Template.Core.Events;

namespace SparkRoseDigital_Template.Application.Questions.Commands
{
    public class CreateFooCommand : IRequest<FooGetModel>
    {
        public string Text { get; set; }

        private class CreateFooCommandHandler : IRequestHandler<CreateFooCommand, FooGetModel>
        {
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly ISendEndpointProvider _sendEndpoint;
            private readonly IRepository<Foo> _repository;
            private readonly IMapper _mapper;

            public CreateFooCommandHandler(
                IPublishEndpoint publishEndpoint,
                ISendEndpointProvider sendEndpoint,
                IRepository<Foo> repository,
                IMapper mapper)
            {
                _publishEndpoint = publishEndpoint;
                _sendEndpoint = sendEndpoint;
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<FooGetModel> Handle(CreateFooCommand request, CancellationToken cancellationToken)
            {
                if (await _repository.GetSingleAsync(f => f.Text == request.Text) != null)
                {
                    throw new BusinessException($"Cannot create {nameof(Foo)} with text {request.Text}.");
                }
                var foo = new Foo(request.Text);
                await _repository.AddAsync(foo);
                try
                {
                    // sending to queue
                    await _publishEndpoint.Publish<IFooCommand>(new { Text = foo.Text });

                    // sending to topic
                    await _publishEndpoint.Publish<IFooEvent>(new { Text = foo.Text });
                }
                catch (System.Exception ex)
                {
                    throw;
                }
                return _mapper.Map<FooGetModel>(foo);
            }
        }
    }
}
