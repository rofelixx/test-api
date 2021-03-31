FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5050

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Segfy.Schedule/Segfy.Schedule.csproj", "src/Segfy.Schedule/"]
RUN dotnet restore "src/Segfy.Schedule/Segfy.Schedule.csproj"
COPY . .
WORKDIR "/src/src/Segfy.Schedule"
RUN dotnet build "Segfy.Schedule.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Segfy.Schedule.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Segfy.Schedule.dll"]
