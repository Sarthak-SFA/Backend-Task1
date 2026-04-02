namespace Practice.Core.Requests;

public sealed class CreateStateRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}
