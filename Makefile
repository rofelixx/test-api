build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
test:
	dotnet test
watch:
	dotnet watch --project src/Segfy.Schedule/Segfy.Schedule.csproj run
start:
	dotnet run --project src/Segfy.Schedule/Segfy.Schedule.csproj