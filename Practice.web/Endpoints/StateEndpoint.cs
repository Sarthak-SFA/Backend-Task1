
using Microsoft.AspNetCore.Http.HttpResults;
using Practice.Core.Dtos;
using Practice.Core.Requests;
using Practice.Services.Services;

namespace Practice.Web.Endpoints;

public static class StateEndpoints
{
    public static IEndpointRouteBuilder MapStateEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        IEndpointRouteBuilder stateGroup = endpoints.MapMasterGroup().MapGroup("states");


        stateGroup.MapGet("", GetAllStates);
        stateGroup.MapPost("update", UpdateStates);

        return endpoints;
    }

    private static Ok<IEnumerable<StateDto>> GetAllStates(StateService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    private static IResult UpdateStates(StateService service, UpdateStatesRequest request)
    {
        IEnumerable<StateDto>? result = service.UpdateStates(request);

        if (result == null)
        {
            return TypedResults.BadRequest(new
            {
                message = "Unable to save state changes."
            });
        }

        return TypedResults.Ok(new
        {
            message = "State changes saved successfully.",
            data = result
        });
    }
}
