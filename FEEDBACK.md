# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Projeto MVC com navegação bem estruturada e funcionalidades operacionais de CRUD.

  * Pontos negativos:
    - Nenhum.

### Design
  - Interface clara e coesa com a proposta administrativa da aplicação.

### Funcionalidade
  * Pontos positivos:
    - CRUD completo nas camadas MVC e API.
    - Identity configurado com autenticação funcional nas duas camadas.
    - Criação do fornecedor associada ao usuário do Identity, com ID compartilhado.
    - Uso de SQLite com seed de dados e migrations automáticas funcionando corretamente.
    - Modelagem com base em conceitos de DDD bem aplicada.

  * Pontos negativos:
    - Troca do nome `Vendedor` por `Fornecedor`, o que compromete a aderência ao escopo proposto.
    - Não há verificação de domínio para garantir que o produto pertence ao fornecedor logado antes da edição.
    - Arquitetura levemente excessiva: `Core` e `Infrastructure` poderiam estar unificadas em um único `Core`.

## Back End

### Arquitetura
  * Pontos positivos:
    - Boa separação entre camadas: API, MVC, Core e Infrastructure.
    - Implementação de princípios como DDD, abstrações e extensão de serviços.

  * Pontos negativos:
    - Arquitetura sofisticada demais para o nível de complexidade do desafio, tornando o projeto mais verboso do que necessário.
    - Duplicação da lógica de seed (`DbSeedData`) nas camadas API e MVC.

### Funcionalidade
  * Pontos positivos:
    - Funcionalidade de autenticação e domínio geral está presente.
    - As operações CRUD são completas e bem implementadas.

  * Pontos negativos:
    - Falta de validação de domínio na edição de produtos (propriedade pelo usuário).

### Modelagem
  * Pontos positivos:
    - Modelos consistentes e alinhados com boas práticas.
    - Separação de entidades, DTOs, comandos e respostas organizada.

  * Pontos negativos:
    - Uso do nome `Fornecedor` em vez de `Vendedor`.

## Projeto

### Organização
  * Pontos positivos:
    - Uso de `src`, `.sln` na raiz, documentação presente, organização dos arquivos é clara.
    - Arquivos de extensão, configuração e utilitários organizados.

  * Pontos negativos:
    - Nenhum relevante além da duplicação do seed.

### Documentação
  * Pontos positivos:
    - Documentação (`README.md`, `FEEDBACK.md`) presente e completa.
    - Swagger configurado corretamente na API.

  * Pontos negativos:
    - Nenhum.

### Instalação
  * Pontos positivos:
    - SQLite funcional com migrations e seed automáticos.
    - Setup executável localmente de forma prática.

  * Pontos negativos:
    - Seed duplicado pode causar manutenção complicada.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 9.5      | 2,85                                     |
| **Qualidade do Código**       | 20%      | 10       | 2,0                                      |
| **Eficiência e Desempenho**   | 20%      | 9.5      | 1,9                                      |
| **Inovação e Diferenciais**   | 10%      | 10       | 1,0                                      |
| **Documentação e Organização**| 10%      | 10       | 1,0                                      |
| **Resolução de Feedbacks**    | 10%      | 10       | 1,0                                      |
| **Total**                     | 100%     | -        | **9,75**                                 |

## 🎯 **Nota Final: 9,75 / 10**
