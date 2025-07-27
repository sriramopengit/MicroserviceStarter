# 1. Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Explanation:
# This sets the base image to the .NET SDK 8.0, which includes tools for building .NET apps.
# The alias `AS build` lets us refer to this stage later (multi-stage build).
# This image is temporary and won't be part of the final image — only used to compile the app.


# 2. Set the working directory inside the container for this build stage
WORKDIR /app

# Explanation:
# WORKDIR creates (if not exists) and moves into the /app directory inside the container.
# All subsequent RUN, COPY, etc., commands will be relative to this folder inside the container.


# 3. Copy only the .csproj file to the container (from build context)
COPY *.csproj ./

# Explanation:
# This copies your .csproj file from your host machine to the /app folder in the container.
# Wildcards like *.csproj are supported only for local matches — not inside nested folders.
# This step helps us restore NuGet packages without copying unnecessary files early.


# 4. Restore NuGet packages (dependencies)
RUN dotnet restore

# Explanation:
# Runs `dotnet restore` to download dependencies defined in the .csproj file.
# Since only .csproj was copied, Docker can cache this layer for faster builds,
# as long as you don’t change the project file.


# 5. Copy the rest of the source code to the container
COPY . ./

# Explanation:
# This copies everything else (controllers, Program.cs, etc.) to /app inside the container.
# This must be done *after* restore to avoid triggering restore again on every small change.


# 6. Build the application and publish it to a folder (in release mode)
RUN dotnet publish -c Release -o /app/publish

# Explanation:
# Publishes the compiled app to the `/app/publish` directory inside the container.
# `-c Release` ensures production-grade optimizations are used.


# 7. Use a smaller runtime image for the final container (no SDK)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Explanation:
# This is a smaller image (ASP.NET runtime only — no SDK/tools).
# This keeps the image size minimal and secure for deployment.


# 8. Set working directory for the final runtime container
WORKDIR /app

# Explanation:
# Again, sets the working directory to /app (fresh context in runtime image).
# The publish folder will be copied into this.


# 9. Copy published output from the build stage to the runtime container
COPY --from=build /app/publish .

# Explanation:
# Copies the published output files from the build stage to the final image’s /app directory.
# Only the binaries and DLLs needed to run the app are copied (no source code).


# 10. Run the application
ENTRYPOINT ["dotnet", "UserService.dll"]

# Explanation:
# This tells the container what command to run when started.
# It launches the compiled DLL using the dotnet runtime — the main entry point of your API.
