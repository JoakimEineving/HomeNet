## Project Structure
```
├───docs - Documentation files
└───src - Source Code  
    ├───ApiGateway - Ocelot api gateway
    ├───Core - Shared resources
    │   ├── Helpers
    │   ├── Models
    │   ├── Dtos
    |
    ├───Services - Backend layer with microservices
    │   └───MicroService - Project structure for a microservice
    │   |   ├───Api
    │   |   ├───Services
    │   |   │───Data
    │   |   └───Tests
    |   |
    |   └───Shared - Shared microservice resources
