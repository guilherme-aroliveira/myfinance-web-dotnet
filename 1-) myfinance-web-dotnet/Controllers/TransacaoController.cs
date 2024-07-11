using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet_domain.Entities;
using myfinance_web_dotnet_service.Interfaces;

namespace myfinance_web_dotnet.Controllers
{
    [Route("[controller]")]
    public class TransacaoController : Controller
    {
        private readonly ILogger<TransacaoController> _logger;

        // Injeção de dependencia
        private readonly ITransacaoService _transacaoService;
        private readonly IPlanoContaService _planoContaService;

        public TransacaoController(
            ILogger<TransacaoController> logger,
            ITransacaoService transacaoService,
            IPlanoContaService planoContaService
            )
        {
            _logger = logger; // injeta um estrutura de log
            _transacaoService = transacaoService; // Injeção de dependencia
            _planoContaService = planoContaService;
        }

        [HttpGet] // Diretiva que informa que o IActionResult é um HTTP Get
        [Route("Index")]
        public IActionResult Index() // renderiza a View com o nome Index
        {
            var listaTransacoes = _transacaoService.ListarRegistros();
            List<TransacaoModel> listaTransacaoModel = new List<TransacaoModel>(); // Mapea o conteúdo para a Model

            foreach (var item in listaTransacoes) {
                var itemTransacao = new TransacaoModel() 
                {
                    Id = item.Id,
                    Historico = item.Historico,
                    Data = item.Data,
                    Valor = item.Valor,
                    Tipo = item.PlanoConta.Tipo,
                    PlanoContaId = item.PlanoContaId
                };

                listaTransacaoModel.Add(itemTransacao);
            }

            ViewBag.ListaTransacao = listaTransacaoModel;

            return View();
        }

        [HttpGet] // Renderiza a tela vazia
        [Route("Cadastrar")]
        [Route("Cadastrar/{Id}")]
        public IActionResult Cadastrar(int? Id) 
        {
            var ListaPlanoContas = new SelectList(_planoContaService.ListarRegistros(), "Id", "Descricao");

            var itemTransacao = new TransacaoModel() {
                Data = DateTime.Now,
                ListaPlanoContas = ListaPlanoContas
            };

            if(Id != null)
            {
                var transacao = _transacaoService.RetornarRegistro((int)Id);
            
                itemTransacao.Id = transacao.Id;
                itemTransacao.Historico = transacao.Historico;
                itemTransacao.Data = transacao.Data;
                itemTransacao.Valor = transacao.Valor;
                itemTransacao.PlanoContaId = transacao.PlanoContaId;
            }

            return View(itemTransacao);
        }

        [HttpPost] 
        [Route("Cadastrar")]
        [Route("Cadastrar/{Id}")]
        public IActionResult Cadastrar(TransacaoModel model) 
        {
            var transacao = new Transacao()
            {
                Id = model.Id,
                Historico = model.Historico,
                Data = model.Data,
                Valor = model.Valor,
                PlanoContaId = model.PlanoContaId
            };

            _transacaoService.Cadastrar(transacao);

            return RedirectToAction("Index");
        }

        [HttpGet] // Renderiza a tela vazia
        [Route("Excluir/{Id}")]
        public IActionResult Excluir(int? Id) 
        {
            _transacaoService.Excluir((int)Id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}