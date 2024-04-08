# Teste Desenvolvedor Spacecaps

## Neste projeto foi desenvolvido dois serviços (apis) e uma biblioteca compartilhada: 
- O serviço "TLA.Identity.Api" terá a responsabilidade de gerenciar os dados pessoais, permissões e acesso do usuário ao sistema.
- O serviço "TLA.Tasks.Api" terá a responsabilidade de gerenciar as Tarefas dos usuários.
- A biblioteca "TLA.WebApi.Core" terá a responsabilidade de compartilhar recursos comuns aos serviços anteriores.

## No projeto geral utilizei:
- Framework: net5.0
- Arquitetura: Onion Architecture/Clean Architecture
- Padrão: CQRS
- Validação: FluentValidation 
- Banco de dados: MySql
- ORM: Entity Framework
- Documentação: Swagger

## Para rodar o projeto deve:
- Rodar as migrations dentro do diretório de cada um dos serviços "TLA.Identity.Api" e "TLA.Tasks.Api" com o seguinte comando: "dotnet ef database update -v"
- Dar start em modo "multiple startup projects" nos dois serviços "TLA.Identity.Api" e "TLA.Tasks.Api", que automaticamente irá abrir as duas apis na página do swagger ui




