# Script para configurar Source Generators no projeto DFe

Write-Host "=== Configurando Source Generators para Unimake.Business.DFe ===" -ForegroundColor Green

# 1. Adicionar referência ao Source Generator no projeto principal
$projectPath = "d:\trab\estudos\dotnet\DFe\source\.NET Standard\Unimake.Business.DFe\Unimake.Business.DFe.csproj"

if (Test-Path $projectPath) {
    Write-Host "✓ Encontrado projeto principal: $projectPath" -ForegroundColor Green
    
    # Ler conteúdo do projeto
    $content = Get-Content $projectPath -Raw
    
    # Verificar se já existe a referência
    if ($content -notmatch "Unimake.Business.DFe.SourceGenerators") {
        Write-Host "→ Adicionando referência ao Source Generator..." -ForegroundColor Yellow
        
        # Adicionar ItemGroup para o Source Generator antes do </Project>
        $newContent = $content -replace "</Project>", @"
  <ItemGroup>
    <ProjectReference Include="..\Unimake.Business.DFe.SourceGenerators\Unimake.Business.DFe.SourceGenerators.csproj" 
                      OutputItemType="Analyzer" 
                      ReferenceOutputAssembly="false" />
  </ItemGroup>

</Project>
"@
        
        Set-Content $projectPath -Value $newContent -Encoding UTF8
        Write-Host "✓ Referência adicionada com sucesso" -ForegroundColor Green
    } else {
        Write-Host "✓ Referência já existe" -ForegroundColor Green
    }
} else {
    Write-Host "✗ Projeto principal não encontrado: $projectPath" -ForegroundColor Red
    exit 1
}

# 2. Compilar o Source Generator
Write-Host "→ Compilando Source Generator..." -ForegroundColor Yellow
$generatorPath = "d:\trab\estudos\dotnet\DFe\source\.NET Standard\Unimake.Business.DFe.SourceGenerators"

if (Test-Path $generatorPath) {
    Set-Location $generatorPath
    dotnet build --configuration Release
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Source Generator compilado com sucesso" -ForegroundColor Green
    } else {
        Write-Host "✗ Erro ao compilar Source Generator" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "✗ Diretório do Source Generator não encontrado: $generatorPath" -ForegroundColor Red
    exit 1
}

# 3. Compilar projeto principal para gerar DTOs
Write-Host "→ Compilando projeto principal para gerar DTOs..." -ForegroundColor Yellow
Set-Location "d:\trab\estudos\dotnet\DFe\source\.NET Standard\Unimake.Business.DFe"

dotnet build --configuration Release
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Projeto principal compilado com sucesso" -ForegroundColor Green
} else {
    Write-Host "✗ Erro ao compilar projeto principal" -ForegroundColor Red
    exit 1
}

# 4. Verificar se os DTOs foram gerados
Write-Host "→ Verificando DTOs gerados..." -ForegroundColor Yellow
$objPath = "obj\Release\*\generated\Unimake.Business.DFe.SourceGenerators"

$generatedFiles = Get-ChildItem -Path $objPath -Filter "*.g.cs" -Recurse -ErrorAction SilentlyContinue

if ($generatedFiles.Count -gt 0) {
    Write-Host "✓ DTOs gerados com sucesso:" -ForegroundColor Green
    foreach ($file in $generatedFiles) {
        Write-Host "  - $($file.Name)" -ForegroundColor Cyan
    }
} else {
    Write-Host "⚠ Nenhum DTO foi gerado ainda. Adicione os atributos [GenerateDto] nas suas classes." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== Próximos Passos ===" -ForegroundColor Green
Write-Host "1. Adicione os atributos [GenerateDto] nas suas classes XML" -ForegroundColor White
Write-Host "   Exemplo: [GenerateDto(DtoType.IncluirRequest, DtoType.AtualizarRequest)]" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Para ver os DTOs gerados, compile o projeto novamente:" -ForegroundColor White
Write-Host "   dotnet build" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Os DTOs ficam em: obj\Release\*\generated\" -ForegroundColor White
Write-Host ""
Write-Host "4. Configure seu projeto de API para usar os DTOs gerados" -ForegroundColor White
Write-Host ""
Write-Host "=== Configuração Concluída ===" -ForegroundColor Green
