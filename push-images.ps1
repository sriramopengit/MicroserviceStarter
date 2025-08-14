# ============================================================
# push-images.ps1
# PowerShell script to tag and push Docker images for
#   1. InventoryService (.NET Core Web API)
#   2. UserService (.NET Core Web API)
#   3. Inventory SQL Server (with DB + tables)
#   4. User SQL Server (with DB + tables)
#
# Images are tagged with your Docker Hub username and a
# dynamically generated version number for uniqueness.
# ============================================================

# ------------------------------------------------------------
# Step 1 Set Docker Hub username
# Replace with your actual Docker Hub username.
# This will be used in image names like
#   sriramdockersinventoryserviceversion
# ------------------------------------------------------------
$DOCKER_HUB_USER = "sriramdockers"

# ------------------------------------------------------------
# Step 2 Generate dynamic version tag based on date & time
# Format yyyyMMdd-HHmm
# Example 20250811-1420 = Aug 11, 2025 at 1420
# This ensures each push is uniquely tagged.
# ------------------------------------------------------------
$VERSION = (Get-Date -Format "yyyyMMdd-HHmm")

# ------------------------------------------------------------
# Step 3 Tag & push InventoryService image
# docker tag creates a new tag for an existing local image.
# Here we take the image built by docker-compose
# (microservicestarter-inventoryservice) and tag it for Docker Hub.
# ------------------------------------------------------------
Write-Host Tagging InventoryService image...
docker tag microservicestarter-inventoryservice "$DOCKER_HUB_USER/inventoryservice:$VERSION"
Write-Host Pushing InventoryService image to Docker Hub...
docker push "$DOCKER_HUB_USER/inventoryservice:$VERSION"

# ------------------------------------------------------------
# Step 4 Tag & push UserService image
# Same approach as InventoryService — tag locally, then push.
# ------------------------------------------------------------
Write-Host Tagging UserService image...
docker tag microservicestarter-userservice "$DOCKER_HUB_USER/userservice:$VERSION"
Write-Host Pushing UserService image to Docker Hub...
docker push "$DOCKER_HUB_USER/userservice:$VERSION"
# ------------------------------------------------------------
# Step 5 Commit & push Inventory SQL Server container
# docker commit takes a running container and saves its
# entire current state as a new image. This includes
#   - SQL Server binaries
#   - Databases and tables already created
#   - Any other changes made inside the container
# We then tag that committed image for Docker Hub and push it.
# ------------------------------------------------------------
Write-Host Tagging Inventory SQL Server container as a new image...
docker commit inventory-sqlserver "$DOCKER_HUB_USER/inventory-sqlserver:$VERSION"
Write-Host Pushing Inventory SQL Server image to Docker Hub...
docker push "$DOCKER_HUB_USER/inventory-sqlserver:$VERSION"

# ------------------------------------------------------------
# Step 6 Commit & push User SQL Server container
# Same logic as Step 5, but for the user-sqlserver container.
# ------------------------------------------------------------
Write-Host Tagging User SQL Server container as a new image...
docker commit user-sqlserver "$DOCKER_HUB_USER/user-sqlserver:$VERSION"
Write-Host Pushing User SQL Server image to Docker Hub...
docker push "$DOCKER_HUB_USER/user-sqlserver:$VERSION"

# ------------------------------------------------------------
# Step 7 Completion message
# ------------------------------------------------------------
Write-Host ✅ All images have been tagged with version $VERSION and pushed to Docker Hub.
