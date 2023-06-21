FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /IdentityAut

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out
EXPOSE 8080
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /IdentityAut
COPY --from=build-env /IdentityAut/out .
ENTRYPOINT ["dotnet", "MVC.dll"]