using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using Questao5.Infrastructure.Sqlite;
using System.Diagnostics.Eventing.Reader;

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

        public async Task<string?> Create(MovimentoRequest movimentoRequest)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var conta = await connection.QuerySingleOrDefaultAsync<ContaCorrente>(
             "SELECT * FROM contacorrente WHERE idcontacorrente = @ContaCorrenteId", new { ContaCorrenteId = movimentoRequest.ContaCorrenteId });

            if (conta == null)
                throw new Exception("INVALID_ACCOUNT");

            if (conta.Ativo == 0)
                throw new Exception("INACTIVE_ACCOUNT.");

            if (movimentoRequest.Valor <= 0)
                throw new Exception("INVALID_VALUE");

            if (movimentoRequest.TipoMovimento != "C" && movimentoRequest.TipoMovimento != "D")
                throw new Exception("INVALID_TYPE");

            if (movimentoRequest.ChaveIdempotencia != null && movimentoRequest.ChaveIdempotencia != "")
              {
                var Empotencia = await RetornaEmpotencia(movimentoRequest.ChaveIdempotencia);

                if (Empotencia == null)
                {
                    IdEmpotencia emp = new IdEmpotencia();

                    string rawRequestBody = JsonConvert.SerializeObject(movimentoRequest); // works

                    emp.Chave_Idempotencia = movimentoRequest.ChaveIdempotencia;
                    emp.Requisicao = rawRequestBody;
                    emp.Resultado = "Movimentação realizada com sucesso!";

                    // Persistência da Empotencia
                    await CreateIdEmpotencia(emp);

                    // Persistência do movimento
                    var idMovimento = Guid.NewGuid();
                    await connection.ExecuteAsync(
                        "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@idmovimento, @idcontacorrente, @datamovimento, @tipomovimento, @valor)",
                        new { idmovimento = idMovimento.ToString(), idcontacorrente = movimentoRequest.ContaCorrenteId, datamovimento = DateTime.Now, tipomovimento = movimentoRequest.TipoMovimento, valor = movimentoRequest.Valor });


                    return "Movimentação realizada com sucesso!";
                }
                else
                    return "Esta mesma requisição já foi feita!";
            }
            else
                  throw new Exception("ENTER_A_EMPOTENCY_VALUE");



        }
    }
}