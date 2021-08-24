cd ./Services/ApiGateway 
dotnet publish -c Release
xcopy /s /i "./bin/Release/net5.0/publish" "../.././Deploy/ApiGateway"
cd ../../

cd ./Services/BlobService
dotnet publish -c Release
xcopy /s /i "./bin/Release/net5.0/publish" "../.././Deploy/BlobService"
cd ../../

cd ./Services/ContentService
dotnet publish -c Release
xcopy /s /i "./bin/Release/net5.0/publish" "../.././Deploy/ContentService"
cd ../../

cd ./Services/FollowService
dotnet publish -c Release
xcopy /s /i "./bin/Release/net5.0/publish" "../.././Deploy/FollowService"
cd ../../

cd ./Services/IdentityAndAccessManagement
dotnet publish -c Release
xcopy /s /i "./bin/Release/net5.0/publish" "../.././Deploy/IdentityAndAccessManagement"
cd ../../

cd ./Services/NewsfeedService
dotnet publish -c Release
xcopy /s /i "./bin/Release/net5.0/publish" "../.././Deploy/NewsfeedService"
cd ../../

cd ./Services/NotificationService
dotnet publish -c Release
xcopy /s /i "./bin/Release/net5.0/publish" "../.././Deploy/NotificationService"
cd ../../

cd ./Services/UserManagement
dotnet publish -c Release
xcopy /s /i "./bin/Release/net5.0/publish" "../.././Deploy/UserManagement"
cd ../../

cd ./UI
npm run-script build

xcopy /s /i "./dist/UI" ".././Deploy/UI"
cd ../

pause




