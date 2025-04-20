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