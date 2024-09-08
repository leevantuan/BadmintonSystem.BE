namespace BadmintonSystem.Domain.Abstractions.Dappers.Repositoies;
public interface IGenericRepository<T>
    where T : class
{
    Task<T?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<T>> GetAllAsync();

    // If want Command
    Task<int> AddAsync(T entity);

    Task<int> UpdateAsync(T entity);

    Task<int> DeleteAsync(Guid id);
}
