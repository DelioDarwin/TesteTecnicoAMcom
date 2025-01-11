using System.ComponentModel.DataAnnotations;

namespace Questao5.Infrastructure.Services.Master
{
    public class IdEmpotencia
    {
        [Key]
        public string? Chave_Idempotencia { get; set; }
        public string? Requisicao { get; set; }
        public string? Resultado { get; set; }
    }
}