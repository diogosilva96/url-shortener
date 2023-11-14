FROM mcr.microsoft.com/dotnet/sdk:7.0 as build

WORKDIR /app

COPY / .

RUN dotnet restore

RUN dotnet publish /app/src/Url.Shortener.Api/Url.Shortener.Api.csproj -c release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000

COPY --from=build /app/publish /app

EXPOSE 5000

ENTRYPOINT [ "dotnet", "Url.Shortener.Api.dll" ]