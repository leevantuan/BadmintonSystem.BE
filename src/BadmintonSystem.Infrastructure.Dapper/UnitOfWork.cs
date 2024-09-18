using BadmintonSystem.Domain.Abstractions.Dappers;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Category;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Gender;

namespace BadmintonSystem.Infrastructure.Dapper;
public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IGenderRepository genderRepository, ICategoryRepository categoryRepository)
    {
        Genders = genderRepository;
        Categories = categoryRepository;
    }

    public IGenderRepository Genders { get; }
    public ICategoryRepository Categories { get; }
}
