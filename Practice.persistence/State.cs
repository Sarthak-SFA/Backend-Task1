using System.ComponentModel.DataAnnotations;

namespace Practice.Persistence;

public sealed class State
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}


