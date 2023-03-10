FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RickAndMorty/RickAndMorty.csproj", "RickAndMorty/"]
RUN dotnet restore "RickAndMorty/RickAndMorty.csproj"
COPY . .
WORKDIR "/src/RickAndMorty"
RUN dotnet build "RickAndMorty.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RickAndMorty.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RickAndMorty.dll"]
