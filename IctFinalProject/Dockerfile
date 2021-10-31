FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["IctFinalProject/IctFinalProject.csproj", "IctFinalProject/"]
COPY ["IctFinalProject.Models/IctFinalProject.Models.csproj", "IctFinalProject.Models/"]
RUN dotnet restore "IctFinalProject/IctFinalProject.csproj"
COPY . .
WORKDIR "/src/IctFinalProject"
RUN dotnet build "IctFinalProject.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "IctFinalProject.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IctFinalProject.dll"]

