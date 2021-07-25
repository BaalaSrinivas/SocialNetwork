FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
ENV ASPNETCORE_URLS=https://+:443  
EXPOSE 443
WORKDIR /app
COPY bin/Release/net5.0/publish ./
ENTRYPOINT ["dotnet", "ContentService.dll"]
