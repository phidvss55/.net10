.PHONY: run build watch

run:
	dotnet run --project webapi.csproj

build: 
	dotnet build -- webapi.csproj
	
watch:
	dotnet watch --project webapi.csproj run