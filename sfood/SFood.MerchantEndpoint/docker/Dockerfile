FROM microsoft/dotnet:latest

WORKDIR /app

EXPOSE 80

ENV ASPNETCORE_URLS=http://*:80

ENTRYPOINT ["dotnet","SFood.MerchantEndpoint.Host.dll"]
