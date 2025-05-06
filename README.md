# **DevXpertHub - Gestão de Mini Loja Virtual com Cadastro de Produtos e Categorias**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **DevXpertHub**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Introdução ao Desenvolvimento ASP.NET Core**.
O objetivo principal desenvolver uma aplicação web básica usando conceitos do Módulo 1 (C#, ASP.NET Core MVC, SQL, EF Core, APIs REST) para gestão simplificada de produtos e categorias em um formato tipo e-commerce marketplace.

### **Autor(es)**
- **Roberio Pinto Souza**

## **2. Proposta do Projeto**

O projeto consiste em:

- **Aplicação MVC:** Interface web para usuários interagirem com o catálogo de produtos e categorias. Isso inclui funcionalidades como listagem, criação, edição e exclusão de produtos e categorias.
- **API RESTful:** Exposição dos recursos de produtos e categorias através de endpoints RESTful. Isso permite que outras aplicações (como um front-end React/Angular ou aplicativos móveis) interajam com os dados. As funcionalidades incluem operações CRUD (Create, Read, Update, Delete) para produtos e categorias.
- **Gerenciamento de Produtos:** Funcionalidades para adicionar nome, descrição, preço, estoque, categoria e imagem de produtos.
- **Gerenciamento de Categorias:** Funcionalidades para adicionar nome e descrição de categorias.
- **Validação de Dados:** Implementação de validações no modelo de dados e nas requisições da API para garantir a integridade dos dados.
- **Documentação da API:** Geração de documentação interativa da API utilizando Swagger/OpenAPI.
- **Tratamento de Erros:** Implementação de tratamento adequado de erros tanto na aplicação MVC quanto na API.
- **Mapeamento de Objetos:** Utilização de padrões de mapeamento (como AutoMapper, embora não explicitamente mencionado nas tecnologias do README) para converter entre modelos de aplicação e entidades de domínio.

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core MVC
  - ASP.NET Core Web API
  - Entity Framework Core (assumido para acesso a dados, embora não listado explicitamente no README)
- **Banco de Dados:** SQL Server (conforme mencionado no README)
- **Autenticação e Autorização:**
  - ASP.NET Core Identity (possível para a aplicação MVC)
  - JWT (JSON Web Token) para autenticação na API (conforme discutido no histórico)
- **Front-end:**
  - Razor Pages/Views (para a aplicação MVC)
  - HTML/CSS para estilização básica (para a aplicação MVC)
- **Documentação da API:** Swagger (conforme discutido no histórico)

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

- src/
  - DevXpertHub.Core/               - Camada de Core - DTOs, Interfaces e Mappers
                                    - Entidades de domínio e Interfaces de Repositório
                                    - Orquestração e Regras de Negócio
  - DevXpertHub.Infrastructure/     - Acesso a Dados - Arquivos de configurações do Entity Framework Core, Migrations e Repositórios
  - DevXpertHub.Infrastructure/Data - Armazenamento do banco de dados SQLite, quando em ambiente de desenvolvimento
  - DevXpertHub.Api/                - Projeto da API RESTful - Controllers, ViewModels, JwtSettings e Transformers
  - DevXpertHub.Web/                - Projeto da Aplicação MVC - Controllers, Views, ViewModels, Extensions e Mappers

- README.md                     - Arquivo de Documentação do Projeto
- FEEDBACK.md                   - Arquivo para Consolidação dos Feedbacks do instrutor
- .gitignore                    - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **CRUD para Posts e Comentários:** Permite criar, editar, visualizar e excluir posts e comentários.
- **Autenticação e Autorização:** Diferenciação entre usuários comuns e administradores.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 9.0 ou superior
- SQLite (desenvolvimento) e SQL Server (produção)
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/dicksouza/DevXpertHub.git`
   - `cd DevXpertHub`

2. **Configuração do Banco de Dados:**
Durante a execução do projeto em ambiente de desenvolvimento, o banco de dados será criado automaticamente, bem como a pasta `Data`, necessária para o funcionamento local com SQLite.
Após criar o banco de dados em desenvolvimento, algumas tabelas são populadas automaticamente com dados de exemplo.
O usuário padrão criado para administrar as categorias e produtos é o admin@devxperthub.com e a senha Admin@123.
Opcionalmente pode ser criado outros usuários, como Consumidor ou Fornecedor.

#### Ambiente de Desenvolvimento

O projeto utiliza **SQLite** no ambiente de desenvolvimento. A configuração da string de conexão está definida no arquivo `appsettings.Development.json` dos projetos `DevXpertHub.Api` e `DevXpertHub.Web`. Por padrão, o banco será salvo na pasta:
`src/DevXpertHub.Infrastructure/Data/DevXpertHub.db`.

Essa pasta será criada automaticamente ao executar a aplicação.

#### Execução das Migrações

**Não é necessário executar comandos manuais** para criar o banco ou aplicar migrações em ambiente de desenvolvimento. Ao iniciar a aplicação (`DevXpertHub.Api` ou `DevXpertHub.Web`), as migrações pendentes são aplicadas automaticamente e dados iniciais (categorias e perfis de usuário) são inseridos no banco.

> 💡 Essa lógica é aplicada apenas nos ambientes: `Development`, `Docker` e `Staging`.

#### Ambiente de Produção

Em produção, o projeto utiliza **SQL Server**. Para configurar, altere a string de conexão no arquivo `appsettings.Production.json` com os dados do seu ambiente.

Caso deseje aplicar as migrações manualmente, utilize um dos comandos abaixo a partir da raiz da solução:

```bash
dotnet ef database update -p src/DevXpertHub.Infrastructure -s src/DevXpertHub.Api
# ou
dotnet ef database update -p src/DevXpertHub.Infrastructure -s src/DevXpertHub.Web

```` 

3. **Executar a Aplicação MVC:**

    ```bash
    cd src/DevXpertHub.Web/
    dotnet run
    ```
  - Acesse a aplicação em: https://localhost:7032

4. **Executar a API:**

   ``` bash
   cd src/DevXpertHub.Api/
   dotnet run
   ```
   - Acesse a documentação da API em: https://localhost:7267/swagger

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.Development.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:7267/swagger

## **9. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
