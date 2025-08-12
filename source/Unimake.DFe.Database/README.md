# 🚀 Unimake DFe Database - Entity Framework 9 + PostgreSQL

Este projeto fornece uma implementação moderna para persistência de dados de NFe usando Entity Framework 9 com PostgreSQL, baseado nas classes XML da biblioteca Unimake.Business.DFe.

## 📋 Funcionalidades

- ✅ **Modelos EF Core 9** baseados na estrutura da EnviNFe da Unimake
- ✅ **PostgreSQL** como banco de dados principal
- ✅ **Relacionamentos bem definidos** entre NFe, Emitente, Destinatário e Itens
- ✅ **Índices otimizados** para consultas de performance
- ✅ **Migrations automáticas** do Entity Framework
- ✅ **Logging estruturado** com Microsoft.Extensions.Logging
- ✅ **Validações de dados** e constraints de integridade
- ✅ **Tipos de dados apropriados** para PostgreSQL

## 🏗️ Estrutura do Banco de Dados

### Tabelas Principais

1. **EnviNFe** - Lotes de envio de NFe
2. **NFe** - Notas fiscais eletrônicas
3. **Emitente** - Dados dos emitentes
4. **Destinatario** - Dados dos destinatários
5. **ItemNFe** - Itens das notas fiscais
6. **TotalNFe** - Totalizadores das notas fiscais

### Relacionamentos

```
EnviNFe (1) ──┐
              │
              ├─→ NFe (N) ──┬─→ ItemNFe (N)
              │             │
              │             ├─→ TotalNFe (1)
              │             │
              │             ├─→ Emitente (N:1)
              │             │
              │             └─→ Destinatario (N:1)
```

## 🚀 Como Usar

### 1. Configuração Inicial

```bash
# Navegar para o diretório do projeto
cd "d:\trab\estudos\dotnet\DFe\source\Unimake.DFe.Database"

# Restaurar pacotes
dotnet restore

# Compilar
dotnet build
```

### 2. Configurar String de Conexão PostgreSQL

Edite o arquivo `appsettings.json` ou configure a string de conexão no `Program.cs`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=nfedatabase;Username=postgres;Password=postgres"
  }
}
```

**Pré-requisitos:**
- PostgreSQL 12+ instalado e rodando
- Criar o banco de dados: `CREATE DATABASE nfedatabase;`
- Usuário `postgres` com senha `postgres` (ou ajustar conforme sua configuração)

### 3. Executar o Projeto

```bash
# Executar aplicação
dotnet run
```

O projeto irá:
- ✅ Criar automaticamente o banco de dados
- ✅ Aplicar migrations necessárias
- ✅ Exibir um menu interativo

### 4. Exemplo de Uso Programático

```csharp
// Injeção de dependência
services.AddDbContext<NFeDbContext>(options =>
    options.UseSqlServer(connectionString));
services.AddScoped<NFeConversionService>();

// Uso no código
public async Task ProcessarNFe(EnviNFe enviNfe)
{
    var entity = await _conversionService.ConvertAndSaveEnviNFeAsync(enviNfe);
    
    Console.WriteLine($"NFe salva com ID: {entity.Id}");
    Console.WriteLine($"Lote: {entity.IdLote}");
    Console.WriteLine($"NFes no lote: {entity.NFes.Count}");
}
```

## 📊 Exemplos de Consultas

### Consultar NFes por Período

```csharp
var nfesDoMes = await context.NFes
    .Where(n => n.DataEmissao.Month == DateTime.Now.Month)
    .Include(n => n.Emitente)
    .Include(n => n.Total)
    .ToListAsync();
```

### Relatório de Vendas por Emitente

```csharp
var relatorio = await context.NFes
    .Include(n => n.Emitente)
    .Include(n => n.Total)
    .GroupBy(n => n.Emitente.RazaoSocial)
    .Select(g => new
    {
        Emitente = g.Key,
        TotalNFes = g.Count(),
        ValorTotal = g.Sum(n => n.Total.ValorTotalNFe)
    })
    .ToListAsync();
```

### Top Produtos Vendidos

```csharp
var topProdutos = await context.ItensNFe
    .GroupBy(i => new { i.CodigoProduto, i.Descricao })
    .Select(g => new
    {
        Produto = g.Key.Descricao,
        Quantidade = g.Sum(i => i.QuantidadeComercial),
        ValorTotal = g.Sum(i => i.ValorTotal)
    })
    .OrderByDescending(p => p.Quantidade)
    .Take(10)
    .ToListAsync();
```

## 🛠️ Migrations

### Criar Nova Migration

```bash
dotnet ef migrations add NomeDaMigration
```

### Aplicar Migrations

```bash
dotnet ef database update
```

### Remover Última Migration

```bash
dotnet ef migrations remove
```

## 📦 Dependências

- **.NET 9.0**
- **Microsoft.EntityFrameworkCore 9.0.0**
- **Npgsql.EntityFrameworkCore.PostgreSQL 9.0.0**
- **Microsoft.EntityFrameworkCore.Tools 9.0.0**
- **Unimake.Business.DFe** (referência ao projeto existente)
- **PostgreSQL 12+**

## 🔧 Configurações Avançadas

### Pool de Conexões

```csharp
services.AddDbContextPool<NFeDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
        npgsqlOptions.CommandTimeout(30);
    }));
```

### Logging do EF Core

```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

### Performance PostgreSQL

- ✅ Índices em campos de busca frequente (ChaveAcesso, NumeroNFe, DataEmissao)
- ✅ Campos de texto com tamanho limitado  
- ✅ Relacionamentos com lazy loading opcional
- ✅ Queries com Include() para evitar N+1
- ✅ Tipos de dados nativos do PostgreSQL
- ✅ Suporte a JSONB para campos não estruturados

## 🚨 Validações Implementadas

- ✅ **Chave de acesso única** para NFe
- ✅ **CNPJ/CPF únicos** para emitentes
- ✅ **Campos obrigatórios** validados
- ✅ **Relacionamentos consistentes**
- ✅ **Tipos de dados apropriados** (decimal para valores monetários)

## 🔄 Conversão de Dados

O serviço `NFeConversionService` faz a conversão automática entre:

- `EnviNFe` (Unimake) ↔ `EnviNFeEntity` (EF Core)
- `NFe` (Unimake) ↔ `NFeEntity` (EF Core)
- `Emit` (Unimake) ↔ `EmitenteEntity` (EF Core)
- `Dest` (Unimake) ↔ `DestinatarioEntity` (EF Core)
- `Det` (Unimake) ↔ `ItemNFeEntity` (EF Core)
- `Total` (Unimake) ↔ `TotalNFeEntity` (EF Core)

## 🎯 Casos de Uso

1. **Importação de XMLs** existentes de NFe
2. **Auditoria de documentos** fiscais
3. **Relatórios gerenciais** e análises
4. **Integração com ERPs** e sistemas de gestão
5. **Compliance fiscal** e rastreabilidade
6. **Business Intelligence** sobre vendas

## 📈 Próximos Passos

- [ ] Implementar importação de diretório de XMLs
- [ ] Adicionar suporte para outros bancos (PostgreSQL, MySQL)
- [ ] Criar APIs REST para acesso aos dados
- [ ] Implementar cache distribuído
- [ ] Adicionar testes unitários e de integração
- [ ] Dashboard web para visualização de dados

## 🤝 Contribuição

Este projeto foi criado como exemplo de implementação usando as melhores práticas do .NET 9 e Entity Framework Core 9. Sinta-se livre para adaptar conforme suas necessidades específicas.

## 📝 Licença

Este projeto segue a mesma licença do projeto principal Unimake.Business.DFe.
