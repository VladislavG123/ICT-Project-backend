﻿FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ictFinalProject.WebAdmin/ictFinalProject.WebAdmin.csproj", "ictFinalProject.WebAdmin/"]
COPY ["IctFinalProject.Models/IctFinalProject.Models.csproj", "IctFinalProject.Models/"]
RUN dotnet restore "ictFinalProject.WebAdmin/ictFinalProject.WebAdmin.csproj"
COPY . .
WORKDIR "/src/ictFinalProject.WebAdmin"
RUN dotnet build "ictFinalProject.WebAdmin.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "ictFinalProject.WebAdmin.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ictFinalProject.WebAdmin.dll"]

