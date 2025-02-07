# Green Products

## Running locally

### Running separately
Instructions for running backend and frontend separately are available in their respective folders.

### Docker compose
Beware docker compose might take a long time first time running it.

1. `docker compose up -d`
2. If this is the first time with a new volume, the database must be seeded by sending a put request to http://localhost:8080
3. http://localhost:8080