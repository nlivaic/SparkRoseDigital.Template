using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SparkRoseDigital_Template.Application.Questions.Queries;
using SparkRoseDigital_Template.Common.Exceptions;
using SparkRoseDigital_Template.Common.Interfaces;
using SparkRoseDigital_Template.Core.Entities;

namespace SparkRoseDigital_Template.Application.Questions.Commands
{
    public class CreateFooCommand : IRequest<FooGetModel>
    {
        public string Text { get; set; }

        private class CreateFooCommandHandler : IRequestHandler<CreateFooCommand, FooGetModel>
        {
            private readonly IRepository<Foo> _repository;
            private readonly IMapper _mapper;

            public CreateFooCommandHandler(
                IRepository<Foo> repository,
                IMapper mapper)
            {
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
                return _mapper.Map<FooGetModel>(foo);
            }
        }
    }
}
