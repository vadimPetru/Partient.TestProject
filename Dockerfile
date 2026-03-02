
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Partient.TestProject.Application/Partient.TestProject.Application.csproj", "Partient.TestProject.Application/"]
COPY ["Partient.TestProject.Domain/Partient.TestProject.Domain.csproj", "Partient.TestProject.Domain/"]
COPY ["Partient.TestProject.Infrastructure/Partient.TestProject.Infrastructure.csproj", "Partient.TestProject.Infrastructure/"]
COPY ["Partient.TestProject.Web/Partient.TestProject.Web.csproj", "Partient.TestProject.Web/"]

RUN dotnet restore "Partient.TestProject.Web/Partient.TestProject.Web.csproj"

COPY . .

WORKDIR "/src/Partient.TestProject.Web/"
RUN dotnet publish "Partient.TestProject.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Partient.TestProject.Web.dll"]