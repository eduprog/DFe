@echo off
echo 🐘 PostgreSQL Setup para Unimake DFe Database
echo =============================================

echo.
echo Este script ajuda a configurar o PostgreSQL para o projeto.
echo.

echo 📥 Opções de Instalação do PostgreSQL:
echo.
echo 1. Winget (Recomendado):
echo    winget install PostgreSQL.PostgreSQL
echo.
echo 2. Chocolatey:
echo    choco install postgresql
echo.
echo 3. Download Manual:
echo    https://www.postgresql.org/download/windows/
echo.

echo 🛠️  Após a instalação, execute os comandos:
echo.
echo    # Conectar ao PostgreSQL
echo    psql -U postgres
echo.
echo    # Criar o banco de dados
echo    CREATE DATABASE nfedatabase;
echo.
echo    # Sair do psql
echo    \q
echo.

echo 🚀 Para testar a aplicação:
echo    cd "d:\trab\estudos\dotnet\DFe\source\Unimake.DFe.Database"
echo    dotnet run
echo.

echo 📖 Documentação completa: README.md
echo.

pause
