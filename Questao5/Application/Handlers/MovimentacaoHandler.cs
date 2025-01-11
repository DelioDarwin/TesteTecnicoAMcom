using Dapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using System.Data;
using Questao5.Infrastructure.Sqlite;


namespace Questao5.Application.Handlers
{
    public class MovimentacaoHandler : IRequestHandler<MovimentoCommand, Guid>
    {
        private readonly IDbConnection _dbConnection;

        public MovimentacaoHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public async Task<Guid> Handle(MovimentoCommand request, CancellationToken cancellationToken)
        {
            // Validações de negócio
            var conta = await _dbConnection.QuerySingleOrDefaultAsync<contacorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = request.ContaCorrenteId.ToString() });

            if (conta == null)
                throw new Exception("Conta não cadastrada.");

            if (conta.ativo == 0)
                throw new Exception("Conta inativa.");

            if (request.Valor <= 0)
                throw new Exception("Valor inválido.");

            if (request.TipoMovimento != "C" && request.TipoMovimento.ToString() != "D")
                throw new Exception("Tipo de movimento inválido.");

            // Persistência do movimento
            var idMovimento = Guid.NewGuid();
            await _dbConnection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@Id, @ContaId, @Data, @Tipo, @Valor)",
                new { Id = idMovimento, ContaId = request.ContaCorrenteId, Data = DateTime.Now, Tipo = request.TipoMovimento, Valor = request.Valor });

            return idMovimento;
        }
    }
}
