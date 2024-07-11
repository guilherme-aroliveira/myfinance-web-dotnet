using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet_domain.Entities.Base;
using myfinance_web_dotnet_infra.Interfaces.Base;

namespace myfinance_web_dotnet_infra
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {
        protected DbContext Db;
        protected DbSet<TEntity> DbSetContext;
        protected Repository(DbContext dbContext)
        {
            Db = dbContext;
            DbSetContext = Db.Set<TEntity>();
        }

        public void Cadastrar(TEntity Entidade)
        {
            if (Entidade.Id == null) 
            {
                DbSetContext.Add(Entidade); // Insere um novo registro no banco
            }
            else {
                DbSetContext.Attach(Entidade); // Trata o registro de um objeto (atualiza)
                Db.Entry(Entidade).State = EntityState.Modified; // Estado de modificação
            }
            Db.SaveChanges();
        }

        public void Excluir(int Id)
        {
            var entidade = new TEntity() { Id = Id }; // objeto do tipo da entidade PlanoConta --> id a ser excluido 
            Db.Attach(entidade);
            Db.Remove(entidade);
            Db.SaveChanges();
        }

        public List<TEntity> ListarRegistros()
        {
            return DbSetContext.ToList();
        }

        public TEntity RetornarRegistro(int Id)
        {
            return DbSetContext.Where(x => x.Id == Id).First();
        }
    }
}