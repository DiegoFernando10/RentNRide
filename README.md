# 🚀 RentNRide API

API para **locações de motos**
Desenvolvida em **.NET 9**, com **PostgreSQL**, **RabbitMQ** e **MinIO** integrados via Docker.

---

## 📋 Pré-requisitos

Antes de executar o projeto, você precisa ter instalado:

- [Docker Desktop](https://www.docker.com/products/docker-desktop)  
- [PowerShell 7+](https://learn.microsoft.com/pt-br/powershell/scripting/install/installing-powershell)  

---

## ▶️ Como executar o projeto
Execute o script de deploy:

powershell
Copiar código
./deploy.ps1
Esse script irá:

Criar as imagens Docker

Subir os containers da aplicação (rentnride-container) e dependências (Postgres, RabbitMQ, MinIO)

Disponibilizar a API em http://localhost:8080

🌐 Acessando a API
Após o deploy, você poderá acessar:

Swagger (documentação interativa):

bash
Copiar código
http://localhost:8080/swagger
Exemplo de endpoints disponíveis:

GET /v1/entregadores → Lista entregadores

POST /v1/entregadores → Cadastro de entregador

GET /v1/motos → Lista motos

POST /v1/motos → Cadastro de moto

GET /v1/planos → Lista planos de locação

POST /v1/locacao → Cria uma locação

PUT /v1/locacao/{id}/devolucao → Finaliza locação

🛠️ Serviços do projeto
O projeto utiliza os seguintes serviços em containers:

API (RentNRide) → Porta 6060

PostgreSQL → Porta 5432

RabbitMQ → Porta 5672 (painel de gestão em http://localhost:15672)

MinIO → Porta 9000 (console em http://localhost:9001)

**Ver arquivo .env para obter os logins.**

📦 Estrutura do projeto
bash
Copiar código
RentNRide/
│
├── RentNRide.Api/              # Projeto principal da API
├── RentNRide.Service/          # Serviços de domínio (drivers, motos, planos, locações)
├── RentNRide.Data/             # Contexto do banco e entidades
├── RentNRide.Listener/         # Consumers RabbitMQ
├── RentNRide.Providers/        # Integrações externas (RabbitMQ, MinIO)
├── deploy.ps1                  # Script para build e deploy com Docker
└── docker-compose.yml          # Orquestração dos containers
✅ Testando a API
No Swagger, você pode executar chamadas diretamente.

Exemplo via curl para listar planos:

bash
Copiar código
curl http://localhost:8080/v1/planos
🧹 Encerrando os containers
Para parar os containers:

powershell
Copiar código
docker compose down
Para parar e remover volumes:

powershell
Copiar código
docker compose down -v
