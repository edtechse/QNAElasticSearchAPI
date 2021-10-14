#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ARG SONAR_PROJECT_KEY=qna-elasticsearch-api
ARG SONAR_OGRANIZAION_KEY=edtechproject
ARG SONAR_HOST_URL=https://sonarcloud.io
ARG SONAR_TOKEN=5a1e737f247648730bc964be02b80027ccdf3ac1

WORKDIR /src

RUN apt-get update && apt-get install -y openjdk-11-jdk
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console --version 1.7.2 
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /o:"$SONAR_OGRANIZAION_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  /d:sonar.cs.opencover.reportsPaths=/coverage.opencover.xml
WORKDIR /src
COPY ["QnAElasticSearchService.csproj", ""]
RUN dotnet restore "./QnAElasticSearchService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "QnAElasticSearchService.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "QnAElasticSearchService.csproj" -c Release -o /app/publish

RUN dotnet sonarscanner end /d:sonar.login=5a1e737f247648730bc964be02b80027ccdf3ac1

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QnAElasticSearchService.dll"]
