Project structure:

```
├───docker-compose.yml - Docker compose file
├───docs - Documentation files
└───src - Source Code  
    ├───ApiGateway - Ocelot api gateway
    ├───Core - Shared resources
    │   ├── Helpers
    │   ├── Models
    │   ├── Dtos
    |
    ├───Services - Layer with microservices
    │   └───MicroService - Project structure for a microservice
    │   |   ├───Api
    │   |   |   ├──Dockerfile - Dockerfile for microservice container
    │   |   ├───Services
    │   |   ├───Data
    │   |   └───Tests
    |   |
    |   └───Shared - Shared microservice resources

```