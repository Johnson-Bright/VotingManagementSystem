FROM mcr.microsoft.com/dotnet/nightly/sdk:10.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish VotingManagementSystem.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/nightly/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "VotingManagementSystem.dll"]
