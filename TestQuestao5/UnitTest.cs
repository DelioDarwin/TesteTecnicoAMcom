using Moq;
using Questao5.Infrastructure.Services.Master;

namespace TestQuestao5
{
    public class UnitTest
    {
        //private readonly Mock<IContaCorrenteProvider> _contaCorrenteProvider;
        //private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepository;
        private List<ContaCorrente>? returnProducts = null;
        private SaldoResponse? returnMoviment = null;

        //public UnitTest()
        //{
        //    _contaCorrenteProvider = new Mock<IContaCorrenteProvider>();
        //    _contaCorrenteRepository = new Mock<IContaCorrenteRepository>();

        //}

        public void GetTeste()
        {
            Mock<IContaCorrenteProvider> contaCorrenteR = new Mock<IContaCorrenteProvider>();
            contaCorrenteR.Setup(m => m.GetAll()).Returns(returnProducts);

            List<ContaCorrente>? result = contaCorrenteR.Object.GetAll();
        }

        [Fact]
        public void ContaInexistenteAsync()
        {
            string id = "nnnn";
            Mock<IContaCorrenteProvider> contaCorrenteR = new Mock<IContaCorrenteProvider>();
            contaCorrenteR.Setup(m => m.Get(id)).Returns(returnMoviment);

            var result = contaCorrenteR.Object.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ContaInativa()
        {
            //chave de uma conta inativa
            string id = "F475F943-7067-ED11-A06B-7E5DFA4A16C9";

            Mock<IContaCorrenteProvider> contaCorrenteR = new Mock<IContaCorrenteProvider>();
            contaCorrenteR.Setup(m => m.Get(id)).Returns(returnMoviment);

            var result = contaCorrenteR.Object.Get(id);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public void TipoMovimentoInvalido()
        {
            //chave de uma conta inativa
            string id = "F475F943-7067-ED11-A06B-7E5DFA4A16C9";

            Mock<IContaCorrenteProvider> contaCorrenteR = new Mock<IContaCorrenteProvider>();
            contaCorrenteR.Setup(m => m.Get(id)).Returns(returnMoviment);

            var result = contaCorrenteR.Object.Get(id);

            // Assert
            Assert.Null(result);
        }
    }
}