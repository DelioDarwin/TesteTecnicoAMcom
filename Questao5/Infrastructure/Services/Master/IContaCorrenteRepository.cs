namespace Questao5.Infrastructure.Services.Master
{
    public interface IContaCorrenteRepository
    {

        Task<object?> RetornaEmpotencia(string id);
        Task CreateIdEmpotencia(IdEmpotencia idEmpotencia);
        Task<string?> Create(MovimentoRequest movimentoRequest);
    }
}