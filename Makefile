dev-up:
	docker compose -p whispr -f ./docker/docker-compose.dev.yaml up -d --build
dev-down:
	docker compose -p whispr -f ./docker/docker-compose.dev.yaml down
dev-restart: dev-down dev-up

stg-up:
	docker compose -p whispr -f ./docker/docker-compose.stg.yaml up -d --build
stg-down:
	docker compose -p whispr -f ./docker/docker-compose.stg.yaml down
stg-restart: stg-up stg-down

prod-up:
	docker compose -p whispr -f ./docker/docker-compose.prod.yaml up -d --build
prod-down:
	docker compose -p whispr -f ./docker/docker-compose.prod.yaml down
prod-restart: prod-up prod-down

add-migration:
	dotnet ef migrations add $(v) --project .\src\Whispr.Infrastructure\Whispr.Infrastructure.csproj --startup-project .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj --context ApplicationDbContext
remove-migration:
	dotnet ef migrations remove --project .\src\Whispr.Infrastructure\Whispr.Infrastructure.csproj --startup-project .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj --context ApplicationDbContext
update-database:
	dotnet ef database update --project .\src\Whispr.Infrastructure\Whispr.Infrastructure.csproj --startup-project .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj --context ApplicationDbContext
drop-database:
	dotnet ef database drop --project .\src\Whispr.Infrastructure\Whispr.Infrastructure.csproj --startup-project .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj --context ApplicationDbContext

watch:
	dotnet watch --project .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj run
build:
	dotnet clean .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj
	dotnet build .\src\Whispr.Presentation.Api\Whispr.Presentation.Api.csproj