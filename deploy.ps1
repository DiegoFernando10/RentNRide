docker-compose build
docker-compose run --rm db_migration
docker-compose up -d api postgres rabbitmq minio