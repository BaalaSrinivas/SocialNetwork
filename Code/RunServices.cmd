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

cd ./Services/NewsfeedService
start cmd /c NewsfeedService.cmd
cd ../../

cd ./Services/NotificationService
start cmd /c NotificationService.cmd
cd ../../

cd ./Services/UserManagement
start cmd /c UserManagement.cmd
cd ../../





