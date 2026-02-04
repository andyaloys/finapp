# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY backend/FinApp.sln ./

# Copy project files
COPY backend/FinApp.Domain/FinApp.Domain.csproj FinApp.Domain/
COPY backend/FinApp.Core/FinApp.Core.csproj FinApp.Core/
COPY backend/FinApp.Infrastructure/FinApp.Infrastructure.csproj FinApp.Infrastructure/
COPY backend/FinApp.API/FinApp.API.csproj FinApp.API/

# Restore dependencies
RUN dotnet restore

# Copy all source files
COPY backend/ .

# Build and publish
WORKDIR /src/FinApp.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "FinApp.API.dll"]
