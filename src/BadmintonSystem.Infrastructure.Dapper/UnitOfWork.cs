using BadmintonSystem.Domain.Abstractions.Dappers;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.AdditionalService;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Category;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Club;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Gender;

namespace BadmintonSystem.Infrastructure.Dapper;
public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IGenderRepository genderRepository,
                      ICategoryRepository categoryRepository,
                      IAdditionalServiceRepository additionalServiceRepository,
                      IClubRepository clubRepository)
    {
        Genders = genderRepository;
        Categories = categoryRepository;
        AdditionalServices = additionalServiceRepository;
        Clubs = clubRepository;
    }

    public IGenderRepository Genders { get; }
    public ICategoryRepository Categories { get; }
    public IAdditionalServiceRepository AdditionalServices { get; }
    public IClubRepository Clubs { get; }
}
