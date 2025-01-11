using MediatR;

namespace Questao5.Application.Queries.Responses
{
    public class ConsultaSaldoQuery : IRequest<SaldoResponse>
    {
        public Guid IdContaCorrente { get; set; }
    }

}
