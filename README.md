# Nuke Commerce Identity Service

[![Build Status](https://img.shields.io/github/actions/workflow/status/israeloliveiranobrega/nuke-commerce-identity-service/build.yml?branch=main)](https://github.com/israeloliveiranobrega/nuke-commerce-identity-service/actions)
[![License](https://img.shields.io/github/license/israeloliveiranobrega/nuke-commerce-identity-service)](LICENSE)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/download)

## About

O **Nuke Commerce Identity Service** é o componente central de autenticação e autorização da plataforma Nuke Commerce. Ele atua como um provedor de identidade (Identity Provider), implementando protocolos modernos como OpenID Connect (OIDC) e OAuth 2.0 para garantir a segurança no acesso aos microserviços do ecossistema.

Este projeto foi construído sobre a stack .NET moderna, utilizando práticas de arquitetura robustas para garantir escalabilidade, manutenibilidade e isolamento de contexto.

## Key Features

* **OpenID Connect & OAuth 2.0 Provider:** Emissão segura de tokens (JWT) para autenticação de usuários e serviços.
* **User Management:** Endpoints para registro, recuperação de senha e gerenciamento de perfil.
* **Role-Based Access Control (RBAC):** Gerenciamento granular de permissões e roles.
* **Centralized Login:** Interface de login unificada para todas as aplicações do ecossistema Nuke.
* **Result Pattern:** Tratamento de erros explícito e funcional para maior previsibilidade do código.

## Architecture & Technology Stack

O projeto segue princípios de **Vertical Slice Architecture**, agrupando código por funcionalidades (features) ao invés de camadas técnicas, facilitando a evolução independente de cada módulo.

**Core Technologies:**
* **.NET 8:** Framework principal.
* **Entity Framework Core:** ORM para persistência de dados.
* **OpenIddict (ou IdentityServer):** Engine de protocolos OIDC/OAuth2.
* **FluentValidation:** Validação robusta de entradas.
* **PostgreSQL / SQL Server:** Banco de dados relacional (configurável).
* **Docker:** Containerização para desenvolvimento e deploy.

## Prerequisites

Antes de iniciar, certifique-se de ter as seguintes ferramentas instaladas em seu ambiente:

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [Git](https://git-scm.com/downloads)

## Getting Started

Siga os passos abaixo para clonar o repositório e executar a aplicação localmente.

### 1. Clone the repository

```bash
git clone [https://github.com/israeloliveiranobrega/nuke-commerce-identity-service.git](https://github.com/israeloliveiranobrega/nuke-commerce-identity-service.git)
cd nuke-commerce-identity-service
