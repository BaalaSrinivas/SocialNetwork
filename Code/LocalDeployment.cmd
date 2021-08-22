SET hostIp=172.28.0.1
SET ApiGateWayBaseUrl=http://%hostIp%:5000
SET BlobServiceBaseUrl=http://%hostIp%:5001
SET ContentServiceBaseUrl=http://%hostIp%:5002
SET FollowServiceBaseUrl=http:/%hostIp%:5003
SET IdentityAndAccessManagementBaseUrl=http://%hostIp%:5004
SET NewsfeedServiceBaseUrl=http://%hostIp%:5005
SET NotificationServiceBaseUrl=http://%hostIp%:5006
SET UserManagementBaseUrl=http://%hostIp%:5007
SET UiUrl=http://%hostIp%:4200

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=BaalaBsk@123" -p 1433:1433 --hostname sqlserver --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
SET DBName=Temp
SET connectionString=Data Source=%hostIp%;Database=%DBName%;User ID=sa;Password=BaalaBsk@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False

docker run -d -p 15671:15671 -p 15672:15672 -p 5672:5672 --hostname rabbitmq --name rabbitmq rabbitmq:3-management

docker run -d -p 5000:80  -e "RabbitMq__HostName=%hostIp%" -e "ConnectionStrings__default=%connectionString%" -e ApiGateWayBaseUrl=%ApiGateWayBaseUrl% -e BlobServiceBaseUrl=%BlobServiceBaseUrl% -e ContentServiceBaseUrl=%ContentServiceBaseUrl% -e FollowServiceBaseUrl=%FollowServiceBaseUrl% -e IdentityAndAccessManagementBaseUrl=%IdentityAndAccessManagementBaseUrl% -e NewsfeedServiceBaseUrl=%NewsfeedServiceBaseUrl% -e NotificationServiceBaseUrl=%NotificationServiceBaseUrl% -e UserManagementBaseUrl=%UserManagementBaseUrl% -e UiUrl=%UiUrl% --name apigateway contactbalasrinivas/bsk-apigateway

docker run -d -p 5001:80  -e "RabbitMq__HostName=%hostIp%" -e "ConnectionStrings__default=%connectionString%" -e ApiGateWayBaseUrl=%ApiGateWayBaseUrl% -e BlobServiceBaseUrl=%BlobServiceBaseUrl% -e ContentServiceBaseUrl=%ContentServiceBaseUrl% -e FollowServiceBaseUrl=%FollowServiceBaseUrl% -e IdentityAndAccessManagementBaseUrl=%IdentityAndAccessManagementBaseUrl% -e NewsfeedServiceBaseUrl=%NewsfeedServiceBaseUrl% -e NotificationServiceBaseUrl=%NotificationServiceBaseUrl% -e UserManagementBaseUrl=%UserManagementBaseUrl% -e UiUrl=%UiUrl% --name blobservice contactbalasrinivas/bsk-blobservice
SET DBName=SMContent
SET connectionString=Data Source=%hostIp%;Database=%DBName%;User ID=sa;Password=BaalaBsk@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
docker run -d -p 5002:80  -e "RabbitMq__HostName=%hostIp%" -e "ConnectionStrings__default=%connectionString%" -e ApiGateWayBaseUrl=%ApiGateWayBaseUrl% -e BlobServiceBaseUrl=%BlobServiceBaseUrl% -e ContentServiceBaseUrl=%ContentServiceBaseUrl% -e FollowServiceBaseUrl=%FollowServiceBaseUrl% -e IdentityAndAccessManagementBaseUrl=%IdentityAndAccessManagementBaseUrl% -e NewsfeedServiceBaseUrl=%NewsfeedServiceBaseUrl% -e NotificationServiceBaseUrl=%NotificationServiceBaseUrl% -e UserManagementBaseUrl=%UserManagementBaseUrl% -e UiUrl=%UiUrl% --name contentservice contactbalasrinivas/bsk-contentservice
SET DBName=SMFollow
SET connectionString=Data Source=%hostIp%;Database=%DBName%;User ID=sa;Password=BaalaBsk@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False 
docker run -d -p 5003:80  -e "RabbitMq__HostName=%hostIp%" -e "ConnectionStrings__default=%connectionString%" -e ApiGateWayBaseUrl=%ApiGateWayBaseUrl% -e BlobServiceBaseUrl=%BlobServiceBaseUrl% -e ContentServiceBaseUrl=%ContentServiceBaseUrl% -e FollowServiceBaseUrl=%FollowServiceBaseUrl% -e IdentityAndAccessManagementBaseUrl=%IdentityAndAccessManagementBaseUrl% -e NewsfeedServiceBaseUrl=%NewsfeedServiceBaseUrl% -e NotificationServiceBaseUrl=%NotificationServiceBaseUrl% -e UserManagementBaseUrl=%UserManagementBaseUrl% -e UiUrl=%UiUrl% --name followservice contactbalasrinivas/bsk-followservice
SET DBName=SMIdentity
SET connectionString=Data Source=%hostIp%;Database=%DBName%;User ID=sa;Password=BaalaBsk@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
docker run -d -p 5004:80  -e "RabbitMq__HostName=%hostIp%" -e "ConnectionStrings__default=%connectionString%" -e ApiGateWayBaseUrl=%ApiGateWayBaseUrl% -e BlobServiceBaseUrl=%BlobServiceBaseUrl% -e ContentServiceBaseUrl=%ContentServiceBaseUrl% -e FollowServiceBaseUrl=%FollowServiceBaseUrl% -e IdentityAndAccessManagementBaseUrl=%IdentityAndAccessManagementBaseUrl% -e NewsfeedServiceBaseUrl=%NewsfeedServiceBaseUrl% -e NotificationServiceBaseUrl=%NotificationServiceBaseUrl% -e UserManagementBaseUrl=%UserManagementBaseUrl% -e UiUrl=%UiUrl% --name identityandaccessmanagement contactbalasrinivas/bsk-identityandaccessmanagement
 
docker run -d -p 5005:80  -e "RabbitMq__HostName=%hostIp%" -e "ConnectionStrings__default=%connectionString%" -e ApiGateWayBaseUrl=%ApiGateWayBaseUrl% -e BlobServiceBaseUrl=%BlobServiceBaseUrl% -e ContentServiceBaseUrl=%ContentServiceBaseUrl% -e FollowServiceBaseUrl=%FollowServiceBaseUrl% -e IdentityAndAccessManagementBaseUrl=%IdentityAndAccessManagementBaseUrl% -e NewsfeedServiceBaseUrl=%NewsfeedServiceBaseUrl% -e NotificationServiceBaseUrl=%NotificationServiceBaseUrl% -e UserManagementBaseUrl=%UserManagementBaseUrl% -e UiUrl=%UiUrl% --name newsfeedservice contactbalasrinivas/bsk-newsfeedservice
SET DBName=SMNotification
SET connectionString=Data Source=%hostIp%;Database=%DBName%;User ID=sa;Password=BaalaBsk@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
docker run -d -p 5006:80  -e "RabbitMq__HostName=%hostIp%" -e "ConnectionStrings__default=%connectionString%" -e ApiGateWayBaseUrl=%ApiGateWayBaseUrl% -e BlobServiceBaseUrl=%BlobServiceBaseUrl% -e ContentServiceBaseUrl=%ContentServiceBaseUrl% -e FollowServiceBaseUrl=%FollowServiceBaseUrl% -e IdentityAndAccessManagementBaseUrl=%IdentityAndAccessManagementBaseUrl% -e NewsfeedServiceBaseUrl=%NewsfeedServiceBaseUrl% -e NotificationServiceBaseUrl=%NotificationServiceBaseUrl% -e UserManagementBaseUrl=%UserManagementBaseUrl% -e UiUrl=%UiUrl% --name notificationservice contactbalasrinivas/bsk-notificationservice
SET DBName=SMUser
SET connectionString=Data Source=%hostIp%;Database=%DBName%;User ID=sa;Password=BaalaBsk@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
docker run -d -p 5007:80  -e "RabbitMq__HostName=%hostIp%" -e "ConnectionStrings__default=%connectionString%" -e ApiGateWayBaseUrl=%ApiGateWayBaseUrl% -e BlobServiceBaseUrl=%BlobServiceBaseUrl% -e ContentServiceBaseUrl=%ContentServiceBaseUrl% -e FollowServiceBaseUrl=%FollowServiceBaseUrl% -e IdentityAndAccessManagementBaseUrl=%IdentityAndAccessManagementBaseUrl% -e NewsfeedServiceBaseUrl=%NewsfeedServiceBaseUrl% -e NotificationServiceBaseUrl=%NotificationServiceBaseUrl% -e UserManagementBaseUrl=%UserManagementBaseUrl% -e UiUrl=%UiUrl% --name usermanagement contactbalasrinivas/bsk-usermanagement

docker run -d -p 4200:80 -e "apiUrl=http://%hostIp%/api/" -e "identityUrl=http://%hostIp%:5004" --name ui contactbalasrinivas/bsk-ui

pause