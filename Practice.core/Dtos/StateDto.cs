using System.ComponentModel.DataAnnotations;

namespace Practice.Core.Dtos;

public sealed class StateDto(
    int Id,
    string Name,
    string Code
    )
{
    [Key]
    public int Id { get; } = Id;
    public string Name { get; } = Name;
    public string Code { get; } = Code;
}

