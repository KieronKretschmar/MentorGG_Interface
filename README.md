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
- `IDENTITY_WORKAROUND_BEARER_TOKEN` : The token which devs need to add in Headers["Authorization"]="Bearer [token]" to authenticate
- `IDENTITY_WORKAROUND_USER_ID` : integer, the ApplicationUserId of the User assigned to devs who login with above Bearer Token.

[*] *Required*


## Monitoring / Prometheus

Collect metrics on port `9913` (Defined in `Startup.cs:METRICS_PORT`) at `/metrics`
