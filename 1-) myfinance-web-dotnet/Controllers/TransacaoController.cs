using Microsoft.AspNetCore.Mvc;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet_domain.Entities;
using myfinance_web_dotnet_service;
using myfinance_web_dotnet_service.Interfaces;

namespace myfinance_web_dotnet.Controllers
{
    [Route("[controller]")]
    public class TransacaoController : Controller
    {
        private readonly ILogger<TransacaoController> _logger;

        // Injeção de dependencia
        private readonly ITransacaoService _transacaoService;

        public TransacaoController(
            ILogger<TransacaoController> logger,
            ITransacaoService transacaoService)
        {
            _logger = logger; // injeta um estrutura de log
            _transacaoService = transacaoService; // Injeção de dependencia
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
            if(Id != null)
            {
                var transacao = _transacaoService.RetornarRegistro((int)Id);
            
                var itemTransacao = new TransacaoModel()
                {
                    Id = transacao.Id,
                    Historico = transacao.Historico,
                    Data = transacao.Data,
                    Valor = transacao.Valor,
                    PlanoContaId = transacao.PlanoContaId
                };

                return View(itemTransacao);
            }
            else {
                return View();
            }
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