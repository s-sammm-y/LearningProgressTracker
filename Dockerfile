# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy the entire solution
COPY . .

# Restore dependencies
RUN dotnet restore LearningTracker.Web/LearningTracker.Web.csproj

# Publish the application
RUN dotnet publish LearningTracker.Web/LearningTracker.Web.csproj \
    -c Release \
    --no-restore \
    -o /app/publish \
    /p:UseAppHost=false


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# Install Kerberos GSSAPI runtime library required by Npgsql
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=build /app/publish .

# Render web service port
ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "LearningTracker.Web.dll"]
