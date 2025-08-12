#!/usr/bin/env pwsh

Write-Host "🚀 Unimake DFe Database Generator" -ForegroundColor Green
Write-Host "================================" -ForegroundColor Green

Write-Host ""
Write-Host "Restaurando pacotes..." -ForegroundColor Yellow
dotnet restore

Write-Host ""
Write-Host "Compilando projeto..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Compilação bem-sucedida!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Executando aplicação..." -ForegroundColor Yellow
    dotnet run
} else {
    Write-Host ""
    Write-Host "❌ Erro na compilação!" -ForegroundColor Red
    Read-Host "Pressione Enter para sair"
}

Write-Host ""
Write-Host "Pressione qualquer tecla para sair..." -ForegroundColor Cyan
Read-Host
