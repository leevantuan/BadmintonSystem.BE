using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Gender;
using BadmintonSystem.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Dapper.SqlMapper;

namespace BadmintonSystem.Infrastructure.Dapper.Repositories;
public class GenderRepository : IGenderRepository
{
    // Takes string
    private readonly IConfiguration _configuration;

    public GenderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> AddAsync(Gender entity)
    {
        // @ is paramater
        // Get from entites
        var sql = "Insert Into Gender (Id, Name) VALUES (@Id, @Name)";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(sql, entity);
            return result;
        }
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        // @ is paramater
        // Get from entites
        var sql = "DELETE FROM Gender WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result;
        }
    }

    public async Task<IReadOnlyList<Gender>> GetAllAsync()
    {
        var sql = "SELECT Id, Name FROM Gender";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<Gender>(sql);
            return result.ToList();
        }
    }

    public async Task<Gender?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT Id, Name FROM Gender WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QuerySingleOrDefaultAsync<Gender>(sql, new { Id = id });
            return result;
        }
    }

    public async Task<int> UpdateAsync(Gender entity)
    {
        var sql = "UPDATE Gender SET Name = @Name WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(sql, entity);
            return result;
        }
    }
}
