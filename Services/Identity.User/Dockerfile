# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY Identity.User.Service.sln .
COPY Ecubytes.Identity.User.Api/*.csproj ./Ecubytes.Identity.User.Api/
COPY Ecubytes.Identity.User.Data/*.csproj ./Ecubytes.Identity.User.Data/
COPY Ecubytes.Identity.User.Infrastructure/*.csproj ./Ecubytes.Identity.User.Infrastructure/
RUN dotnet restore

# copy everything else and build app
COPY Ecubytes.Identity.User.Api/. ./Ecubytes.Identity.User.Api
COPY Ecubytes.Identity.User.Data/. ./Ecubytes.Identity.User.Data
COPY Ecubytes.Identity.User.Infrastructure/. ./Ecubytes.Identity.User.Infrastructure
WORKDIR /source/Ecubytes.Identity.User.Api
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Ecubytes.Identity.User.Api.dll"]