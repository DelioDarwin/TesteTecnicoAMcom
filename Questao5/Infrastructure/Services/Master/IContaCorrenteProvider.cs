namespace Questao5.Infrastructure.Services.Master
{
    public interface IContaCorrenteProvider
    {
        SaldoResponse? Get(string id);

        List<ContaCorrente>? GetAll();
    }
}