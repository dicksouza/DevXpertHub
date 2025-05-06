# Feedback - Avaliação Geral

## Front End
### Navegação
  * Pontos positivos:
    - O projeto possui views e rotas definidas para as funcionalidades no projeto MVC.
    - Implementação de controllers e views para as operações principais.

### Design
    - Será avaliado na entrega final

### Funcionalidade
  * Pontos positivos:
    - Implementações básicas de CRUD para produtos e categorias no front-end MVC.

## Back End
### Arquitetura
  * Pontos positivos:
    - Separação clara entre projetos MVC e API.

  * Pontos negativos:
    - Arquitetura excessivamente complexa com mais camadas do que o necessário (Core, Domain, Services, Infrastructure).
    - As camadas Core, Domain e Services possuem responsabilidades sobrepostas que poderiam estar unificadas.
    - Recomenda-se "Deixar o arsenal técnico para desafios que exigem complexidade".
    - Para o escopo do projeto, uma única camada Core unificando business/data seria suficiente.
    - Entenda que errar na mão da complexidade é tão grave quanto ser simplório demais

### Funcionalidade
  * Pontos positivos:
    - Implementação de migrations automáticas e seed de dados no projeto MVC.
    - Uso do Entity Framework Core.

  * Pontos negativos:
    - Projeto API não implementa migrations ou seed de dados.
    - Falta a implementação da criação automática do vendedor durante o registro do Identity.
    - Excesso de complexidade em funcionalidades não essenciais enquanto requisitos básicos não foram atendidos.

### Modelagem
  * Pontos positivos:
    - Uso do Entity Framework Core para acesso a dados.

  * Pontos negativos:
    - Modelagem excessivamente complexa distribuída em múltiplas camadas sem necessidade.
    - As responsabilidades poderiam estar centralizadas em uma única camada Core.
    - Exagero na modelagem que deveria ser simples e anêmica.

## Projeto
### Organização
  * Pontos positivos:
    - Uso da pasta `src` na raiz.
    - Arquivo de solução (`.sln`) presente.
    - Separação em projetos distintos.

### Documentação
  * Pontos positivos:
    - Repositório com `README.md` presente e bem documentado.
    - Arquivo `FEEDBACK.md` presente.
    - Documentação via Swagger para API.

### Instalação
  * Pontos positivos:
    - Implementação do SQLite para ambiente de desenvolvimento.
    - Migrations automáticas e seed de dados no projeto MVC.

  * Pontos negativos:
    - Ausência de migrations e seed de dados no projeto API.