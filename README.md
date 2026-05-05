# TaskFlow

Clean Architecture solution template for ASP.NET Core, MySQL and Docker.

## Architecture

```txt
src/
  TaskFlow.Api
  TaskFlow.Application
  TaskFlow.Domain
  TaskFlow.Infrastructure

tests/
  TaskFlow.Application.Tests
  TaskFlow.IntegrationTests
```

## Requirements

- .NET 9 SDK
- Docker
- Docker Compose
- MySQL Workbench, optional

## Configuration

To run the project locally, you must set up your development settings:

1.  Navigate to 
2.  Copy  and name the copy .
3.  Fill in the connection strings and credentials in the new  file.

## Run infrastructure only

Use this mode when you want to run the API locally with hot reload or debugger:

```bash
docker compose -f docker-compose.dev.yml up -d taskflow.mysql taskflow.redis
```

Then run the API locally:

```bash
dotnet watch --project src/TaskFlow.Api
```

## Run full Docker environment

```bash
docker compose -f docker-compose.dev.yml up -d
```

