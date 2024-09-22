using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Club;
using BadmintonSystem.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BadmintonSystem.Infrastructure.Dapper.Repositories;

public class ClubRepository : IClubRepository
{
    private readonly IConfiguration _configuration;

    public ClubRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(Club entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Club>> GetAllAsync()
    {
        var sql = "SELECT * FROM Club";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<Club>(sql);
            return result.ToList();
        }
    }

    public async Task<Club?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM Club WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QuerySingleOrDefaultAsync<Club>(sql, new { Id = id });
            return result;
        }
    }

    public Task<int> UpdateAsync(Club entity)
    {
        throw new NotImplementedException();
    }
}

