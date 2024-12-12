namespace BadmintonSystem.Contract.Abstractions.Entities;

public interface IAuditable : IDateTracking, IUserTracking, ISoftDelete
{ }
