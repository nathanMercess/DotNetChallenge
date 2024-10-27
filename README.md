
# Desafio Técnico DotNet

Este projeto implementa uma API RestFul e interface em ASP.NET com as funcionalidades solicitadas.

## Tecnologias

- **Back-End**: .NET 8, Dapper, SQL Server
- **Front-End**: ASP.NET com Razor ou Blazor

## Funcionalidades

- **API RestFul** com CRUD de `Aluno`, `Turma` e associações.
- **Validação de Payloads** e **Testes Unitários** com XUnit.
- **Banco de Dados**: Estrutura relacional conforme o diagrama.

### Telas

1. **Aluno**: Cadastro, edição, lista.
2. **Turma**: Cadastro, edição, lista e inativação.
3. **Relacionar Turmas**: Associação de Aluno e Turma, com lista e inativação de relacionamento.

## Regras de Negócio

1. Nomes únicos para `Turma`.
2. Aluno não pode estar na mesma turma duas vezes.
3. Senhas fortes e hash para armazenamento.

## Execução

1. Clone o repositório e configure o banco de dados.
2. Defina a string de conexão no app.
3. Execute com Visual Studio ou CLI do .NET.
