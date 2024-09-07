namespace SmartGrowHub.WebApi.Modules;

public interface IEndpointModule
{
    static abstract IEndpointRouteBuilder AddEndpointsTo(IEndpointRouteBuilder routeBuilder);
}
