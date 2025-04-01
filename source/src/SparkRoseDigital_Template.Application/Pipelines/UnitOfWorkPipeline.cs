using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SparkRoseDigital_Template.Common.Interfaces;

namespace SparkRoseDigital_Template.Application.Pipelines;

public class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull

{
    private readonly IUnitOfWork _uow;

    public UnitOfWorkPipeline(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();
        await _uow.SaveAsync();
        return response;
    }
}
