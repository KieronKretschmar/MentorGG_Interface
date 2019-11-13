FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./MentorInterface/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./MentorInterface/ ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MentorInterface.dll"]

# Enable HTTPS
#ENV ASPNETCORE_URLS="https://+;http://+"

# Enable Development Mode
# ENV ASPNETCORE_ENVIRONMENT="Development"
