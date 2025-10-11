# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution and restore as distinct layers
COPY Condominio.sln ./
COPY WebHost/WebHost.csproj ./WebHost/
RUN dotnet restore ./WebHost/WebHost.csproj

# Copy the rest of the source code
COPY . ./


# Build the application (restore will run automatically)
RUN dotnet publish ./WebHost/WebHost.csproj -c Release -o /app/publish

# Use the official .NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Ensure ASP.NET Core listens on all interfaces and the correct port
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# Expose port 8080
EXPOSE 8080

# Set the entrypoint
ENTRYPOINT ["dotnet", "WebHost.dll"]
