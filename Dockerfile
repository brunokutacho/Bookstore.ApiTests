FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet build --configuration Release
CMD ["dotnet", "test", "--logger:trx;LogFileName=TestResults.trx", "--results-directory", "./reports"]
