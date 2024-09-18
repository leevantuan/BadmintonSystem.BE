using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Category;
using BadmintonSystem.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BadmintonSystem.Infrastructure.Dapper.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IConfiguration _configuration;

    public CategoryRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync()
    {
        var sql = "SELECT Id, Name FROM Category";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<Category>(sql);
            return result.ToList();
        }
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT Id, Name FROM Category WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QuerySingleOrDefaultAsync<Category>(sql, new { Id = id });
            return result;
        }
    }

    public Task<int> UpdateAsync(Category entity)
    {
        throw new NotImplementedException();
    }
}
