# Mentor Interface

A REST API providing authentication and access to the Mentor Engine

## Paddle Plans, Subscriptions and Roles
Users can have different (AspNet)`Roles`, e.g. "Admin" or "Premium", which may be required for accessing certain resources.
Some Roles are available to users through `Subscriptions`.
For each `Subscription` there may be one or multiple PaddlePlans the user can choose from, differing e.g. by contract duration and pricing. 

## Environment Variables

- `STEAM_API_KEY` : Steam application key for OpenID authentication. [\*]
- `MYSQL_CONNECTION_STRING` : Connection string for the User Database. [\*]
- `IS_MIGRATING` : Warning: Do not use in production. If this is set to true, MentorInterface will be able to run migrations but nothing else. Only works if `MYSQL_CONNECTION_STRING` is also provided.
- `IDENTITY_WORKAROUND_BEARER_TOKEN` : The token which devs need to add in Headers["Authorization"]="Bearer [token]" to authenticate
- `IDENTITY_WORKAROUND_USER_ID` : integer, the default ApplicationUserId of the User assigned to devs who login with above Bearer Token. Can be overriden by specifying Headers["Impersonate-ApplicationUserId"]
- `PADDLE_VENDOR_ID` : int, provided by Paddle for identifying us. [\*]
- `PADDLE_VENDOR_AUTH_CODE` : string, our secret key for communication with Paddle.[\*]

**JWT**

- `JWT_SIGNING_KEY`: The passphrase used to create and validate JWT Tokens. [\*]
- `JWT_ISSUER`: The issuer of the tokens. (`api.mentor.gg`) [\*]
- `JWT_AUDIENCE`: The valid audience sending the token (`mentor.gg`) [\*]
- `JWT_VALIDITY`: The amount of time, in minutes a token is valid for. [\*]

[\*] *Required*


## Monitoring / Prometheus

Collect metrics on port `9913` (Defined in `Startup.cs:METRICS_PORT`) at `/metrics`
