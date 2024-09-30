using BadmintonSystem.Domain.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class Sale : AuditableEntity<Guid>
{
    public string Name { get; private set; }
    public decimal Persent { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public bool Status { get; private set; }

    public static Sale CreateSale(string name, decimal persent, DateTime startTime, DateTime endTime, bool? status)
    {
        return new Sale()
        {
            Name = name,
            Persent = persent,
            StartTime = startTime,
            EndTime = endTime,
            Status = status ?? false,
        };
    }

    public void Update(string name, decimal persent, DateTime startTime, DateTime endTime, bool? status)
    {
        Name = name;
        Persent = persent;
        StartTime = startTime;
        EndTime = endTime;
        Status = status ?? false;
    }
}
