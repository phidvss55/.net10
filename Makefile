.PHONY: run build watch

run:
	dotnet run --project webapi.csproj

build: 
	dotnet build -- webapi.csproj
	
watch:
	dotnet watch --project webapi.csproj run
	
migrate-gen:
	dotnet ef migrations add init
    
migrate-up:
	dotnet ef database update