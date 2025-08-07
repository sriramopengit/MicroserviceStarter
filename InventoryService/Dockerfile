# ===============================================================
# Multi-stage Dockerfile for InventoryService (.NET 8 Web API)
# Stage 1: Build and publish the application
# Stage 2: Use lightweight runtime for final image
# ===============================================================

# ---------------------------------------------------------------
# Stage 1: Build stage
# Uses full SDK image to compile and publish the app
# ---------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY InventoryService.csproj ./
RUN dotnet restore

# Copy the entire source and build the app in Release mode
COPY . ./
RUN dotnet publish -c Release -o /app/publish


# ---------------------------------------------------------------
# Stage 2: Runtime stage
# Uses smaller ASP.NET Core runtime-only image for deployment
# Keeps the image size minimal and secure
# ---------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Enable support for TLS or plaintext HTTP across containers
# Workaround for internal HTTP communication issues in some cases
ENV DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
ENV DOTNET_SYSTEM_NET_SECURITY_ALLOWUNENCRYPTED=true

# Copy published output from the build stage
COPY --from=build /app/publish .

# Set the startup command for the container
ENTRYPOINT ["dotnet", "InventoryService.dll"]
