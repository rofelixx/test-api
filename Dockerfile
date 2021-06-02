FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://+:5050
EXPOSE 5050

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Segfy.Schedule/Segfy.Schedule.csproj", "Segfy.Schedule/"]
COPY ["src/Segfy.Schedule.Infra/Segfy.Schedule.Infra.csproj", "Segfy.Schedule.Infra/"]
COPY ["src/Segfy.Schedule.Model/Segfy.Schedule.Model.csproj", "Segfy.Schedule.Model/"]
RUN dotnet restore "Segfy.Schedule/Segfy.Schedule.csproj"
COPY . .
WORKDIR "/src/Segfy.Schedule"
RUN dotnet build "Segfy.Schedule.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Segfy.Schedule.csproj" -c Release --self-contained --runtime linux-x64 -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Segfy.Schedule.dll"]
