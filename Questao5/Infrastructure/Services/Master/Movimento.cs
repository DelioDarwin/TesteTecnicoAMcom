using System.ComponentModel.DataAnnotations;

namespace Questao5.Infrastructure.Services.Master
{
    public class Movimento
    {
        public string IdContaCorrente { get; set; }

        [Key]
        public string IdMovimento { get; set; }

        public string DataMovimento { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}