FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TMS.Api/TMS.Api.csproj", "TMS.Api/"]
COPY ["TMS.Contracts/TMS.Contracts.csproj", "TMS.Contracts/"]
COPY ["TMS.Application/TMS.Application.csproj", "TMS.Application/"]
COPY ["TMS.Domain/TMS.Domain.csproj", "TMS.Domain/"]
COPY ["TMS.MessagingContracts/TMS.MessagingContracts.csproj", "TMS.MessagingContracts/"]
COPY ["TMS.Infrastructure/TMS.Infrastructure.csproj", "TMS.Infrastructure/"]
RUN dotnet restore "TMS.Api/TMS.Api.csproj"
COPY . .
WORKDIR "/src/TMS.Api"
RUN dotnet build "TMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TMS.Api.dll"]
