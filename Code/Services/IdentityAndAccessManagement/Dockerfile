FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS build-env
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80
WORKDIR /app
COPY bin/Release/net5.0/publish ./
ENTRYPOINT ["dotnet", "IdentityAndAccessManagement.dll"]