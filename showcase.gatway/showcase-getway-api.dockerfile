ARG VERSION=6.0
ARG TARGET_FRAMEWORK="net6.0"
ARG PUBLISH_ARGS="-nowarn:msb3202,nu1503"
ARG PUBLISH_ARGS="-warnaserror"

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build 
WORKDIR /showcase
   
# copy source
COPY ./showcase-gatway/showcase.gatway .

RUN dotnet restore  
RUN dotnet build --configuration Release 
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION as runtime 
WORKDIR /showcase

COPY --from=build /showcase/out ./

# # Padrão de container ASP.NET
# ENTRYPOINT ["dotnet", "Account.Api.dll"]
# Opção utilizada pelo Heroku
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet showcase.gatway.dll

CMD ASPNETCORE_URLS=http://*:$PORT dotnet showcase.gatway.dll