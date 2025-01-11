using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Services.Master;
using System.Collections.Generic;
using System.Threading.Tasks;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Runtime.InteropServices;

namespace Questao5.Infrastructure.Services.Master
{
    public class ContaCorrenteProvider : IContaCorrenteProvider 
    {
        private readonly DatabaseConfig databaseConfig;


        public ContaCorrenteProvider(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public object? Get(string id)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);


            // Validações de negócio
            var conta =  connection.QuerySingleOrDefault<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @idcontacorrente", new { idcontacorrente = id });

            if (conta == null)
                throw new Exception("Conta não cadastrada.");

            if (conta.Ativo == 0)
                throw new Exception("Conta inativa.");

            // Cálculo do saldo
            var creditos =  connection.QuerySingle<decimal>(
                "SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @idcontacorrente AND tipomovimento = 'C'", new { idcontacorrente = id });
            var debitos =  connection.QuerySingle<decimal>(
                "SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @idcontacorrente AND tipomovimento = 'D'", new { idcontacorrente = id });

            var saldo = creditos - debitos;

            if (conta != null)
            {
                return new SaldoResponse
                {
                    NumeroConta = conta.Numero,
                    NomeTitular = conta.Nome,
                    DataHoraConsulta = DateTime.Now,
                    Saldo = saldo
                };
            }
            else
                return null;
        }
                
        public List<object>? GetAll()
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            List<object>? list;
            list = connection.Query<object>("SELECT * FROM contacorrente ") as List<object>;
  


            return list;

        }

    }
}
