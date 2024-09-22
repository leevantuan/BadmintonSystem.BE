using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.AdditionalService;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Category;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Gender;

namespace BadmintonSystem.Domain.Abstractions.Dappers;
public interface IUnitOfWork
{
    IGenderRepository Genders { get; }
    ICategoryRepository Categories { get; }
    IAdditionalServiceRepository AdditionalServices { get; }
}
