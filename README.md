# Mentor Interface

A REST API providing authentication and access to the Mentor Engine

## Building

`docker build -t mentorinterface:<version> .`

## Run the docker image

`docker run --name mi_container --rm -d -p 80:80 mentorinterface:<version>`

## Enviroment Variables

- `STEAM_API_KEY` : Steam application key for OpenID authentication. [*]
- `MYSQL_CONNECTIONSTRING` : Connection string for the User Database. [*]

[*] *Required*


## Monitoring / Prometheus

Collect metrics on port `9913` (Defined in `Startup.cs:METRICS_PORT`) at `/metrics`
