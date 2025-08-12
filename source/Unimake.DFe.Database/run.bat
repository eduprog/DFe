@echo off
echo 🚀 Unimake DFe Database Generator - PostgreSQL
echo ==============================================

echo.
echo ✅ Verificando PostgreSQL...
echo    Certifique-se de que o PostgreSQL está rodando
echo    e o banco 'nfedatabase' foi criado.
echo.

echo 📦 Restaurando pacotes...
dotnet restore

echo.
echo 🔨 Compilando projeto...
dotnet build

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ Compilação bem-sucedida!
    echo.
    echo 🗄️  Conectando ao PostgreSQL...
    echo    Host: localhost
    echo    Database: nfedatabase
    echo    User: postgres
    echo.
    echo 🚀 Executando aplicação...
    dotnet run
) else (
    echo.
    echo ❌ Erro na compilação!
    echo.
    echo 💡 Dica: Execute setup-postgresql.bat para configurar o banco
    pause
)

echo.
echo Pressione qualquer tecla para sair...
pause > nul
