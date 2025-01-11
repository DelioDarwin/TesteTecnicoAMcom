namespace Questao5.Infrastructure.Services.Master
{
    [Serializable]
    public class MovimentoRequest
    {
        public string? ChaveIdempotencia { get; set; }
        public string? ContaCorrenteId { get; set; }
        public decimal   Valor { get; set; }
        public string? TipoMovimento { get; set; } // "C" ou "D"
    }
}