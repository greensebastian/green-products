# Green Products

## Running locally

### Running separately
Instructions for running backend and frontend separately are available in their respective folders.

### Docker compose
Beware docker compose might take a long time first time running it.

1. `docker compose up -d`
2. If this is the first time with a new volume, the database must be seeded by sending an empty put request to http://localhost:5068/seed. This can be done though a button on the create-product page or through swagger on http://localhost:5068/swagger
3. View frontend at http://localhost:8081