## .Net Core

### Structure module

- [project name].sln - Solution File for [project name]
- bin/ - Compiled binaries directory
- obj/ - Object files directory
- src/ - Source code directory
  - [project name]/ - Main project directory
    - [project name].csproj - Project file
    - Program.cs - Main program file
  - [project name].Tests/ - Test project directory
    - [project name].Tests.csproj - Test project file
    - UnitTests.cs - Unit tests file

---

### Run project

```bash
dotnet restore
dotnet build
dotnet run
```

---

### Commands

Run generate migrations

```bash
  dotnet ef migrations add [name-migration]
  -> example: dotnet ef migrations add InitialCreate
```

Execute migration files into database

```bash
  dotnet ef database update
```

---

### Flow

There are two flow inside

1. API

- including `BaseApiController` and `ApiConventionController`

2. View

- All include inside `View` directory
