# 📄 Sistema de Emissão de Notas Fiscais — Teste Técnico Korp

Solução full-stack desenvolvida para o teste técnico da **Korp**, baseada em arquitetura de microsserviços para gerenciamento de produtos (estoque) e emissão/impressão de Notas Fiscais.

| | |
|---|---|
| **Candidato** | Gabriel Albuquerque |
| **Repositório** | `Korp_Teste_Gabriel` |

---

## 📑 Sumário

- [Vídeo de apresentação](#-vídeo-de-apresentação)
- [Arquitetura](#️-arquitetura)
- [Como executar o projeto](#-como-executar-o-projeto)
- [Detalhamento técnico](#️-detalhamento-técnico)
  - [Frontend (Angular)](#1-frontend-angular)
  - [Backend (C# .NET 8)](#2-backend-c-net-8)
- [Requisitos obrigatórios atendidos](#-requisitos-obrigatórios-atendidos)
- [Tratamento de falhas (resiliência)](#-tratamento-de-falhas-resiliência)
- [Tecnologias utilizadas](#-tecnologias-utilizadas)

---

## 🎥 Vídeo de apresentação

*(Avaliador, clique no link abaixo para assistir à demonstração do sistema, funcionalidades e testes de resiliência)*

👉 **https://drive.google.com/file/d/1wWwYsF89B1QjfVBXedFQ-bLmZe6KShvH/view?usp=sharing**

---

## 🏗️ Arquitetura

O sistema foi desacoplado em dois domínios distintos, cada um com seu próprio banco de dados:

| Microsserviço | Responsabilidade | Banco de dados |
|---|---|---|
| **InventoryService** (Estoque) | Gerenciar produtos | `inventory_db` |
| **BillingService** (Faturamento) | Gerar, armazenar e imprimir Notas Fiscais | `invoice_db` |

**Comunicação e API Gateway**

O Frontend se comunica com a porta `80`, onde um **proxy reverso NGINX** roteia as chamadas:
- `/api/Products` → InventoryService
- `/api/Invoices` → BillingService

A comunicação interna (síncrona) do Faturamento com o Estoque — necessária para abater o saldo na emissão da nota — ocorre pela rede isolada do Docker.

---

## 🚀 Como executar o projeto

O projeto está totalmente containerizado com Docker. Para rodar a aplicação completa (Frontend + Microsserviços + Bancos de Dados + Proxy NGINX):

1. Certifique-se de ter o **Docker** e o **Docker Compose** instalados.
2. Na raiz do projeto, execute:
```bash
   docker compose up --build -d
```
3. Aguarde alguns segundos para os bancos de dados (MySQL) inicializarem — o *healthcheck* garante que as APIs só sobem quando o banco estiver pronto.
4. Acesse a aplicação no navegador: **http://localhost**

**Swagger (documentação das APIs):**
| Serviço | URL |
|---|---|
| Estoque | http://localhost:5225/swagger |
| Faturamento | http://localhost:5132/swagger |

---

## 🛠️ Detalhamento técnico

### 1. Frontend (Angular)

**Ciclos de vida do Angular utilizados**
- `ngOnInit`: utilizado nos componentes de listagem (`ProdutosComponent` e `NotasFiscaisComponent`) para disparar as requisições HTTP de busca inicial de dados assim que o componente é inicializado na tela.

**Uso da biblioteca RxJS**

O RxJS foi amplamente utilizado para lidar com a reatividade e as chamadas assíncronas do `HttpClient`:
- `takeUntilDestroyed` — desinscreve automaticamente os Observables quando o componente é destruído, prevenindo memory leaks.
- `finalize` — desliga os indicadores de carregamento (spinners/loaders) das tabelas, independentemente de a requisição ter retornado sucesso (`next`) ou erro (`error`).
- `subscribe` — para observar as respostas das APIs de criação, listagem e impressão.

**Bibliotecas visuais**
- **Ng-Zorro-Antd** (Ant Design for Angular): escolhida por sua robustez e design corporativo. Componentes utilizados: `NzTable` (tabelas), `NzModal` (modais de cadastro), `NzDrawer` (painel lateral de detalhamento da nota), `NzMessage` (feedbacks/toasts de sucesso e erro) e ícones.

**Outras bibliotecas / módulos**
- `ReactiveFormsModule` — utilizado na criação de formulários reativos escaláveis, com destaque para o uso de `FormArray`, que permite a adição dinâmica de múltiplos produtos em uma única Nota Fiscal.

### 2. Backend (C# .NET 8)

**Frameworks e bibliotecas utilizados**

| Biblioteca | Finalidade |
|---|---|
| ASP.NET Core 8 | Framework base para construção das Web APIs |
| Entity Framework Core + Pomelo MySQL | ORM para mapeamento objeto-relacional e persistência física em MySQL |
| Riok.Mapperly | Mapeamento de alta performance (Source Generator) entre Entidades e DTOs |
| Swashbuckle (Swagger) | Documentação e testes rápidos das APIs |

**Tratamento de erros e exceções**

A aplicação utiliza o recurso nativo do .NET 8 `IExceptionHandler` aliado à interface `IProblemDetailsService` (`GlobalExceptionHandler`). Isso garante que qualquer exceção não tratada seja capturada globalmente e devolvida ao Frontend no padrão de mercado **RFC 7807** (`ProblemDetails`). No Frontend, o Angular lê os campos `detail` e `title` para exibir mensagens de erro amigáveis ao usuário (ex.: *"Serviço de estoque indisponível"*, *"Produto sem saldo suficiente"*).

**Uso de LINQ**

O LINQ foi extensivamente utilizado nos serviços e repositórios para manipulação e consulta de coleções:
- `.Where()` — buscar produtos específicos no estoque.
- `.FirstOrDefaultAsync()` — recuperar a entidade exata no banco de dados.
- `.Select()` — projeção de dados ao listar o histórico de itens.
- `.Any()` / `.All()` — validações combinadas para verificar a viabilidade do saldo antes da emissão.

---

## ✅ Requisitos obrigatórios atendidos

- [x] Arquitetura com no mínimo dois microsserviços (Estoque e Faturamento)
- [x] Tratamento de falha de microsserviço com recuperação e feedback ao usuário
- [x] Persistência real em banco de dados (MySQL, um banco por serviço)

---

## ⚠️ Tratamento de falhas (resiliência)

**Cenário de teste:** o sistema está preparado para lidar com a queda de microsserviços. Caso o **InventoryService** (Estoque) caia, o **BillingService** (Faturamento) captura a falha de comunicação HTTP ao tentar registrar os itens de uma nota.

**Recuperação/feedback:** o `BillingService` encapsula o erro e retorna um status **422 (Unprocessable Entity)** com um descritivo claro. O Frontend intercepta esse `ProblemDetails` e exibe um toast vermelho ao usuário informando que não foi possível se comunicar com o Estoque — impedindo que o sistema quebre silenciosamente.

---

## 🧰 Tecnologias utilizadas

| Camada | Tecnologias |
|---|---|
| Frontend | Angular, RxJS, Ng-Zorro-Antd, ReactiveFormsModule |
| Backend | .NET 8, ASP.NET Core, Entity Framework Core, Pomelo MySQL, Riok.Mapperly, Swashbuckle |
| Banco de dados | MySQL (um banco por microsserviço) |
| Infraestrutura | Docker, Docker Compose, NGINX (proxy reverso) |

---

*Desenvolvido como requisito de teste técnico.*
