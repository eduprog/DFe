# Guia de Implementação - Source Generators para DTOs

## ⚠️ SOLUÇÃO PARA NÃO MODIFICAR O PROJETO BASE

Como você não pode modificar o projeto base, implementamos uma solução que permite usar **classes wrapper** com herança para configurar a geração de DTOs.

## Estrutura Criada

### 1. Projeto Source Generator
- `Unimake.Business.DFe.SourceGenerators` - Contém o gerador automático de DTOs

### 2. Atributos Disponíveis
- `[SourceGeneratorConfig()]` - Configura geração a partir de classe base
- `[PropertyConfig()]` - Configura propriedades específicas
- `[GenerateDto()]` - Para classes que você pode modificar diretamente
- `[ExcludeFromDto()]` - Exclui propriedades específicas  
- `[DtoPropertyName()]` - Customiza nome da propriedade
- `[DtoRequired()]` - Marca propriedade como obrigatória

## Como Usar (Abordagem Wrapper)

### 1. Criar Classes Wrapper

Crie classes que herdam das originais e configure com atributos:

```csharp
using Unimake.Business.DFe.SourceGenerators.Attributes;
using Unimake.Business.DFe.Xml.NFe;

// Wrapper para EnviNFe sem modificar a classe original
[SourceGeneratorConfig(
    typeof(EnviNFe), 
    DtoType.IncluirRequest, 
    DtoType.AtualizarRequest, 
    DtoType.ConsultarRequest,
    CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
[PropertyConfig("Versao", RequiredForTypes = new[] { DtoType.IncluirRequest })]
[PropertyConfig("IdLote", RequiredForTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest, DtoType.ConsultarRequest })]
[PropertyConfig("NFe", CustomName = "NotasFiscais")]
public class EnviNFeSourceGenerator : EnviNFe
{
    // Esta classe serve apenas como configuração
    // Não precisa implementar nada adicional
}

// Wrapper para NFe individual  
[SourceGeneratorConfig(
    typeof(NFe), 
    DtoType.IncluirRequest, 
    DtoType.AtualizarRequest,
    CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
[PropertyConfig("InfNFe", RequiredForTypes = new[] { DtoType.IncluirRequest })]
[PropertyConfig("Signature", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })]
public class NFeSourceGenerator : NFe
{
}

// Wrapper para InfNFe
[SourceGeneratorConfig(
    typeof(InfNFe), 
    DtoType.IncluirRequest, 
    DtoType.AtualizarRequest,
    CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
[PropertyConfig("Ide", RequiredForTypes = new[] { DtoType.IncluirRequest })]
[PropertyConfig("Emit", RequiredForTypes = new[] { DtoType.IncluirRequest })]
[PropertyConfig("Det", RequiredForTypes = new[] { DtoType.IncluirRequest })]
[PropertyConfig("Total", RequiredForTypes = new[] { DtoType.IncluirRequest })]
[PropertyConfig("Chave", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })] // Calculado automaticamente
[PropertyConfig("Id", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })] // Calculado automaticamente
public class InfNFeSourceGenerator : InfNFe
{
}
```

### 2. DTOs Gerados Automaticamente

O Source Generator criará automaticamente:

```csharp
// EnviNFeIncluirRequest.g.cs
namespace Unimake.Business.DFe.Dtos.NFe
{
    /// <summary>
    /// DTO gerado automaticamente para EnviNFe - Tipo: IncluirRequest
    /// </summary>
    public partial class EnviNFeIncluirRequest
    {
        /// <summary>
        /// Versao
        /// </summary>
        [Required]
        [JsonPropertyName("versao")]
        public string Versao { get; set; }

        /// <summary>
        /// IdLote
        /// </summary>
        [Required]
        [JsonPropertyName("idLote")]
        public string IdLote { get; set; }

        /// <summary>
        /// IndSinc
        /// </summary>
        [JsonPropertyName("indSinc")]
        public SimNao IndSinc { get; set; }

        /// <summary>
        /// NFe
        /// </summary>
        [JsonPropertyName("notasFiscais")]
        public List<NFe> NotasFiscais { get; set; }
    }
}
```

## Configuração do Projeto

### 1. Adicionar Referência ao Source Generator

No seu projeto principal (`Unimake.Business.DFe.csproj`), adicione:

```xml
<ItemGroup>
  <ProjectReference Include="..\Unimake.Business.DFe.SourceGenerators\Unimake.Business.DFe.SourceGenerators.csproj" 
                    OutputItemType="Analyzer" 
                    ReferenceOutputAssembly="false" />
</ItemGroup>
```

### 2. Criar Projeto Separado para Wrappers (Recomendado)

Crie um projeto separado para suas classes wrapper:

```xml
<!-- Unimake.Business.DFe.SourceGenerators.Wrappers.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\Unimake.Business.DFe\Unimake.Business.DFe.csproj" />
    <ProjectReference Include="..\Unimake.Business.DFe.SourceGenerators\Unimake.Business.DFe.SourceGenerators.csproj" 
                      OutputItemType="Analyzer" 
                      ReferenceOutputAssembly="false" />
  </ItemGroup>
</Project>
```

## Vantagens da Abordagem Wrapper

### ✅ **Sem modificar código base**
- O projeto original permanece intocado
- Classes wrapper ficam em projeto separado
- Fácil de remover se necessário

### ✅ **Flexibilidade total**  
- Configuração granular por propriedade
- Diferentes tipos de DTO por operação
- Namespaces customizados

### ✅ **Sincronização automática**
- Qualquer mudança na classe base é refletida automaticamente
- DTOs sempre atualizados

### ✅ **Type Safety**
- Herança mantém compatibilidade de tipos
- Verificação em tempo de compilação

## Configurações Avançadas

### Usar nome da classe wrapper
```csharp
[SourceGeneratorConfig(typeof(EnviNFe), DtoType.Request, UseWrapperName = true)]
public class CustomEnviNFe : EnviNFe { }
// Gera: CustomEnviNFeRequest
```

### Prefixo customizado
```csharp
[SourceGeneratorConfig(typeof(EnviNFe), DtoType.Request, ClassPrefix = "Api")]
public class EnviNFeWrapper : EnviNFe { }
// Gera: ApiEnviNFeRequest
```

### Namespace customizado
```csharp
[SourceGeneratorConfig(typeof(EnviNFe), DtoType.Request, 
    CustomNamespace = "MeuProjeto.ApiModels")]
public class EnviNFeWrapper : EnviNFe { }
```

## Exemplo de Uso em API

```csharp
[ApiController]
[Route("api/[controller]")]
public class NFController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> IncluirNF([FromBody] EnviNFeIncluirRequest request)
    {
        // Converter DTO para classe original
        var enviNFe = new EnviNFe
        {
            Versao = request.Versao,
            IdLote = request.IdLote,
            IndSinc = request.IndSinc,
            NFe = request.NotasFiscais
        };

        // Processar usando as classes originais...
        var xml = enviNFe.GerarXML();
        
        return Ok();
    }
}
```

## Instalação Rápida

Execute o script de configuração:

```powershell
.\setup-source-generators.ps1
```

## Próximos Passos

1. **Crie o projeto wrapper** separado
2. **Adicione as classes wrapper** com configurações
3. **Compile o projeto** para gerar os DTOs
4. **Use os DTOs gerados** em suas APIs
5. **Configure AutoMapper** se desejar mapeamento automático

## Comparação com Outras Abordagens

| Abordagem | Modificar Base | Sincronização | Manutenção | Flexibilidade |
|-----------|----------------|---------------|------------|---------------|
| **Wrapper + Source Generator** | ❌ Não | ✅ Automática | ✅ Mínima | ✅ Total |
| Modificar classes originais | ❌ Sim | ✅ Automática | ⚠️ Média | ✅ Total |
| Reflection Runtime | ❌ Não | ❌ Manual | ❌ Alta | ⚠️ Média |
| Manual | ❌ Não | ❌ Manual | ❌ Muito alta | ✅ Total |

## Conclusão

A abordagem **Wrapper + Source Generator** é perfeita para seu cenário porque:

- ✅ **Não modifica o projeto base** - Requisito fundamental atendido
- ✅ **Automação completa** - DTOs gerados automaticamente
- ✅ **Flexibilidade total** - Controle granular sobre propriedades
- ✅ **Performance** - Zero overhead em runtime  
- ✅ **Maintibilidade** - Sincronização automática com mudanças
- ✅ **Isolamento** - Configurações ficam em projeto separado
    public string Versao { get; set; }

    [DtoRequired(DtoType.IncluirRequest, DtoType.AtualizarRequest)]
    public string IdLote { get; set; }

    public SimNao IndSinc { get; set; }

    [DtoPropertyName("NotasFiscais")]
    public List<NFe> NFe { get; set; }

    // Esta propriedade será excluída dos DTOs de inclusão
    [ExcludeFromDto(DtoType.IncluirRequest)]
    public string DataCriacao { get; set; }
}
```

### 3. DTOs Gerados Automaticamente

O Source Generator criará automaticamente:

```csharp
// EnviNFeIncluirRequest.g.cs
namespace Unimake.Business.DFe.Xml.NFe.Dtos
{
    public partial class EnviNFeIncluirRequest
    {
        [Required]
        [JsonPropertyName("versao")]
        public string Versao { get; set; }

        [Required]
        [JsonPropertyName("idLote")]
        public string IdLote { get; set; }

        [JsonPropertyName("indSinc")]
        public SimNao IndSinc { get; set; }

        [JsonPropertyName("notasFiscais")]
        public List<NFe> NotasFiscais { get; set; }
    }
}
```

## Vantagens da Abordagem

### 1. **Sincronização Automática**
- Quando você atualizar uma propriedade na classe XML, o DTO é atualizado automaticamente
- Sem necessidade de manutenção manual

### 2. **Flexibilidade**
- Controle granular sobre quais propriedades incluir/excluir
- Diferentes tipos de DTO para diferentes operações
- Customização de nomes e validações

### 3. **Performance**
- Geração em tempo de compilação (zero overhead em runtime)
- DTOs otimizados para JSON

### 4. **Type Safety**
- Todos os DTOs são fortemente tipados
- Detecção de erros em tempo de compilação

## Tipos de DTO Suportados

- `DtoType.Request` - DTO genérico de request
- `DtoType.Response` - DTO de response
- `DtoType.IncluirRequest` - Para operações de inclusão
- `DtoType.AtualizarRequest` - Para operações de atualização
- `DtoType.ExcluirRequest` - Para operações de exclusão
- `DtoType.ConsultarRequest` - Para operações de consulta

## Configuração Avançada

### Namespace Customizado
```csharp
[GenerateDto(DtoType.Request, CustomNamespace = "MeuProjeto.ApiModels")]
public class MinhaClasse { }
```

### Sufixo Customizado
```csharp
[GenerateDto(DtoType.Request, CustomSuffix = "Model")]
public class MinhaClasse { }
// Gera: MinhaClasseModel
```

## Exemplo de Uso em API

```csharp
[ApiController]
[Route("api/[controller]")]
public class NFController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> IncluirNF([FromBody] EnviNFeIncluirRequest request)
    {
        // Converter DTO para classe XML
        var enviNFe = new EnviNFe
        {
            Versao = request.Versao,
            IdLote = request.IdLote,
            IndSinc = request.IndSinc,
            NFe = request.NotasFiscais
        };

        // Processar...
        
        return Ok();
    }
}
```

## Próximos Passos

1. Compile o projeto para gerar os DTOs
2. Configure os atributos nas suas classes XML conforme necessário
3. Use os DTOs gerados em suas APIs
4. Configure mapeamentos automáticos (AutoMapper) se desejar

## Alternativas Consideradas

### 1. **Reflection em Runtime**
- ❌ Impacto na performance
- ❌ Menos type safety

### 2. **T4 Templates**
- ❌ Mais complexo de manter
- ❌ Menos integrado com o build

### 3. **AutoMapper com Profiles**
- ✅ Boa para conversões
- ❌ Não gera os DTOs automaticamente

### 4. **Manual**
- ❌ Propenso a erros
- ❌ Manutenção trabalhosa

## Conclusão

O Source Generator é a melhor abordagem para seu caso porque:
- **Automação completa** - Você só precisa marcar as classes
- **Flexibilidade total** - Controle sobre cada propriedade
- **Performance** - Zero overhead em runtime
- **Maintibilidade** - Sincronização automática com mudanças
- **Integração** - Funciona perfeitamente com o ecossistema .NET
