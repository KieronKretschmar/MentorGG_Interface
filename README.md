# Mentor Interface

A REST API providing authentication and access to the Mentor Engine

## Paddle Plans, Subscriptions and Roles
Users can have different (AspNet)`Roles`, e.g. "Admin" or "Premium", which may be required for accessing certain resources.
Some Roles are available to users through `Subscriptions`.
For each `Subscription` there may be one or multiple PaddlePlans the user can choose from, differing e.g. by contract duration and pricing. 


## Building

`docker build -t mentorinterface:<version> .`

## Run the docker image

`docker run --name mi_container --rm -d -p 80:80 mentorinterface:<version>`

## Environment Variables

- `STEAM_API_KEY` : Steam application key for OpenID authentication. [*]
- `MYSQL_CONNECTION_STRING` : Connection string for the User Database. [*]

[*] *Required*


## Monitoring / Prometheus

Collect metrics on port `9913` (Defined in `Startup.cs:METRICS_PORT`) at `/metrics`
