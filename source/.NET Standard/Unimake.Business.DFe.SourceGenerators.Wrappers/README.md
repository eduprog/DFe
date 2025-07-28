# Projeto Wrapper para Source Generators

Este projeto contém as classes wrapper que permitem gerar DTOs automaticamente **sem modificar o projeto base**.

## Estrutura

```
Wrappers/
├── NFe/
│   ├── NFWrappers.cs           # Wrappers principais (EnviNFe, NFe, InfNFe, Ide)
│   ├── EmitDestWrappers.cs     # Wrappers para Emit, Dest
│   ├── DetWrappers.cs          # Wrappers para Det (detalhes/produtos)
│   ├── TotalWrappers.cs        # Wrappers para totais
│   └── TranspPagWrappers.cs    # Wrappers para transporte e pagamento
├── CTe/
│   └── CTeWrappers.cs          # Wrappers para CTe (quando necessário)
└── MDFe/
    └── MDFeWrappers.cs         # Wrappers para MDFe (quando necessário)
```

## Como Funciona

### 1. **Herança Simples**
```csharp
public class EnviNFeWrapper : EnviNFe
{
    // Herda todas as propriedades da classe original
    // Não precisa reimplementar nada
}
```

### 2. **Configuração via Atributos**
```csharp
[SourceGeneratorConfig(typeof(EnviNFe), DtoType.IncluirRequest)]
[PropertyConfig("Versao", RequiredForTypes = new[] { DtoType.IncluirRequest })]
public class EnviNFeWrapper : EnviNFe { }
```

### 3. **DTOs Gerados Automaticamente**
O Source Generator analisa a classe base e gera:
- `EnviNFeIncluirRequest.g.cs`
- `EnviNFeAtualizarRequest.g.cs`  
- `EnviNFeConsultarRequest.g.cs`

## Vantagens

### ✅ **Não modifica código base**
- Projeto `Unimake.Business.DFe` permanece intocado
- Classes originais funcionam normalmente
- Fácil de remover se necessário

### ✅ **Configuração granular**
- Controle por propriedade
- Diferentes regras por tipo de DTO
- Nomes customizados para API

### ✅ **Sincronização automática**
- Mudanças na classe base = DTOs atualizados
- Sem manutenção manual

### ✅ **Type Safety**
- Herança mantém compatibilidade
- Erros detectados em compilação

## Exemplo de DTO Gerado

Para a configuração do `EnviNFeWrapper`, o Source Generator criará:

```csharp
// EnviNFeIncluirRequest.g.cs
namespace Unimake.Business.DFe.Dtos.NFe
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

        [Required]
        [JsonPropertyName("notasFiscais")]
        public List<NFe> NotasFiscais { get; set; }
    }
}
```

## Configurações Disponíveis

### **SourceGeneratorConfig**
- `BaseClassType` - Classe base para extrair propriedades
- `DtoTypes` - Tipos de DTO a gerar (IncluirRequest, AtualizarRequest, etc.)
- `CustomNamespace` - Namespace customizado
- `ClassPrefix` - Prefixo para nome da classe
- `UseWrapperName` - Usar nome do wrapper ao invés da classe base

### **PropertyConfig**  
- `PropertyName` - Nome da propriedade na classe base
- `RequiredForTypes` - Tipos onde é obrigatória
- `ExcludeFromTypes` - Tipos onde deve ser excluída
- `CustomName` - Nome customizado no DTO

## Uso em APIs

```csharp
[HttpPost]
public async Task<IActionResult> IncluirNFe([FromBody] EnviNFeIncluirRequest request)
{
    // Converter para classe original
    var enviNFe = new EnviNFe
    {
        Versao = request.Versao,
        IdLote = request.IdLote, 
        IndSinc = request.IndSinc,
        NFe = request.NotasFiscais
    };

    // Usar normalmente
    var xml = enviNFe.GerarXML();
    
    return Ok();
}
```

## Compilação

```bash
# Compilar para gerar DTOs
dotnet build

# DTOs gerados ficam em:
# obj/Debug/netstandard2.0/generated/
```

## Próximos Passos

1. **Adicione mais wrappers** conforme necessário
2. **Configure propriedades específicas** para seu caso de uso  
3. **Compile e use os DTOs** em suas APIs
4. **Configure AutoMapper** para conversão automática (opcional)
