# ===============================================================
# Multi-stage Dockerfile for UserService (.NET 8 Web API)
# Stage 1: Build and publish the application
# Stage 2: Use lightweight runtime for final image
# ===============================================================

# ---------------------------------------------------------------
# Stage 1: Build stage
# Starts from the full .NET 8 SDK image for building the app
# ---------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file first to leverage Docker caching
# This allows cached layer reuse if dependencies haven’t changed
COPY UserService.csproj ./
RUN dotnet restore

# Copy the rest of the source and publish to a folder
# Output goes to /app/publish in Release configuration
COPY . ./
RUN dotnet publish -c Release -o /app/publish


# ---------------------------------------------------------------
# Stage 2: Final runtime stage
# Uses ASP.NET Core runtime image (no SDK) for leaner deployment
# ---------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Environment variables to allow HTTP communication between services
# These are essential for avoiding TLS-related errors in Docker networks
ENV DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
ENV DOTNET_SYSTEM_NET_SECURITY_ALLOWUNENCRYPTED=true

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Define the container startup command
ENTRYPOINT ["dotnet", "UserService.dll"]
