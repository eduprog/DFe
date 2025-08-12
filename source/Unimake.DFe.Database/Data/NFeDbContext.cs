using Microsoft.EntityFrameworkCore;
using Unimake.DFe.Database.Data.Mapping;
using Unimake.DFe.Database.Models;

namespace Unimake.DFe.Database.Data;

/// <summary>
/// DbContext para o sistema de NFe
/// </summary>
public class NFeDbContext : DbContext
{
    public NFeDbContext(DbContextOptions<NFeDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<EnviNFeEntity> EnviNFes { get; set; }
    public DbSet<NFeEntity> NFes { get; set; }
    public DbSet<EmitenteEntity> Emitentes { get; set; }
    public DbSet<DestinatarioEntity> Destinatarios { get; set; }
    public DbSet<ItemNFeEntity> ItensNFe { get; set; }
    public DbSet<TotalNFeEntity> TotaisNFe { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configurações de mapeamento
        modelBuilder.ApplyConfiguration(new EnviNFeMapping());
        modelBuilder.ApplyConfiguration(new NFeMapping());
        modelBuilder.ApplyConfiguration(new EmitenteMapping());
        modelBuilder.ApplyConfiguration(new DestinatarioMapping());
        modelBuilder.ApplyConfiguration(new ItemNFeMapping());
        modelBuilder.ApplyConfiguration(new TotalNFeMapping());

        // Configurações de precisão para campos decimais
        ConfigureDecimalProperties(modelBuilder);
    }

    private static void ConfigureDecimalProperties(ModelBuilder modelBuilder)
    {
        // Configurar precisão padrão para decimais
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                {
                    if (!property.GetAnnotations().Any(a => a.Name == "Relational:ColumnType"))
                    {
                        property.SetColumnType("decimal(18,2)");
                    }
                }
            }
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // String de conexão padrão para desenvolvimento PostgreSQL
            optionsBuilder.UseNpgsql("Host=localhost;Database=nfedatabase;Username=postgres;Password=postgres");
        }
    }
}
