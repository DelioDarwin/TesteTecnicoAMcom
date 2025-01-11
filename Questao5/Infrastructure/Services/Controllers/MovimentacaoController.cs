using IdempotentAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Questao5.Infrastructure.Services.Master;
using IdempotentAPI.Cache.DistributedCache.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace Questao5.Infrastructure.Services.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteProvider contaCorrenteProvider;
        private readonly IContaCorrenteRepository contaCorrenteRepository;

        public ContaCorrenteController(IContaCorrenteProvider contaCorrenteProvider,
            IContaCorrenteRepository contaCorrenteRepository)
        {
            this.contaCorrenteProvider = contaCorrenteProvider;
            this.contaCorrenteRepository = contaCorrenteRepository;
            
        }

        // GET: api/<ProductController>
        [HttpGet]
        [Route(template: "RetornaConta/{id}")]
        public object RetornaConta([FromRoute] string id)
        {
            return contaCorrenteProvider.Get(id);
        }

        [HttpGet]
        public  List<object> GetAll()
        {
            return  contaCorrenteProvider.GetAll();
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<string> Create(MovimentoRequest movimentoRequest)
        {
            var Empotencia = await contaCorrenteRepository.RetornaEmpotencia(movimentoRequest.ChaveIdempotencia);

            if (Empotencia == null)
            {
                IdEmpotencia emp = new IdEmpotencia();

                string rawRequestBody = JsonConvert.SerializeObject(movimentoRequest); // works

                emp.Chave_Idempotencia = movimentoRequest.ChaveIdempotencia;
                emp.Requisicao = rawRequestBody;
                emp.Resultado = "Movimentação realizada com sucesso!";

                await contaCorrenteRepository.CreateIdEmpotencia(emp);
                await contaCorrenteRepository.Create(movimentoRequest);

                return "Movimentação realizada com sucesso!";
            }
            else
                return "Esta mesma requisição já foi feita!";


        }



        // Private method to read the raw request body
        private async Task<string> ReadRawRequestBodyAsync()
        {
            // Rewind the stream so we can read it from the beginning
            Request.Body.Seek(0, SeekOrigin.Begin);

            // Read the request body
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var rawBody = await reader.ReadToEndAsync();
                // Rewind the stream again for further consumption by other middleware
                Request.Body.Seek(0, SeekOrigin.Begin);
                return rawBody;
            }
        }
    }
}