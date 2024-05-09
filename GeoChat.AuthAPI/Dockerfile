FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /AuthService
COPY . ./
RUN dotnet restore GeoChat.Auth.sln
RUN dotnet publish GeoChat.Auth.sln -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /AuthService
COPY --from=build /AuthService/out .
ENV ASPNETCORE_HTTP_PORTS=5003
EXPOSE 5003

# For SQLite db only copy the .db file from build environment to this env
COPY --from=build /AuthService/GeoChatAuth.db .

ENTRYPOINT [ "dotnet", "GeoChat.AuthAPI.dll" ]