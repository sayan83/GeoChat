FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /RoomService
COPY . ./
RUN dotnet restore GeoChat.Rooms.sln
RUN dotnet publish GeoChat.Rooms.sln -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /RoomService
COPY --from=build /RoomService/out .
ENV ASPNETCORE_HTTP_PORTS=5004
EXPOSE 5004

# For SQLLite copy the .db file into this environment from the build environment
COPY --from=build /RoomService/GeoChatRooms.db .

ENTRYPOINT [ "dotnet", "GeoChat.RoomsAPI.dll" ]