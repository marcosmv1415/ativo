using APINotification.Data;
using APINotification.Models;
using APINotification.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace APINotification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtivoController : Controller
    {
        private readonly BancoContext _context;
        private IConfiguration _configuration;
        private IAtivoRepository _ativoRepository;
        public AtivoController(BancoContext context, IConfiguration configuration, IAtivoRepository ativoRepository)
        {
            _context = context;
            _configuration = configuration;
            _ativoRepository = ativoRepository;
        }
        /// <summary>
        /// Obtém os dados de um ativo com base em seu ticker.
        /// </summary>
        /// <param name="ticker">O ticker do ativo.</param>
        /// <returns>Os dados do ativo.</returns>
        // GET: api/ativo/
        [HttpGet("{ticker}")]
        public async Task<IActionResult> GetAtivo(string ticker)
        {
            try
            {
                var symbol = ticker;
                var range = "30d";
                var interval = "1d";
                var url = $"https://query2.finance.yahoo.com/v8/finance/chart/{symbol}?range={range}&interval={interval}";

                using var client = new HttpClient();
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<Resultado>(content);

                    _ativoRepository.LimparAtivos(resultado.Chart.Result[0].Meta.Symbol);

                    foreach (var item in resultado.Chart.Result)
                    {
                        for (int i = 0; i < item.Timestamp.Count(); i++)
                        {
                            Ativo ativo = new Ativo();
                            ativo.Dia_Ativo = i + 1;
                            ativo.Nome_Ativo = item.Meta.Symbol.ToString();
                            ativo.Data_Ativo = DateTimeOffset.FromUnixTimeSeconds(item.Timestamp[i]).DateTime;
                            ativo.Val_Abertura = Convert.ToDecimal(item.Indicators.Quote[0].Open[i]);
                            ativo.Val_Fechamento = Convert.ToDecimal(item.Indicators.Quote[0].Close[i]);

                            if (ativo.Dia_Ativo > 1)
                            {
                                double variacaoPorcentagem = ((item.Indicators.Quote[0].Close[i] - item.Indicators.Quote[0].Close[i - 1]) / item.Indicators.Quote[0].Close[i - 1]) * 100;
                                double variacaoPorcentagemmes = ((item.Indicators.Quote[0].Close[i] - item.Indicators.Quote[0].Close[0]) / item.Indicators.Quote[0].Close[0]) * 100;
                                ativo.Variacao_Dia_Anterior = Convert.ToDecimal(variacaoPorcentagem);
                                ativo.Variacao_Primeiro_Dia = Convert.ToDecimal(variacaoPorcentagemmes);
                            }
                            else
                            {
                                ativo.Variacao_Dia_Anterior = 0;
                                ativo.Variacao_Primeiro_Dia = 0;
                            }
                            ativo.Min_Dia = Convert.ToDecimal(item.Indicators.Quote[0].Low[i]);
                            ativo.Max_Dia = Convert.ToDecimal(item.Indicators.Quote[0].High[i]);

                            _ativoRepository.Cadastrar(ativo);

                        }


                    }
                    var ativos = _ativoRepository.ObterDadosAtivo(resultado.Chart.Result[0].Meta.Symbol);

                    return Ok(ativos);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
