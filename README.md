# Teste Desenvolvedor Spacecaps

## Neste projeto foi desenvolvido três camadas de serviços e uma biblioteca compartilhada: 
- O serviço de api "TLA.Identity.Api" terá a responsabilidade de gerenciar os dados pessoais, permissões e acesso do usuário ao sistema.
- O serviço de api "TLA.Tasks.Api" terá a responsabilidade de gerenciar as Tarefas dos usuários.
- A biblioteca "TLA.WebApi.Core" terá a responsabilidade de compartilhar recursos comuns aos serviços anteriores.
- O serviço de teste "TLA.Tasks.Tests" terá a responsabilidade de testar o sistema.

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




