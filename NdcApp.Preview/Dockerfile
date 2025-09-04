# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["NdcApp.Preview/NdcApp.Preview.csproj", "NdcApp.Preview/"]
COPY ["NdcApp.Core/NdcApp.Core.csproj", "NdcApp.Core/"]

# Restore dependencies
RUN dotnet restore "NdcApp.Preview/NdcApp.Preview.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/NdcApp.Preview"
RUN dotnet build "NdcApp.Preview.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "NdcApp.Preview.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80
EXPOSE 443

# Set environment
ENV ASPNETCORE_URLS=http://+:80

# Start the application
ENTRYPOINT ["dotnet", "NdcApp.Preview.dll"]