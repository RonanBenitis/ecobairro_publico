# Usar a imagem base do SDK do .NET 8.0
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS final

# Defina o diretório de trabalho no contêiner
WORKDIR /source

# Copie todos os arquivos do projeto para o diretório de trabalho
COPY . .

# Restaurar dependências
RUN dotnet restore

# Publica aplicação
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app --use-current-runtime --self-contained false

# Expor a porta 80 para tráfego HTTP
EXPOSE 80

# Comando de entrada para rodar a aplicação
ENTRYPOINT ["dotnet", "/app/ecobairroServer.dll"]