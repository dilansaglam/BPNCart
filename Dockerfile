# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 80

# Copy the project files and restore dependencies
COPY *.sln .
COPY BPNCart.API/*.csproj ./BPNCart.API/
COPY BPNCart.Application/*.csproj ./BPNCart.Application/
COPY BPNCart.Domain/*.csproj ./BPNCart.Domain/
COPY BPNCart.Infrastructure/*.csproj ./BPNCart.Infrastructure/
COPY BPNCart.Application.Tests/*.csproj ./BPNCart.Application.Tests/
RUN dotnet restore

# Copy the rest of the application code and build the application
COPY . .
WORKDIR /app/BPNCart.API
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Set environment variable
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/BPNCart.API/out ./
ENTRYPOINT ["dotnet", "BPNCart.API.dll"]