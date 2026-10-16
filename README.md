# 🐾 PetCore – Backend ASP.NET Core
## Challenger - 2026

## 🗣️ Dev do projeto
#### NOME: Emanuelly Ventura Do Nascimento

RM562339 - 2TDSPJ

#### NOME: Carolina Nascimento Gonçalves

RM564786 - 2TDSPJ

#### NOME: Julia Sayuri Kina

RM564555 - 2TDSPJ

## 📌 Descrição do Projeto

O **PetCore** é uma API REST desenvolvida em C# com ASP.NET Core para gerenciamento de informações veterinárias de pets, tutores, médicos, clínicas, prontuários, receitas, exames, medicamentos, históricos e relatórios.

O projeto foi organizado em camadas, separando responsabilidades entre API, Application, Domain e Infrastructure. A persistência dos dados é feita com Entity Framework Core e MySQL, utilizando migrations para criação e versionamento da estrutura do banco de dados.

## Solução Proposta

A solução permite cadastrar e gerenciar os principais dados de uma clínica veterinária, oferecendo endpoints para criação, consulta, atualização e remoção de recursos.

O sistema contempla relacionamentos entre entidades, como:

- Tutor e Pet
- Pet e Histórico
- Histórico e Prontuário
- Prontuário, Exames e Receitas
- Receita e Medicamentos
- Médico e Prontuários, Exames, Receitas e Relatórios
- Clínica e Relatórios

## 🛠️ Funcionalidades

- Cadastro, listagem, busca, atualização e remoção de tutores
- Cadastro, listagem, busca, atualização e remoção de médicos
- Cadastro, listagem, busca, atualização e remoção de pets
- Cadastro e gerenciamento de clínicas
- Cadastro e gerenciamento de endereços
- Cadastro e gerenciamento de exames
- Cadastro e gerenciamento de prontuários
- Cadastro e gerenciamento de receitas
- Cadastro e gerenciamento de medicamentos
- Cadastro e gerenciamento de históricos
- Cadastro e gerenciamento de relatórios
- Login por email e senha para tutor
- Login por email e senha para médico
- Documentação via Swagger
- Migrations com Entity Framework Core

## 🛠️ Tecnologias Utilizadas

- C#
- ASP.NET Core
- Entity Framework Core
- MySQL
- Pomelo.EntityFrameworkCore.MySql
- Swagger / Swashbuckle
- Arquitetura em camadas
- Fluent API para mapeamento das entidades
- Serilog para logs estruturados
- OpenTelemetry e Prometheus para tracing e métricas
- xUnit, Moq e WebApplicationFactory para testes automatizados

## 📈 Monitoramento e Observabilidade

### Health checks

- `GET /health/live`: confirma que a API está em execução, sem dependências externas.
- `GET /health/ready`: valida a conectividade do `PetCoreContext` com o banco MySQL e o serviço externo configurado.

O projeto atualmente utiliza **MySQL**, portanto o health check de dados acompanha a tecnologia existente; não foram introduzidos Oracle ou MongoDB. Para verificar um serviço externo, informe a URL no campo `ExternalServices:BaseUrl` de `PetCore.Api/appsettings.json`.

### Logs, correlação e rastreabilidade

O Serilog escreve logs estruturados nos níveis Information, Warning e Error tanto no console como em `PetCore.Api/logs/`, com retenção de 14 dias. Toda requisição recebe/devolve o cabeçalho `X-Correlation-ID`; envie-o na chamada para correlacionar os eventos de ponta a ponta.

O OpenTelemetry instrumenta automaticamente as requisições ASP.NET Core e disponibiliza tracing distribuído entre as camadas. As métricas incluem duração de requisições e contador de erros HTTP 5xx, expostos em formato Prometheus em:

```text
GET /metrics
```

## ✅ Testes automatizados

Os testes estão organizados por responsabilidade e seguem explicitamente o padrão **AAA** (Arrange, Act, Assert):

- `PetCore.Tests.Unit`: testes de Domínio e Application com xUnit e Moq.
- `PetCore.Tests.Integration`: testes HTTP com `WebApplicationFactory`, `CollectionFixture` compartilhada e banco EF Core InMemory, sem exigir MySQL local.

Execute todos os testes na pasta `PetCore`:

```bash
dotnet test
```

> A API atual não implementa middleware, esquema ou endpoints de autenticação. Para não alterar a lógica existente, os testes de integração validam os fluxos HTTP disponíveis (sucesso do health check e erro 404). Quando a autenticação for adicionada ao projeto, a mesma `PetCoreApiFactory` deverá ser usada para cobrir credenciais válidas e inválidas.

## 📂 Estrutura do Projeto

```txt
PetCore
├── PetCore.Api
│   ├── Controllers
│   ├── Extensions
│   ├── Exceptions
│   └── Program.cs
│
├── PetCore.Application
│   ├── DTOs
│   ├── Interfaces
│   └── Services
│
├── PetCore.Domain
│   ├── Commons
│   ├── Entities
│   └── Enums
│
└── PetCore.Infrastructure
│   ├── Persistence
│   ├── Configurations
│   │   └── PetCoreContext.cs
│   ├── Repositories
│   └── Migrations
│
├── PetCore.Tests.Integration
│   ├── Api.Collection.cs
│   ├── HealthEndpointsTests.cs
│   └── PetCoreApiFactory.cs
│
└── PetCore.Infrastructure
│   ├── Application
│   │   └── ClinicaServiceTests.cs
│   ├── Domain
│   │   └── ClinicaTests.cs

```

## Documentação das Rotas

### Clínica

GET /api/Clinica

GET /api/Clinica/{id}

POST /api/Clinica

PUT /api/Clinica/{id}/patch

DELETE /api/Clinica/{id}

---

### Endereço

GET /api/Endereco

GET /api/Endereco/{id}

POST /api/Endereco

PUT /api/Endereco/{id}/patch

DELETE /api/Endereco/{id}

---

### Exame

GET /api/Exame

GET /api/Exame/{id}

POST /api/Exame

PUT /api/Exame/{id}/patch

DELETE /api/Exame/{id}

---

### Histórico

GET /api/Historico

GET /api/Historico/{id}

POST /api/Historico

DELETE /api/Historico/{id}

---

### Medicamento

GET /api/Medicamento

GET /api/Medicamento/{id}

POST /api/Medicamento

PUT /api/Medicamento/{id}/patch

DELETE /api/Medicamento/{id}

---

### Médico

GET /api/Medico

GET /api/Medico/{id}

GET /api/Medico/login?email={email}&senha={senha}

POST /api/Medico

PUT /api/Medico/{id}/patch

DELETE /api/Medico/{id}

---

## Pet

GET /api/Pet

GET /api/Pet/menu

GET /api/Pet/{id}

POST /api/Pet

PUT /api/Pet/{id}/patch

DELETE /api/Pet/{id}

---

### Prontuário

GET /api/Prontuario

GET /api/Prontuario/{id}

POST /api/Prontuario

PUT /api/Prontuario/{id}/patch

DELETE /api/Prontuario/{id}

---

### Receita

GET /api/Receita

GET /api/Receita/{id}

POST /api/Receita

DELETE /api/Receita/{id}

---

### Relatório

GET /api/Relatorio

GET /api/Relatorio/{id}

POST /api/Relatorio

PUT /api/Relatorio/{id}/patch

DELETE /api/Relatorio/{id}

---

## Tutor

GET /api/Tutor

GET /api/Tutor/{id}

GET /api/Tutor/login?email={email}&senha={senha}

POST /api/Tutor

PUT /api/Tutor/{id}/patch

DELETE /api/Tutor/{id}

---

## Protocolo

GET /api/Protocolo

GET /api/Protocolo/{id}

POST /api/Protocolo

PUT /api/Protocolo/{id}/patch

DELETE /api/Protocolo/{id}

---

## 🌐 Retornos HTTP

200 OK - Requisição realizada com sucesso

201 Created - Recurso criado com sucesso

204 NoContent - Recurso removido com sucesso

400 BadRequest - Dados inválidos enviados na requisição

404 NotFound - Recurso não encontrado

500 InternalServerError - Erro interno no servidor

--- 

## 🚀 Como Executar o Projeto

Siga os passos abaixo para configurar e executar a API PetCore localmente.

### 1. Pré-requisitos

Antes de iniciar, verifique se você possui instalado:

- .NET SDK
- MySQL Server
- Visual Studio, Rider ou VS Code
- Ferramenta do Entity Framework Core

Para verificar se o .NET está instalado:

```bash
dotnet --version

```

### 2. Acessar a Pasta Raiz da Solução

```bash
cd PetCore
```

---

### 3. Restaurar os Pacotes

Na pasta raiz da solução, execute o comando abaixo para restaurar todas as dependências do projeto:

```bash
dotnet restore
```

---

### 4. Configurar o Banco de Dados

No arquivo:

```bash
PetCore.Api/appsettings.json
```

configure a connection string do MySQL.

#### Exemplo com senha:

```json
{
  "ConnectionStrings": {
    "PetCoreMySql": "server=127.0.0.1;port=3306;database=petcore;user=root;password=sua_senha"
  }
}
```

#### Exemplo sem senha:

```json
{
  "ConnectionStrings": {
    "PetCoreMySql": "server=127.0.0.1;port=3306;database=petcore;user=root;password="
  }
}
```

---

### 5. Aplicar as Migrations

Com o MySQL em execução, aplique as migrations para criar as tabelas do banco de dados:

```bash
dotnet ef database update --project PetCore.Infrastructure --startup-project PetCore.Api --context PetCoreContext
```

#### Alternativa com Script SQL

Caso não seja possível conectar ao banco no momento, o projeto possui um script SQL gerado a partir da migration:

```bash
petcore-migration.sql
```

Esse script pode ser executado manualmente no MySQL.

---

### 6. Compilar o Projeto

Compile a solução para verificar se tudo está funcionando corretamente:

```bash
dotnet build
```

---

### 7. Executar a API

Execute a aplicação com o comando:

```bash
dotnet run --project PetCore.Api
```

---

### 8. Acessar o Swagger

Após iniciar a aplicação, o terminal exibirá a porta em que a API está rodando.

Acesse a documentação Swagger pelo navegador:

```bash
https://localhost:<porta>/swagger
```

ou

```bash
http://localhost:<porta>/swagger
```

#### Exemplo:

```bash
https://localhost:5001/swagger

```
---

# 🚀Link do Deploy
https://petcore-net.onrender.com


---
