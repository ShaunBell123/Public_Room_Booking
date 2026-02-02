ARG DOTNET_VERSION=10.0
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}

WORKDIR /src

COPY . .

EXPOSE 8080

CMD ["dotnet", "watch", "run", "--project", "RoomBooking.Api/RoomBooking.Api.csproj", "--urls=http://0.0.0.0:8080"]

