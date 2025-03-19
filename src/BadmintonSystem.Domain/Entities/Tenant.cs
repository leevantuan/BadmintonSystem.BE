using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class Tenant : EntityBase<Guid>
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string? HotLine { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public string? Slogan { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? ConnectionString { get; set; }
}
