using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet_domain.Entities;
using myfinance_web_dotnet_infra;
using myfinance_web_dotnet_service.Interfaces;

namespace myfinance_web_dotnet_service
{
    public class PlanoContaService : IPlanoContaService
    {
        private readonly MyFinanceDbContext _dbContext;

        public PlanoContaService(MyFinanceDbContext dbContext) // Construtor
        {
            _dbContext = dbContext; // injeção de dependencia --> estrutura dentro da camada de infra
        } 
        public void Cadastrar(PlanoConta Entidade)
        {
            var dbset = _dbContext.PlanoConta;

            if (Entidade.Id == null) 
            {
                dbset.Add(Entidade); // Insere um novo registro no banco
            }
            else {
                dbset.Attach(Entidade); // Trata o registro de um objeto (atualiza)
                _dbContext.Entry(Entidade).State = EntityState.Modified; // Estado de modificação
            }
            _dbContext.SaveChanges();
        }

        public void Excluir(int Id)
        {
            var PlanoConta = new PlanoConta() { Id = Id }; // objeto do tipo da entidade PlanoConta --> id a ser excluido 
            _dbContext.Attach(PlanoConta);
            _dbContext.Remove(PlanoConta);
            _dbContext.SaveChanges();
        }

        public List<PlanoConta> ListarRegistros()
        {
            var dbSet = _dbContext.PlanoConta;
            return dbSet.ToList();
        }

        public PlanoConta RetornarRegistro(int Id)
        {
            return _dbContext.PlanoConta.Where(x => x.Id == Id).First();
        }
    }
}