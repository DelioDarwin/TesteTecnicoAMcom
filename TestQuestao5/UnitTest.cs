using Moq;
using Questao5.Infrastructure.Services.Master;

namespace TestQuestao5
{
    public class UnitTest
    {
        private readonly Mock<IContaCorrenteProvider> _contaCorrenteProvider;
        private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepository;

        //private List<object>? _saldo;
        private List<object>? returnValue = null;

        public UnitTest()
        {
            _contaCorrenteProvider = new Mock<IContaCorrenteProvider>();
            _contaCorrenteRepository = new Mock<IContaCorrenteRepository>();

        }

      
        public void GetTeste()
        {
            string id = "B6BAFC09 -6967-ED11-A567-055DFA4A16C9";

            Mock<IContaCorrenteProvider> contaCorrenteR = new Mock<IContaCorrenteProvider>();
            contaCorrenteR.Setup(m => m.GetAll())
               .Returns(returnValue);

            List<object>? result = contaCorrenteR.Object.GetAll();

        }

        [Fact]
        public void ContaInexistenteAsync()
        {
            string id = "nnnn";
            object? saldo = new object();
            Mock<IContaCorrenteProvider> contaCorrenteR = new Mock<IContaCorrenteProvider>();
            contaCorrenteR.Setup(m => m.Get(id))
               .Returns(returnValue);

            var result = contaCorrenteR.Object.GetAll();

            // Assert
            Xunit.Assert.Null(result);
        }

        [Fact]
        public void ContaInativa()
        {
            string id = "B6BAFC09 -6967-ED11-A567-055DFA4A16C9";

            var ret = _contaCorrenteProvider.Object.GetAll();

            // Assert
            Xunit.Assert.Null(ret);
        }

    }
}