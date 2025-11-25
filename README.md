# Escalonadores

## Pré Requisitos

- Visual Studio 2022 ou Visual Studio Insiders
- .NET SDK 8
- PostgreSQL
- Git

## Instalação dos pré requisitos

- Visual Studio 2022 ou Visual Studio Insiders
 - Baixe o instalador [Visual Studio Insiders](https://visualstudio.microsoft.com/insiders/?rwnlp=pt-br) ou [Visual Studio 2022 Community](https://visualstudio.microsoft.com/pt-br/downloads/)
 - Abra o instalador e marque "Desenvolvimento em ASP.NET e Web".
 - Confira se estes componentes estão selecionados:
 - SDK do .NET 8
 - Ferramentas de desenvolvimento .NET
 - Ferramentas CLI do Entity Framework
 - Clique em Instalar e aguarde finalizar.

- PostgreSQL
 - Download [Postgresql](https://www.postgresql.org/download/)
 - Durante a instalação, defina uma senha para o usuário postgres
 - Finalize a instalação.
 - Recomendado também baixar um cliente de banco de dados, como PgAdmin ou DBeaver
 - Dentro do cliente do banco de dados ou em linha de comando no terminal acessando o SGBD, crie o banco escalonadores com o comando "create database db_escalonadores"
 - Execute o primeiro e o segundo tópico do passo a passo e em seguida ajuste o arquivo appsettings.Development.json colocando a senha do seu usuario postgres.

- Git
 - Download [GIT](https://git-scm.com/install/)
 - Realize a instalação

## Passo a passo para execução da API localmente:
- Clone o repositório para sua máquina local usando o comando "git clone https://github.com/BryanSilva-Dev/Escalonadores.git"
- Abra o projeto com o visual studio insiders ou o 2022
- No terminal do visual studio, acesse a pasta escalonadores dentro do projeto com o comando cd .\Escalonadores\
- Aplique o migrations com o comando "dotnet ef database update", caso retorne erro de ferramenta ausente, use o comando "dotnet tool install --global dotnet-ef" e depois o primeiro comamdo novamente
- Caso tenha um problema com o tópico anterior, você pode criar o banco no seu cliente de banco favorito copiando o script do arquivo db.script, e comentar as linhas 54 a 58 do Program.cs
- Para executar:
 - Abra a solução.
 - Selecione o projeto Escalonadores como startup.
 - Pressione F5 ou clique em Iniciar Depuração.
 - Ou alternativamente, pelo terminal, execute "dotnet run --project Escalonadores/Escalonadores.csproj"
- A api inicia automaticamente redirecionando para o swagger, caso não tenha o projeto visual (Escalonadores-Visual) e queira visualizar a resposta do backend apenas, poderá visualizar a resposta no mesmo por esta tela, ou utilizando um cliente para consumo da API como o Postman