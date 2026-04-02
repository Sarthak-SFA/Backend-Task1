namespace Practice.Core.Requests;

public sealed class UpdateStatesRequest
{
    public IList<CreateStateRequest> States { get; set; } = new List<CreateStateRequest>();
}