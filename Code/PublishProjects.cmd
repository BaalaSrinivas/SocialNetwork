cd ./Services/ApiGateway 
dotnet publish -c Release
cd ../../

cd ./Services/BlobService
dotnet publish -c Release
cd ../../

cd ./Services/ContentService
dotnet publish -c Release
cd ../../

cd ./Services/FollowService
dotnet publish -c Release
cd ../../

cd ./Services/IdentityAndAccessManagement
dotnet publish -c Release
cd ../../

cd ./Services/NewsfeedService
dotnet publish -c Release
cd ../../

cd ./Services/NotificationService
dotnet publish -c Release
cd ../../

cd ./Services/UserManagement
dotnet publish -c Release
cd ../../




