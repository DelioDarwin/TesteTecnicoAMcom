using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Infrastructure.Services.Master
{
    public class ContaCorrente
    {
        [Key]
        public string? IdContaCorrente { get; set; }

        public int Numero { get; set; }
        public string? Nome { get; set; }
        public int Ativo { get; set; }

        [ForeignKey("IdContaCorrente")]
        public List<Movimento>? Movimentos { get; set; }
    }
}