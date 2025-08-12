using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unimake.DFe.Database.Data;

namespace Unimake.DFe.Database;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 Unimake DFe Database Generator - Entity Framework 9 + PostgreSQL");
        Console.WriteLine("====================================================================");

        var host = CreateHostBuilder(args).Build();

        try
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var context = services.GetRequiredService<NFeDbContext>();

            logger.LogInformation("Inicializando banco de dados PostgreSQL...");

            // Garantir que o banco de dados seja criado
            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("✅ Banco de dados criado/atualizado com sucesso!");

            Console.WriteLine("\n📋 Menu de Opções:");
            Console.WriteLine("1. Gerar estrutura do banco (já executado)");
            Console.WriteLine("2. Consultar dados");
            Console.WriteLine("3. Sair");
            
            Console.Write("\nEscolha uma opção: ");
            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Console.WriteLine("✅ Estrutura do banco já foi gerada!");
                    break;
                case "2":
                    await ExibirEstatisticas(context);
                    break;
                case "3":
                    Console.WriteLine("👋 Encerrando aplicação...");
                    break;
                default:
                    Console.WriteLine("❌ Opção inválida!");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro durante a execução: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Detalhes: {ex.InnerException.Message}");
            }
        }

        Console.WriteLine("\nPressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    private static async Task ExibirEstatisticas(NFeDbContext context)
    {
        Console.WriteLine("\n📊 Estatísticas do Banco de Dados:");
        Console.WriteLine("==================================");

        var totalEnviNFe = await context.EnviNFes.CountAsync();
        var totalNFes = await context.NFes.CountAsync();
        var totalEmitentes = await context.Emitentes.CountAsync();
        var totalDestinatarios = await context.Destinatarios.CountAsync();
        var totalItens = await context.ItensNFe.CountAsync();

        Console.WriteLine($"📦 Total de Lotes EnviNFe: {totalEnviNFe}");
        Console.WriteLine($"📄 Total de NFes: {totalNFes}");
        Console.WriteLine($"🏢 Total de Emitentes: {totalEmitentes}");
        Console.WriteLine($"👥 Total de Destinatários: {totalDestinatarios}");
        Console.WriteLine($"📋 Total de Itens: {totalItens}");
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Configurar Entity Framework com PostgreSQL
                services.AddDbContext<NFeDbContext>(options =>
                {
                    var connectionString = "Host=localhost;Database=nfedatabase;Username=postgres;Password=autocom";
                    
                    options.UseNpgsql(connectionString);
                    options.EnableSensitiveDataLogging(true);
                    options.EnableDetailedErrors(true);
                });
                
                // Configurar logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });
            });
}
