cd ./Services/ApiGateway 
start cmd /c ApiGateway.cmd
cd ../../

cd ./Services/BlobService
start cmd /c BlobService.cmd
cd ../../

cd ./Services/ContentService
start cmd /c ContentService.cmd
cd ../../

cd ./Services/FollowService
start cmd /c FollowService.cmd
cd ../../

cd ./Services/IdentityAndAccessManagement
start cmd /c IdentityAndAccessManagement.cmd
cd ../../

cd ./Services/MessageBus
start cmd /c /k MessageBus.cmd
cd ../../

REM cd ./Services/NewsfeedService
REM start cmd /c NewsfeedService.cmd
REM cd ../../

REM cd ./Services/NotificationService
REM start cmd /c NotificationService.cmd
REM cd ../../

cd ./Services/UserManagement
start cmd /c UserManagement.cmd
cd ../../

cd ./UI-New
start cmd /c UI-New.cmd
cd ../../






