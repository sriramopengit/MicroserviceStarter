# =====================================================================
# Script Name   : push-pull-userservice.ps1
# Description   : Push local UserService image to local registry, pull it,
#                 and run it from the registry.
# Author        : Your Name
# Date          : 2025-08-14
# =====================================================================

# ---------------------------------------------------------------------------------------------------------
# Step 1: Start local registry (persistant storage of the images created in the Docker Container Registry)
# ---------------------------------------------------------------------------------------------------------
# Create a Docker volume to store registry data outside the container filesystem
docker volume create registry-data

# Remove any old registry container (if exists) so we can recreate it with the volume
docker rm -f registry 2>$null


# docker run:
#   -d              → Run container in detached mode (background)
#   -p 5000:5000    → Map host port 5000 to container port 5000
#   --name registry → Name the container "registry"
#   -v registry-data:/var/lib/registry → Mount named volume to registry's internal storage path
#   registry:2      → Use the official Docker registry image version 2

Write-Host "Starting local registry on port 5000..."

docker run -d -p 5000:5000 --name registry `
  -v registry-data:/var/lib/registry `
  registry:2

# Wait for registry to start (prevents "connection refused" when pushing immediately)
Write-Host "Waiting for registry container to initialize..."
Start-Sleep -Seconds 5  # Adjust if your system is slower

# -------------------------------
# Step 2: Tag local image for registry
# -------------------------------
# docker tag:
#   Format: docker tag <source_image>:<tag> <registry>/<image_name>:<tag>
#   Here we re-tag the existing local image so it can be pushed to our local registry
Write-Host "Tagging local microservicestarter-userservice:latest image..."
docker tag microservicestarter-userservice:latest localhost:5000/microservicestarter-userservice:1.0

# -------------------------------
# Step 3: Push image to registry
# -------------------------------
# docker push sends the image to the registry we specified in the tag
Write-Host "Pushing image to registry..."
docker push localhost:5000/microservicestarter-userservice:1.0

# -------------------------------
# Step 4: Verify registry contents
# -------------------------------
# Invoke-WebRequest:
#   -Uri             → The HTTP endpoint to call (here the registry API endpoint /v2/_catalog)
#   -UseBasicParsing → In older PowerShell versions (< 6.0), prevents Internet Explorer rendering and returns raw HTML/JSON.
#
# This specific registry API endpoint:
#   http://localhost:5000/v2/_catalog
#   Returns JSON showing available repositories in the registry, e.g.:
#   {"repositories":["microservicestarter-userservice"]}
#
# Select-Object -ExpandProperty Content:
#   Extracts only the "Content" property from the HTTP response object
#   so we see just the JSON text in the console.
Write-Host "Verifying registry contents..."
Invoke-WebRequest -Uri "http://localhost:5000/v2/_catalog" -UseBasicParsing |
    Select-Object -ExpandProperty Content

# -------------------------------
# Step 5: Pull image from registry
# -------------------------------
# docker pull fetches the image from the registry to ensure it can be retrieved successfully.
Write-Host "Pulling image back from registry..."
docker pull localhost:5000/microservicestarter-userservice:1.0

# -------------------------------
# Step 6: Run container from registry image
# -------------------------------
# docker run:
#   -d                        → Run container in detached mode
#   --name userservice        → Name the container "userservice"
#   -p 8081:8081              → Map host port 8081 to container port 8081
#   localhost:5000/...:1.0    → Image location in the local registry
Write-Host "Running UserService container from registry image..."
docker run -d --name userservice -p 8081:8081 localhost:5000/microservicestarter-userservice:1.0

# -------------------------------
# Completion message
# -------------------------------
Write-Host "`nAll steps completed. UserService should now be running at http://localhost:8081"
