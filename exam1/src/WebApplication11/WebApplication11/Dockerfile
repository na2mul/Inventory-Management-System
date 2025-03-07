FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
RUN apt update && apt install -y nodejs
COPY ["WebApplication11/WebApplication11.csproj","WebApplication11/"]
RUN dotnet restore "WebApplication11/WebApplication11.csproj"

COPY . .
WORKDIR "/src/WebApplication11"
RUN dotnet build "WebApplication11.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebApplication11.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet","WebApplication11.dll"]