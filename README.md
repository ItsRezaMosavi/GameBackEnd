# Kasit Test Game API

Its a authentication system and leaderboard

## Main Features
- Authentication by JWT token
- Submit Score
- Online Leaderboard

## Used Technology
- Asp.net Core Web API (.net 8.0)
- JWT
- JwtBearer
- C#
- BCrypt

## Getting Started

### Requirements
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)

### 1. Establish Server
```
# Clone Repository
git clone https://github.com/ItsRezaMosavi/GameBackEnd.git

# Enter Server Folder
cd GameBackEnd

# Install Packages
dotnet restore

# Run Server
dotnet run
```
### 2. Establish Client
- 1. Open Unity Hub Project
- 2. Start Scene: ``` Assets/Scenes/AuthScene.unity```
- 3. API settings
	+ Set Server Address in ``` ApiUrl.cs ``` : 
	  ``` public static string apiBaseUrl = "https://localhost:7221/api/"; ```
	+ Configuration Server (```appsettings.json```) : 
	Set database Connection string
	``` bash "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LeaderboardDB;Trusted_Connection=True;"
  } 

## How to Use
- Register New User
```
IEnumerator Register()
{
    RegistrationData data = new RegistrationData {
        UserName = "testUser",
        Password = "Test_123",
        Email = "test@example.com"
    };
    
    yield return StartCoroutine(authManager.SendRegisterRequest(data));
} 
```

- [Sample Api & Endpoint](https://documenter.getpostman.com/view/44755590/2sB2jAa7U7)

## Developer
- [GitHub](https://github.com/ItsRezaMosavi)
- [Gmail](https://mailto:itsrezamosavi@gmail.com/)
- [zil Link](https://zil.ink/itsrezamosavi)