## Creating basic Microservices using Docker

The solution has 3 projects:
  - InventoryService (.Net Core Web API Project)
  - UserService (.Net Core Web API Project)
  - UI

  The 3rd project is not used.

  Separate __containers__ are created for __InventoryService__ and __UserService__ using __Docker__.  Each of these service has its own DB.
  Each __DB__ has its own __container__ 