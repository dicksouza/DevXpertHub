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
  - DevXpertHub.Domain/             - Camada de Domínio - Entidades de domínio e Interfaces de Repositório
  - DevXpertHub.Services/           - Camada de Serviços da Aplicação - Orquestração e Regras de Negócio
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
   - No arquivo `appsettings.Development.json` em ambos os projetos DevXpertHub.Api e DevXpertHub.Web, configure a string de conexão do SQL Server para o seu ambiente.
   - Execute as migrações do Entity Framework Core a partir da pasta raiz da solução ou do projeto DevXpertHub.Infrastructure:

      ```bash
      dotnet ef database update -p src/DevXpertHub.Infrastructure -s src/DevXpertHub.Api
      # ou
      dotnet ef database update -p src/DevXpertHub.Infrastructure -s src/DevXpertHub.Web

   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Executar a Aplicação MVC:**

    ```bash
    cd src/DevXpertHub.Web/
    dotnet run
    ```
  - Acesse a aplicação em: http://localhost:5000

4. **Executar a API:**

   ``` bash
   cd src/DevXpertHub.Api/
   dotnet run
   ```
   - Acesse a documentação da API em: http://localhost:5001/swagger

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.Development.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5001/swagger

## **9. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.