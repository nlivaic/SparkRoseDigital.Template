using Microsoft.AspNetCore.Routing;

namespace SparkRoseDigital_Template.Api.Helpers;

public interface IEndpoint
{
    public void Register(IEndpointRouteBuilder endpointRouteBuilder);
}
