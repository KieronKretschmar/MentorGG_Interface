# Mentor Interface

A REST API providing authentication and access to the Mentor Engine

## Building

`docker build -t mentorinterface:<version> .`

## Run the docker image

`docker run --name mi_container --rm -d -p 80:80 mentorinterface:<version>`

## Routes
- `/docs/` : Swagger documentation and testing.
- `/authentication/signin/steam` : Sign in using Steam's OpenID service.
- `/authentication/signout` : Delete the session cookie, signing out.
- `/authentication/callback/steam` : Internal callback used for steam user Authentication.
- `/v:<version>/verify/` : Debug route for verifing Authentication and Authorization.


## Enviroment Variables

- `STEAM_API_KEY` : Steam application key for OpenID authentication. [*]
- `MYSQL_CONNECTIONSTRING` : Connection string for the User Database. [*]

[*] *Required*