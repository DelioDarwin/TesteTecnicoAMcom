using MediatR;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentoCommand : IRequest<Guid>
    {
        public string ChaveIdempotencia { get; set; }
        public Guid ContaCorrenteId { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; } // "C" ou "D"
    }
}
