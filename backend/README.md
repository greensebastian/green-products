# Green Products backend

## Running tests
Running tests requires docker, as the project utilizes Testcontainers to work with a real database. This means on each test run, a fresh postgres instance is launched an migrated.

1. Start docker
2. `dotnet test`

## Running the solution
1. Compose dependencies using docker compose in project root folder.
2. `dotnet run --project ./GreenProducts.WebApi`
3. http://localhost:5068/swagger
4. Seed database by PUT to /seed