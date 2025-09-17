## Creating basic Microservices using Docker

The solution has 3 projects:
  - InventoryService (.Net Core Web API Project)
  - UserService (.Net Core Web API Project)
  - UI

The 3rd project is not used.

Separate __containers__ are created for __InventoryService__ and __UserService__ using __Docker__.  Each of these service has its own DB.
Each __DB__ has its own __container__.

Each of these projects (InventoryService and UserService) has a __Dockerfile__ in their project folders.

The solution folder has an container orchestrator file __dockercompose.yml__. The container orchestrator tool __Docker Compose__ uses the __dockercompose.yml__ file to create containers using the images created by the __Dockerfile__ in the project folders for their respective projects.  Additionally the __dockercompose.yml__ file also creates 2 separate containers, each containing an SQL Server engine, one to hold data for __InventoryService__ and the other for __UserService__.

Running __dockercompose__ file spins the following containers:
  - userservice-1 (container for User Service)
  - inventoryservice-1 (container for Inventory Service)
  - user-sqlserver (container for Microsoft SQL Server, that holds data pertaining to User Service)
  - inventory-sqlserver (container for Microsoft SQL Server, that holds data pertaining to Inventory Service)

The __Port Mapping__ for each of these containers to the localhost are as below:

| Container Name | Localhost Port | Container Port |
| :------------- | :------------- | :------------- |
| userservice-1  | 8081           | 8081           |
| inventoryservice-1 | 8080 | 8080 |
| user-sqlserver | 1434 | 1433 |
| inventory-sqlserver | 1433 | 1433 |

This project also has __Powershell__ scripts to __push images to Docker Hub__ and a separate script to __create a private registry to push and pull images to and from the private registry__.
