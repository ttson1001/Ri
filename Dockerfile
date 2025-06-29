# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY BEBase/*.csproj ./BEBase/
RUN dotnet restore ./BEBase/BEBase.csproj

# Copy the rest of the files
COPY . .

# Build the app
RUN dotnet publish ./BEBase/BEBase.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (sửa theo app của bạn nếu khác)
EXPOSE 80
EXPOSE 443

# Run the app
ENTRYPOINT ["dotnet", "BEBase.dll"]
