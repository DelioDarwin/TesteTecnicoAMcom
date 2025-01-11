using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Services.Master
{
    public class ContaCorrentetRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContaCorrentetRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<object?> RetornaEmpotencia(string id)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            // Validações de negócio
            var idcontacorrente = connection.QuerySingleOrDefault<ContaCorrente>(
                "SELECT * FROM IdEmpotencia WHERE Chave_Idempotencia = @Chave_Idempotencia", new { Chave_Idempotencia = id });

            return idcontacorrente;
        }


        public async Task CreateIdEmpotencia(IdEmpotencia idEmpotencia)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            IdEmpotencia emp = new IdEmpotencia();

            emp.Chave_Idempotencia = idEmpotencia.Chave_Idempotencia;
            emp.Requisicao = idEmpotencia.Requisicao;
            emp.Resultado = idEmpotencia.Resultado;

            await connection.ExecuteAsync(
                "INSERT INTO IdEmpotencia (Chave_Idempotencia, Requisicao, Resultado) VALUES (@chaveIdempotencia, @requisicao, @resultado)",
                new { chaveIdempotencia = emp.Chave_Idempotencia, requisicao = emp.Requisicao, resultado = emp.Resultado });
        }

        public async Task Create(MovimentoRequest movimentoRequest)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var conta = await connection.QuerySingleOrDefaultAsync<ContaCorrente>(
             "SELECT * FROM contacorrente WHERE idcontacorrente = @ContaCorrenteId", new { ContaCorrenteId = movimentoRequest.ContaCorrenteId });

            if (conta == null)
                throw new Exception("Conta não cadastrada.");

            if (conta.Ativo == 0)
                throw new Exception("Conta inativa.");

            if (movimentoRequest.Valor <= 0)
                throw new Exception("Valor inválido.");

            if (movimentoRequest.TipoMovimento != "C" && movimentoRequest.TipoMovimento != "D")
                throw new Exception("Tipo de movimento inválido.");

            // Persistência do movimento
            var idMovimento = Guid.NewGuid();
            await connection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@idmovimento, @idcontacorrente, @datamovimento, @tipomovimento, @valor)",
                new { idmovimento = idMovimento.ToString(), idcontacorrente = movimentoRequest.ContaCorrenteId, datamovimento = DateTime.Now, tipomovimento = movimentoRequest.TipoMovimento, valor = movimentoRequest.Valor });
        }
    }
}