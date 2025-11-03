# Start using docker
start:
	docker compose up -d

# start Project API from Root
run:
	dotnet run --project API

# Down the project
clear:
	docker compose down
