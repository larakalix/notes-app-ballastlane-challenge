FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/NotesApp.Api/NotesApp.Api.csproj", "src/NotesApp.Api/"]
COPY ["src/NotesApp.Application/NotesApp.Application.csproj", "src/NotesApp.Application/"]
COPY ["src/NotesApp.Infrastructure/NotesApp.Infrastructure.csproj", "src/NotesApp.Infrastructure/"]
COPY ["src/NotesApp.Domain/NotesApp.Domain.csproj", "src/NotesApp.Domain/"]

RUN dotnet restore "src/NotesApp.Api/NotesApp.Api.csproj"

COPY . .
RUN dotnet publish "src/NotesApp.Api/NotesApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "NotesApp.Api.dll"]
