#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FraudIdent.Backbone/FraudIdent.Backbone.csproj", "FraudIdent.Backbone/"]
COPY ["FraudIdent/FraudIdent.csproj", "FraudIdent/"]
RUN dotnet restore "FraudIdent/FraudIdent.csproj"
COPY . .
WORKDIR "/src/FraudIdent"
RUN dotnet build "FraudIdent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FraudIdent.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FraudIdent.dll"]