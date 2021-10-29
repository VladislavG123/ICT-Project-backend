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
COPY ["IctFinalProject/IctFinalProject.WebAdmin.csproj", "IctFinalProject.WebAdmin/"]
RUN dotnet restore "IctFinalProject/IctFinalProject.WebAdmin.csproj"
COPY . .
WORKDIR "/src/IctFinalProject.WebAdmin"
RUN dotnet build "IctFinalProject.WebAdmin.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "IctFinalProject.WebAdmin.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IctFinalProject.WebAdmin.dll"]

