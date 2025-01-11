using Dapper;
using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Application.Handlers
{
    public class ConsultaSaldoHandler : IRequestHandler<ConsultaSaldoQuery, SaldoResponse>
    {
        private readonly IDbConnection _dbConnection;

        public ConsultaSaldoHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

 

        public async Task<SaldoResponse> Handle(ConsultaSaldoQuery request, CancellationToken cancellationToken)
        {
            // Validações de negócio
            var conta = await _dbConnection.QuerySingleOrDefaultAsync<contacorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = request.IdContaCorrente });

            if (conta == null)
                throw new Exception("Conta não cadastrada.");

            if (conta.ativo == 0)
                throw new Exception("Conta inativa.");

            // Cálculo do saldo
            var creditos = await _dbConnection.QuerySingleAsync<decimal>(
                "SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'C'", new { Id = request.IdContaCorrente });
            var debitos = await _dbConnection.QuerySingleAsync<decimal>(
                "SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'D'", new { Id = request.IdContaCorrente });

            var saldo = creditos - debitos;

            return new SaldoResponse
            {
                NumeroConta = conta.numero,
                NomeTitular = conta.nome,
                DataHoraConsulta = DateTime.Now,
                Saldo = saldo
            };
        }
    }
}
