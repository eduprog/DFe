# 🚀 Unimake DFe Database - PostgreSQL Setup
# ==========================================

Write-Host "🐘 Configurando PostgreSQL para Unimake DFe Database" -ForegroundColor Green
Write-Host "====================================================" -ForegroundColor Green

Write-Host ""
Write-Host "🔍 Verificando se o PostgreSQL está instalado..." -ForegroundColor Yellow

try {
    $pgVersion = & psql --version 2>$null
    if ($pgVersion) {
        Write-Host "✅ PostgreSQL encontrado: $pgVersion" -ForegroundColor Green
        
        Write-Host ""
        Write-Host "🗄️  Para criar o banco de dados 'nfedatabase'..." -ForegroundColor Yellow
        Write-Host "Execute este comando no psql:" -ForegroundColor Cyan
        Write-Host "CREATE DATABASE nfedatabase;" -ForegroundColor White
        Write-Host ""
        
        Write-Host "🚀 Para executar a aplicação:" -ForegroundColor Yellow
        Write-Host "dotnet run" -ForegroundColor White
        
    } else {
        throw "PostgreSQL não encontrado"
    }
}
catch {
    Write-Host "❌ PostgreSQL não está instalado ou não está no PATH" -ForegroundColor Red
    Write-Host ""
    Write-Host "📥 Instale o PostgreSQL usando uma dessas opções:" -ForegroundColor Yellow
    Write-Host "1. winget install PostgreSQL.PostgreSQL" -ForegroundColor White
    Write-Host "2. choco install postgresql" -ForegroundColor White
    Write-Host "3. Download: https://www.postgresql.org/download/windows/" -ForegroundColor White
}

Write-Host ""
Write-Host "📖 Para mais informações, consulte o README.md" -ForegroundColor Cyan

Read-Host "Pressione Enter para continuar"
