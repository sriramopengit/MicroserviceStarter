# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY UserService.csproj ./
RUN dotnet restore

# Copy the full source and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Fix TLS and HTTP handler issues
ENV DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
ENV DOTNET_SYSTEM_NET_SECURITY_ALLOWUNENCRYPTED=true

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UserService.dll"]
