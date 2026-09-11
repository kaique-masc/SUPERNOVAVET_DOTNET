#  SUPERNOVAVET API

API REST desenvolvida em **ASP.NET Core 8** para o projeto **SUPERNOVAVET**, uma solução voltada ao acompanhamento preventivo da saúde de animais de estimação.

A aplicação permite o gerenciamento de informações relacionadas aos pets e foi evoluída para possuir recursos de **monitoramento, observabilidade, autenticação e testes automatizados**.

Nesta versão foram implementados Health Checks, logging estruturado com Serilog, Correlation ID, tracing e métricas com OpenTelemetry, autenticação por API Key, testes unitários com xUnit e Moq e testes de integração utilizando WebApplicationFactory.

---

#  Objetivo do Projeto

O SUPERNOVAVET tem como objetivo auxiliar no acompanhamento preventivo da saúde dos pets.

A solução foi projetada para permitir o armazenamento e gerenciamento de informações importantes dos animais, possibilitando a evolução futura para recursos relacionados a:

- cadastro de pets;
- acompanhamento de consultas;
- controle de vacinas;
- medicamentos;
- lembretes;
- acompanhamento preventivo;
- classificação de nível de risco.

A API foi desenvolvida seguindo os princípios de uma API REST e utiliza banco de dados Oracle através do Entity Framework Core.

---

#  Integrantes

- **Kaique Mascarenhas dos Santos**
- **Felipe Augusto Lopes Ferreira**

---

#  Tecnologias Utilizadas

O projeto utiliza as seguintes tecnologias:

- .NET 8
- ASP.NET Core Web API
- C#
- Entity Framework Core
- Oracle Database
- Oracle Entity Framework Core
- Swagger / OpenAPI
- Health Checks
- Serilog
- OpenTelemetry
- xUnit
- Moq
- WebApplicationFactory
- Git
- GitHub
- JetBrains Rider

---

#  Estrutura do Projeto

A solução está organizada em três projetos principais:

```text
SUPERNOVAVET_DOTNET
│
├── Challenge_Sprint1_.NET
│   ├── Authentication
│   │   └── ApiKeyAuthenticationHandler.cs
│   │
│   ├── Controllers
│   │   ├── PetsController.cs
│   │   └── ProtectedController.cs
│   │
│   ├── Data
│   │   └── AppDbContext.cs
│   │
│   ├── HealthChecks
│   │   └── ExternalServiceHealthCheck.cs
│   │
│   ├── Models
│   │   └── Pet.cs
│   │
│   ├── Observability
│   │   └── ApiMetrics.cs
│   │
│   ├── Services
│   │   ├── IPetService.cs
│   │   └── PetService.cs
│   │
│   ├── logs
│   ├── appsettings.json
│   ├── Program.cs
│   └── README.md
│
├── Tests.Unit
│   ├── PetTests.cs
│   ├── PetServiceTests.cs
│   └── Tests.Unit.csproj
│
├── Tests.Integration
│   ├── ApiFactory.cs
│   ├── AuthenticationTests.cs
│   ├── HealthCheckTests.cs
│   ├── IntegrationTestCollection.cs
│   └── Tests.Integration.csproj
│
└── SUPERNOVAVET_DOTNET.sln
```

Essa separação permite manter a aplicação principal, os testes unitários e os testes de integração organizados de forma independente.

---

#  Banco de Dados

A aplicação utiliza **Oracle Database**.

A comunicação entre a API e o Oracle é realizada utilizando:

- Entity Framework Core;
- Oracle Entity Framework Core;
- `DbContext`;
- configuração de Connection String.

O contexto principal da aplicação é:

```text
AppDbContext
```

O `DbContext` possui acesso às entidades utilizadas pela API, incluindo os dados dos pets.

Exemplo da configuração utilizada no `Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(
        builder.Configuration.GetConnectionString("OracleConnection")
    )
);
```

---

#  Configuração da Connection String

A conexão com o Oracle deve ser configurada no arquivo:

```text
appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "SUA_CONNECTION_STRING"
  }
}
```

>  Não publique usuário, senha ou outras credenciais reais do banco de dados em um repositório público.

Para projetos públicos, recomenda-se utilizar variáveis de ambiente, User Secrets ou outra solução segura para armazenamento de credenciais.

---

#  API REST

A aplicação utiliza Controllers do ASP.NET Core para disponibilizar seus recursos através de endpoints HTTP.

A API trabalha com operações REST e utiliza códigos HTTP adequados para representar o resultado das operações.

Entre os códigos utilizados estão:

| Código | Significado |
|---|---|
| `200 OK` | Requisição executada com sucesso |
| `201 Created` | Recurso criado com sucesso |
| `204 No Content` | Operação concluída sem conteúdo de retorno |
| `400 Bad Request` | Dados enviados são inválidos |
| `401 Unauthorized` | Requisição não autenticada |
| `404 Not Found` | Recurso ou rota não encontrado |
| `500 Internal Server Error` | Erro interno da aplicação |

---

#  Swagger / OpenAPI

A aplicação possui documentação interativa utilizando Swagger.

O Swagger permite:

- visualizar os endpoints disponíveis;
- analisar os parâmetros;
- visualizar os modelos;
- executar requisições diretamente pelo navegador;
- verificar os códigos HTTP retornados pela API.

Ao iniciar a aplicação, acesse:

```text
/swagger
```

A configuração está presente no `Program.cs`:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

e:

```csharp
app.UseSwagger();
app.UseSwaggerUI();
```

---

#  Health Checks

A aplicação utiliza **ASP.NET Core Health Checks** para monitorar sua disponibilidade e suas principais dependências.

O endpoint disponível é:

```http
GET /health
```

Quando todos os componentes necessários estão funcionando corretamente, o endpoint retorna:

```text
Healthy
```

com status:

```text
200 OK
```

---

## Health Check da API

O endpoint `/health` permite verificar se a aplicação está disponível e respondendo corretamente.

Exemplo:

```http
GET /health
```

Resposta esperada:

```text
Healthy
```

---

## Health Check do Oracle

A conectividade com o Oracle é verificada através do `AppDbContext`.

Configuração:

```csharp
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(
        name: "oracle_database",
        tags: new[] { "database" }
    );
```

Dessa forma, o sistema consegue verificar a disponibilidade da conexão utilizada pelo Entity Framework Core.

---

## Health Check de Serviço Externo

Também foi criado um Health Check personalizado para verificar a disponibilidade de um serviço HTTP externo.

Classe utilizada:

```text
ExternalServiceHealthCheck
```

O serviço utiliza `HttpClient` para realizar a verificação.

Registro:

```csharp
builder.Services.AddHttpClient<ExternalServiceHealthCheck>();
```

Health Check:

```csharp
.AddCheck<ExternalServiceHealthCheck>(
    "external_service",
    tags: new[] { "external" }
);
```

O componente pode retornar:

```text
Healthy
Degraded
Unhealthy
```

dependendo da disponibilidade do serviço consultado.

---

#  Logging Estruturado com Serilog

A aplicação utiliza **Serilog** para geração de logs estruturados.

Foram implementados os níveis:

```text
Information
Warning
Error
```

Os logs são enviados para dois destinos:

- console da aplicação;
- arquivos locais.

Configuração:

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();
```

Os arquivos são gerados no diretório:

```text
logs/
```

com rotação diária.

---

#  Information

Requisições executadas corretamente são registradas como `Information`.

Exemplo:

```text
Requisição iniciada: GET /health
Requisição finalizada com sucesso. StatusCode: 200
```

---

#  Warning

Respostas HTTP entre `400` e `499` são registradas como `Warning`.

Por exemplo, uma rota inexistente:

```http
GET /rota-inexistente
```

gera:

```text
StatusCode: 404
```

e um log de nível Warning.

---

#  Error

Erros de servidor e exceções inesperadas são registrados utilizando o nível `Error`.

O middleware também possui tratamento para exceções:

```csharp
catch (Exception ex)
{
    Log.Error(
        ex,
        "Erro inesperado durante a requisição."
    );

    throw;
}
```

---

#  Correlation ID

Foi implementado um **Correlation ID** para identificar e acompanhar cada requisição processada pela API.

A aplicação verifica o header:

```text
X-Correlation-ID
```

Caso o cliente não envie um identificador, a própria API cria um novo utilizando:

```csharp
Guid.NewGuid().ToString();
```

O mesmo identificador é retornado no header da resposta:

```text
X-Correlation-ID
```

e incluído no contexto do Serilog.

Isso facilita a identificação dos logs pertencentes à mesma requisição.

---

#  OpenTelemetry

A aplicação utiliza **OpenTelemetry** para implementação de observabilidade.

Foram configurados:

- tracing de requisições ASP.NET Core;
- tracing das chamadas realizadas com HttpClient;
- métricas do ASP.NET Core;
- métricas do HttpClient;
- métricas personalizadas da aplicação;
- exportação para o console.

---

#  Distributed Tracing

O tracing permite acompanhar a execução das requisições.

Configuração:

```csharp
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    });
```

Durante a execução podem ser observados dados como:

```text
TraceId
SpanId
Activity
Duration
```

Isso permite acompanhar requisições e chamadas realizadas entre componentes da aplicação.

---

#  Métricas

Além das métricas automáticas fornecidas pelo OpenTelemetry, foram criadas métricas específicas da API.

A classe responsável é:

```text
ApiMetrics
```

O Meter utilizado é:

```text
SuperNovaVet.Api
```

Foram implementadas três métricas principais.

### Quantidade de requisições

```text
api_requests_total
```

Registra a quantidade de requisições processadas pela API.

### Quantidade de erros

```text
api_errors_total
```

Registra respostas HTTP com status de erro.

### Tempo de resposta

```text
api_request_duration_ms
```

Registra o tempo de processamento das requisições em milissegundos.

A duração é calculada utilizando:

```csharp
Stopwatch
```

As métricas são coletadas pelo OpenTelemetry e, na configuração atual, exportadas para o console da aplicação através do Console Exporter.

---

#  Autenticação por API Key

A aplicação possui uma autenticação simples utilizando **API Key**.

A implementação está localizada em:

```text
Authentication/ApiKeyAuthenticationHandler.cs
```

O esquema utilizado é:

```text
ApiKey
```

A chave é enviada através do header:

```text
X-API-KEY
```

Exemplo:

```http
X-API-KEY: SUPERNOVAVET-123
```

A configuração utilizada para desenvolvimento pode ser definida no `appsettings.json`:

```json
{
  "ApiKey": "SUPERNOVAVET-123"
}
```

> Para ambientes reais, a API Key não deve ser armazenada diretamente em um repositório público.

---

#  Endpoint Protegido

Foi criado um endpoint protegido para validação do fluxo de autenticação.

Endpoint:

```http
GET /api/protected
```

O Controller utiliza:

```csharp
[Authorize]
```

Sem a API Key:

```text
401 Unauthorized
```

Com uma API Key válida:

```text
200 OK
```

Resposta:

```json
{
  "mensagem": "Acesso autorizado."
}
```

---

#  Testes Automatizados

A solução possui testes automatizados divididos em:

```text
Tests.Unit
Tests.Integration
```

Os testes utilizam:

- xUnit;
- Moq;
- WebApplicationFactory;
- padrão AAA;
- Fixtures;
- Collection Fixtures.

Todos os testes implementados atualmente foram executados com sucesso.

---

#  Padrão AAA

Os testes seguem o padrão:

```text
Arrange
Act
Assert
```

### Arrange

Prepara os objetos, dados e dependências necessários para o teste.

### Act

Executa a operação que será testada.

### Assert

Verifica se o resultado obtido corresponde ao comportamento esperado.

Exemplo:

```csharp
[Fact]
public void AlterarNivelRisco_NovoNivel_DeveAtualizarNivel()
{
    // Arrange
    var pet = new Pet
    {
        Nome = "Luna",
        Especie = "Gato",
        Idade = 3,
        NivelRisco = "Baixo"
    };

    // Act
    pet.NivelRisco = "Medio";

    // Assert
    Assert.Equal("Medio", pet.NivelRisco);
}
```

---

#  Testes Unitários

Os testes unitários estão no projeto:

```text
Tests.Unit
```

Foram implementados testes para o domínio e serviço relacionado aos pets.

Os testes atuais verificam:

```text
CriarPet_DadosValidos_DeveCriarPetCorretamente
AlterarNivelRisco_NovoNivel_DeveAtualizarNivel
ObterPetPorId_IdValido_DeveRetornarPet
```

Resultado atual:

```text
Com falha: 0
Aprovado: 3
Total: 3
```

---

#  Mock com Moq

O projeto utiliza **Moq** para criação de objetos simulados durante os testes unitários.

Foi criada a interface:

```text
IPetService
```

e sua implementação:

```text
PetService
```

Nos testes, o comportamento do serviço pode ser simulado utilizando:

```csharp
var mockService = new Mock<IPetService>();
```

Exemplo:

```csharp
mockService
    .Setup(service => service.ObterPetPorId(1))
    .Returns(petEsperado);
```

Também é realizada a verificação da chamada:

```csharp
mockService.Verify(
    service => service.ObterPetPorId(1),
    Times.Once
);
```

---

#  Testes de Integração

Os testes de integração estão no projeto:

```text
Tests.Integration
```

Eles utilizam:

```text
Microsoft.AspNetCore.Mvc.Testing
```

através de:

```csharp
WebApplicationFactory<Program>
```

A classe:

```text
ApiFactory
```

é responsável por inicializar a aplicação durante os testes.

---

#  Fixtures e Collection Fixtures

Os testes de integração utilizam uma Collection Fixture do xUnit.

A configuração é realizada através de:

```text
IntegrationTestCollection
```

Isso permite compartilhar a `ApiFactory` entre os testes de integração.

Exemplo:

```csharp
[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection
    : ICollectionFixture<ApiFactory>
{
}
```

---

#  Fluxos Testados na Integração

Atualmente são executados quatro testes de integração.

### Health Check

```text
HealthCheck_ApiDisponivel_DeveRetornar200
```

Valida:

```text
GET /health
→ 200 OK
```

### Rota inexistente

```text
RotaInexistente_DeveRetornar404
```

Valida:

```text
GET /rota-inexistente
→ 404 Not Found
```

### Requisição sem autenticação

```text
RotaProtegida_SemApiKey_DeveRetornar401
```

Valida:

```text
GET /api/protected
SEM X-API-KEY
→ 401 Unauthorized
```

### Requisição autenticada

```text
RotaProtegida_ComApiKeyValida_DeveRetornar200
```

Valida:

```text
GET /api/protected
X-API-KEY: SUPERNOVAVET-123
→ 200 OK
```

Resultado atual:

```text
Com falha: 0
Aprovado: 4
Total: 4
```

---

#  Resultado Geral dos Testes

Atualmente a solução possui:

| Projeto | Testes | Aprovados | Falhas |
|---|---:|---:|---:|
| Tests.Unit | 3 | 3 | 0 |
| Tests.Integration | 4 | 4 | 0 |
| **Total** | **7** | **7** | **0** |

Todos os testes automatizados implementados estão passando.

---

#  Como Executar o Projeto

## 1. Clonar o repositório

```bash
git clone https://github.com/kaique-masc/SUPERNOVAVET_DOTNET.git
```

Entre na pasta:

```bash
cd SUPERNOVAVET_DOTNET
```

---

## 2. Restaurar as dependências

Como a pasta contém a solução e diferentes projetos, utilize:

```bash
dotnet restore SUPERNOVAVET_DOTNET.sln
```

---

## 3. Configurar o Oracle

Configure sua Connection String no `appsettings.json` ou utilizando uma forma segura de configuração.

Exemplo:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "SUA_CONNECTION_STRING"
  }
}
```

---

## 4. Configurar a API Key

Para desenvolvimento:

```json
{
  "ApiKey": "SUPERNOVAVET-123"
}
```

---

## 5. Compilar a solução

```bash
dotnet build SUPERNOVAVET_DOTNET.sln
```

O resultado esperado é:

```text
Compilação com êxito.
0 Erro(s)
```

---

## 6. Executar a API

Execute especificamente o projeto principal:

```bash
dotnet run --project Challenge_Sprint1_.NET.csproj
```

Após iniciar, o endereço será informado no terminal.

---

#  Como Executar os Testes

## Executar todos os testes

Na raiz do repositório:

```bash
dotnet test SUPERNOVAVET_DOTNET.sln
```

Esse comando executará tanto os testes unitários quanto os testes de integração.

Resultado validado durante o desenvolvimento:

```text
Tests.Unit
Com falha: 0
Aprovado: 3
Total: 3

Tests.Integration
Com falha: 0
Aprovado: 4
Total: 4
```

Total:

```text
7 testes aprovados
0 testes com falha
```

---

## Executar somente os testes unitários

```bash
dotnet test Tests.Unit/Tests.Unit.csproj
```

---

## Executar somente os testes de integração

```bash
dotnet test Tests.Integration/Tests.Integration.csproj
```

---

#  Como Testar o Health Check

Com a aplicação executando:

```http
GET /health
```

Resultado esperado quando as dependências verificadas estão disponíveis:

```text
Healthy
```

---

#  Como Testar a Autenticação

Para acessar a rota protegida:

```http
GET /api/protected
```

adicione o header:

```text
X-API-KEY: SUPERNOVAVET-123
```

Com a chave correta:

```text
200 OK
```

Sem a chave:

```text
401 Unauthorized
```

---

#  Como Acompanhar os Logs

Durante a execução da aplicação, os logs são apresentados diretamente no console.

Também são armazenados em:

```text
logs/
```

Exemplo:

```text
logs/log-20260911.txt
```

Os logs permitem acompanhar:

- início das requisições;
- método HTTP;
- rota acessada;
- status HTTP;
- requisições bem-sucedidas;
- warnings;
- erros;
- Correlation ID.

---

#  Como Acompanhar Traces e Métricas

O OpenTelemetry está configurado com Console Exporter.

Ao executar a aplicação e realizar requisições, o terminal apresenta informações de tracing e métricas.

Entre as informações de tracing estão:

```text
TraceId
SpanId
Duration
```

Entre as métricas personalizadas estão:

```text
api_requests_total
api_errors_total
api_request_duration_ms
```

Também são coletadas métricas automáticas da instrumentação ASP.NET Core e HttpClient.

---

#  Recursos de Monitoramento Implementados

A Sprint adicionou os seguintes recursos de monitoramento e observabilidade:

```text
Health Check da API
Health Check do Oracle
Health Check de serviço externo
Serilog
Logs Information
Logs Warning
Logs Error
Logs no console
Logs em arquivo
Correlation ID
OpenTelemetry
Tracing ASP.NET Core
Tracing HttpClient
Métricas ASP.NET Core
Métricas HttpClient
Contador de requisições
Contador de erros
Tempo de resposta
```

---

#  Recursos de Qualidade Implementados

A aplicação também conta com:

```text
xUnit
Moq
Padrão AAA
Testes unitários
Testes de integração
WebApplicationFactory
Fixtures
Collection Fixtures
Teste HTTP 200
Teste HTTP 404
Teste HTTP 401
Teste de autenticação
Separação entre Tests.Unit e Tests.Integration
```

---

#  Segurança

O projeto possui autenticação por API Key para demonstrar o controle de acesso a endpoints protegidos.

Credenciais sensíveis, como:

```text
Senha do Oracle
Connection Strings com credenciais
API Keys de produção
Tokens
```

não devem ser publicadas em repositórios públicos.

Para ambientes de produção, recomenda-se utilizar mecanismos seguros de gerenciamento de segredos.

---

#  Evoluções Implementadas

Nesta etapa do projeto foram adicionados:

- Health Checks;
- monitoramento da conexão Oracle;
- monitoramento de serviço externo;
- logging estruturado;
- armazenamento de logs em arquivo;
- níveis Information, Warning e Error;
- Correlation ID;
- OpenTelemetry;
- distributed tracing;
- métricas de requisições;
- métricas de erros;
- métricas de tempo de resposta;
- autenticação por API Key;
- endpoint protegido;
- testes unitários;
- Moq;
- padrão AAA;
- testes de integração;
- WebApplicationFactory;
- Fixtures e Collection Fixtures;
- testes de autenticação;
- organização dos projetos de teste.

---

#  Pacotes Principais

Entre os principais pacotes utilizados estão:

```text
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Tools
Oracle.EntityFrameworkCore
Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
Serilog.AspNetCore
Serilog.Sinks.File
OpenTelemetry.Extensions.Hosting
OpenTelemetry.Instrumentation.AspNetCore
OpenTelemetry.Instrumentation.Http
OpenTelemetry.Exporter.Console
Microsoft.AspNetCore.Mvc.Testing
Microsoft.NET.Test.Sdk
xunit
xunit.runner.visualstudio
Moq
coverlet.collector
```

---

#  Status Atual

A aplicação possui os principais recursos necessários para monitoramento, observabilidade e testes automatizados.

Estado validado:

```text
API ASP.NET Core: OK
Oracle / EF Core: configurado
Health Checks: OK
Serilog: OK
Correlation ID: OK
OpenTelemetry: OK
Tracing: OK
Métricas: OK
Autenticação: OK
Testes Unitários: 3/3 aprovados
Testes de Integração: 4/4 aprovados
Total de Testes: 7/7 aprovados
```

---

#  Desenvolvedores

**Kaique Mascarenhas dos Santos**  
**Felipe Augusto Lopes Ferreira**

Projeto desenvolvido como parte da evolução do **SUPERNOVAVET**, utilizando ASP.NET Core, Oracle, monitoramento, observabilidade e testes automatizados.
