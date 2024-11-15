FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/*.csproj ./

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

ENV ASPNETCORE_URLS=http://+:5190;http://+5189
EXPOSE 5190 5189




ENTRYPOINT ["dotnet", "AssetServer.dll"]