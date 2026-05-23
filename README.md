# API REST - Monitoramento Pet

## Descrição do Projeto

Este projeto foi desenvolvido para o Challenge da FIAP na disciplina Advanced Business Development with .NET.

A aplicação consiste em uma API RESTful desenvolvida com ASP.NET Core, utilizando Entity Framework Core e integração com banco de dados Oracle.

O objetivo da API é realizar o gerenciamento de pets, permitindo operações completas de CRUD (Create, Read, Update e Delete), além de consultas parametrizadas.

---

# Tecnologias Utilizadas

- ASP.NET Core
- C#
- Entity Framework Core
- Oracle Database
- Swagger / OpenAPI
- Rider IDE

---

# Estrutura do Projeto

```text
Controllers/
Data/
Models/
Program.cs
appsettings.json
README.md
```

---

# Funcionalidades

## CRUD Completo de Pets

### GET
- Listar todos os pets
- Buscar pet por ID
- Buscar pet por nome
- Buscar pet por nível de risco

### POST
- Cadastrar novo pet

### PUT
- Atualizar pet

### DELETE
- Remover pet

---

# Rotas da API

| Método | Endpoint | Descrição |
|---|---|---|
| GET | /api/pets | Lista todos os pets |
| GET | /api/pets/{id} | Busca pet por ID |
| GET | /api/pets/nome/{nome} | Busca pet por nome |
| GET | /api/pets/risco/{risco} | Busca pets por nível de risco |
| POST | /api/pets | Cadastra um novo pet |
| PUT | /api/pets/{id} | Atualiza um pet |
| DELETE | /api/pets/{id} | Remove um pet |

---

# Retornos HTTP Utilizados

| Código | Descrição |
|---|---|
| 200 | OK |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request |
| 404 | Not Found |

---

# Integração com Oracle

A aplicação utiliza Oracle Database juntamente com Entity Framework Core para persistência de dados.

### String de conexão

```json
"ConnectionStrings": {
  "OracleConnection": "User Id=rm563982;Password=051206;Data Source=oracle.fiap.com.br:1521/ORCL"
}
```

---

# Como Executar o Projeto

## Clonar o repositório

```bash
git clone https://github.com/kaique-masc/SUPERNOVAVET_DOTNET.git
```

---

## Acessar a pasta do projeto

```bash
cd Challenge_Sprint1_.NET
```

---

## Restaurar dependências

```bash
dotnet restore
```

---

## Executar aplicação

```bash
dotnet run
```

---

# Swagger / OpenAPI

Após executar a aplicação, acessar:

```text
http://localhost:5000/swagger
```

---

# Banco de Dados

O projeto utiliza Oracle Database e as tabelas desenvolvidas no Challenge:

- CH_PET
- CH_TUTOR
- CH_VACINA

---

# Desenvolvido por

- Felipe Augusto Lopes Ferreira
- Kaique Mascarenhas dos Santos

---

# Challenge FIAP 2026
