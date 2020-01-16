# ===============
# BUILD IMAGE
# ===============
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers

WORKDIR /app/Entities
COPY ./Entities/*.csproj ./
RUN dotnet restore

WORKDIR /app/Database
COPY ./Database/*.csproj ./
RUN dotnet restore

WORKDIR /app/MentorInterface
COPY ./MentorInterface/*.csproj ./
RUN dotnet restore

# Copy everything else and build
WORKDIR /app
COPY ./MentorInterface/ ./MentorInterface
COPY ./Database/ ./Database
COPY ./Entities ./Entities

RUN dotnet publish MentorInterface/ -c Release -o out

# ===============
# RUNTIME IMAGE
# ===============
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MentorInterface.dll"]

