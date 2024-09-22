using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.AdditionalService;
using BadmintonSystem.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BadmintonSystem.Infrastructure.Dapper.Repositories;

public class AdditionalServiceRepository : IAdditionalServiceRepository
{
    private readonly IConfiguration _configuration;

    public AdditionalServiceRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(AdditionalService entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<AdditionalService>> GetAllAsync()
    {
        var sql = "SELECT Id, Name, Price, ClubId, CategoryId FROM AdditionalService";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<AdditionalService>(sql);
            return result.ToList();
        }
    }

    public async Task<AdditionalService?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM AdditionalService WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QuerySingleOrDefaultAsync<AdditionalService>(sql, new { Id = id });
            return result;
        }
    }

    public Task<int> UpdateAsync(AdditionalService entity)
    {
        throw new NotImplementedException();
    }
}
