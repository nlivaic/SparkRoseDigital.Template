using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SparkRoseDigital_Template.Common.Exceptions;
using SparkRoseDigital_Template.Common.Interfaces;
using SparkRoseDigital_Template.Core.Entities;

namespace SparkRoseDigital_Template.Application.Foos.Queries;

public class GetFooQuery : IRequest<FooGetModel>
{
    public Guid Id { get; set; }

    private class GetFooQueryHandler : IRequestHandler<GetFooQuery, FooGetModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Foo> _repository;

        public GetFooQueryHandler(IMapper mapper, IRepository<Foo> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<FooGetModel> Handle(GetFooQuery request, CancellationToken cancellationToken)
        {
            var foo = await _repository.GetSingleAsync(f => f.Id == request.Id);
            if (foo is null)
            {
                throw new EntityNotFoundException(nameof(Foo), request.Id);
            }
            return _mapper.Map<FooGetModel>(foo);
        }
    }
}
