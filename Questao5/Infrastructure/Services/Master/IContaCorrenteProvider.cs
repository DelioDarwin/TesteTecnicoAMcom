namespace Questao5.Infrastructure.Services.Master
{
    public interface IContaCorrenteProvider
    {
        object? Get(string id);

        List<object>? GetAll();
    }
}