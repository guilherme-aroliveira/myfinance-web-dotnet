using Microsoft.AspNetCore.Mvc;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet_domain.Entities;
using myfinance_web_dotnet_service.Interfaces;

namespace myfinance_web_dotnet.Controllers
{
    [Route("[controller]")]
    public class PlanoContaController : Controller
    {
        private readonly ILogger<PlanoContaController> _logger;

        // Injeção de dependencia
        private readonly IPlanoContaService _planoContaService;

        public PlanoContaController(
            ILogger<PlanoContaController> logger,
            IPlanoContaService planoContaService)
        {
            _logger = logger; // injeta um estrutura de log
            _planoContaService = planoContaService; // Injeção de dependencia
        }

        [HttpGet] // Diretiva que informa que o IActionResult é um HTTP Get
        [Route("Index")]
        public IActionResult Index() // renderiza a View com o nome Index
        {
            var listaPlanoContas = _planoContaService.ListarRegistros();
            List<PlanoContaModel> listaPlanoContaModel = new List<PlanoContaModel>(); // Mapea o conteúdo para a Model

            foreach (var item in listaPlanoContas) {
                var itemPlanoConta = new PlanoContaModel() {
                    Id = item.Id,
                    Descricao = item.Descricao,
                    Tipo = item.Tipo
                };

                listaPlanoContaModel.Add(itemPlanoConta);
            }

            ViewBag.listaPlanoConta = listaPlanoContaModel;

            return View();
        }

        [HttpGet] // Renderiza a tela vazia
        [Route("Cadastrar")]
        [Route("Cadastrar/{Id}")]
        public IActionResult Cadastrar(int? Id) 
        {
            if(Id != null)
            {
                var planoConta = _planoContaService.RetornarRegistro((int)Id);
            
                var planoContaModel = new PlanoContaModel()
                {
                    Id = planoConta.Id,
                    Descricao = planoConta.Descricao,
                    Tipo = planoConta.Tipo
                };

                return View(planoContaModel);
            }
            else {
                return View();
            }
        }

        [HttpPost] 
        [Route("Cadastrar")]
        [Route("Cadastrar/{Id}")]
        public IActionResult Cadastrar(PlanoContaModel model) 
        {
            var planoConta = new PlanoConta()
            {
                Id = model.Id,
                Descricao = model.Descricao,
                Tipo = model.Tipo,
            };

            _planoContaService.Cadastrar(planoConta);

            return RedirectToAction("Index");
        }

        [HttpGet] // Renderiza a tela vazia
        [Route("Excluir/{Id}")]
        public IActionResult Excluir(int? Id) 
        {
            _planoContaService.Excluir((int)Id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}