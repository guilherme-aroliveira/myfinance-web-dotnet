using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet_domain.Entities;

namespace myfinance_web_dotnet_infra;

public class MyFinanceDbContext : DbContext
{
    public MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options) : base(options) 
    {
    }

    // Mapear tabelas do DB com uma entidade de Dominio
    public DbSet<PlanoConta> PlanoConta { get; set; }
    public DbSet<Transacao> Transacao { get; set; }
}
