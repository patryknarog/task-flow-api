FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/TaskFlow.Domain/TaskFlow.Domain.csproj src/TaskFlow.Domain/
COPY src/TaskFlow.Application/TaskFlow.Application.csproj src/TaskFlow.Application/
COPY src/TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj src/TaskFlow.Infrastructure/
COPY src/TaskFlow.Api/TaskFlow.Api.csproj src/TaskFlow.Api/

RUN dotnet restore src/TaskFlow.Api/TaskFlow.Api.csproj

COPY src/ src/

RUN dotnet publish src/TaskFlow.Api/TaskFlow.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TaskFlow.Api.dll"]
