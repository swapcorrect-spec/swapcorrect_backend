# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Set the PORT environment variable
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Use the official ASP.NET Core SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

# Copy the project files and restore dependencies
COPY ["SwapShop.Api/SwapShop.Api.csproj", "SwapShop.Api/"]
COPY ["SwapShop.Domain/SwapShop.Domain.csproj", "SwapShop.Domain/"]
COPY ["SwapShop.Application/SwapShop.Application.csproj", "SwapShop.Application/"]
COPY ["SwapShop.Infrastructure/SwapShop.Infrastructure.csproj", "SwapShop.Infrastructure/"]
RUN dotnet restore "SwapShop.Api/SwapShop.Api.csproj"

# Copy the rest of the files and build the project
COPY . .
WORKDIR "/build/SwapShop.Api"
RUN dotnet build "SwapShop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SwapShop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SwapShop.Api.dll"]
