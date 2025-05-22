using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure;

public class ContaCorrenteRepository
{
    private readonly string _connectionString;

    public ContaCorrenteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new SqliteConnection(_connectionString);

    public async Task<ContaCorrente?> ObterContaPorNumero(int numero)
    {
        var sql = "SELECT Numero, Nome, Saldo FROM ContaCorrente WHERE Numero = @Numero";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Numero = numero });
    }

    public async Task<bool> AtualizarSaldo(int numero, decimal saldo)
    {
        var sql = "UPDATE ContaCorrente SET Saldo = @Saldo WHERE Numero = @Numero";
        using var conn = Connection;
        var result = await conn.ExecuteAsync(sql, new { Saldo = saldo, Numero = numero });
        return result > 0;
    }
}
