# Sử dụng .NET SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy các file project và restore dependencies
COPY ["DemoDocker.sln", "./"]
COPY ["DemoDocker/*.csproj", "DemoDocker/"]
COPY ["DemoDocker_Infrastructure/*.csproj", "DemoDocker_Infrastructure/"]
COPY ["DemoDocker_Domain/*.csproj", "DemoDocker_Domain/"]
COPY ["DemoDocker_Application/*.csproj", "DemoDocker_Application/"]
COPY ["DemoDocker_Common/*.csproj", "DemoDocker_Common/"]

RUN dotnet restore

# Copy toàn bộ source code
COPY . .

# Build và publish ứng dụng
RUN dotnet build "DemoDocker.sln" -c Release -o /app/build
RUN dotnet publish "DemoDocker.sln" -c Release -o /app/publish

# Tạo runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Tạo thư mục cho certificate
RUN mkdir -p /https

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "DemoDocker.dll"] 