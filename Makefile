dev-up:
	docker compose -p whispr -f ./docker/docker-compose.dev.yaml up -d --build
dev-down:
	docker compose -p whispr -f ./docker/docker-compose.dev.yaml down
dev-restart: dev-down dev-up

prod-up:
	docker compose --env-file ./.env -p whispr -f ./docker/docker-compose.prod.yaml up -d --build
prod-down:
	docker compose --env-file ./.env -p whispr -f ./docker/docker-compose.prod.yaml down
prod-restart: prod-up prod-down

add-migration:
	dotnet ef migrations add $(v) --project .\src\Whispr.Infrastructure\Whispr.Infrastructure.csproj --startup-project .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj --context ApplicationDbContext
remove-migration:
	dotnet ef migrations remove --project .\src\Whispr.Infrastructure\Whispr.Infrastructure.csproj --startup-project .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj --context ApplicationDbContext
update-database:
	dotnet ef database update --project .\src\Whispr.Infrastructure\Whispr.Infrastructure.csproj --startup-project .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj --context ApplicationDbContext