# Start using docker
up:
	docker compose up -d

db-sync:
	dotnet ef database update --project Data --startup-project API

# start Project API from Root
run:
	dotnet run --project API

# Down the project
down:
	docker compose down -v
