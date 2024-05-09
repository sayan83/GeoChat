FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /ChatService
COPY . ./
RUN dotnet restore GeoChat.Chat.sln
RUN dotnet publish GeoChat.Chat.sln -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /ChatService
COPY --from=build /ChatService/out .
ENV ASPNETCORE_HTTP_PORTS=5005
EXPOSE 5005

# For SQLLite copy the .db file into this environment from the build environment
COPY --from=build /ChatService/GeoChatMessages.db .

ENTRYPOINT [ "dotnet", "GeoChat.ChatAPI.dll" ]