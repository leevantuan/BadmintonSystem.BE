using BadmintonSystem.Domain.Abstractions.Dappers;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Gender;

namespace BadmintonSystem.Infrastructure.Dapper;
public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IGenderRepository genderRepository)
    {
        Genders = genderRepository;
    }

    public IGenderRepository Genders { get; }
}
