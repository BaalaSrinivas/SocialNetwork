cd ./UI
docker build -t contactbalasrinivas/bsk-ui .
cd ../

cd ./Services/ApiGateway 
docker build -t contactbalasrinivas/bsk-apigateway .
cd ../../

cd ./Services/BlobService
docker build -t contactbalasrinivas/bsk-blobservice .
cd ../../

cd ./Services/ContentService
docker build -t contactbalasrinivas/bsk-contentservice .
cd ../../

cd ./Services/FollowService
docker build -t contactbalasrinivas/bsk-followservice .
cd ../../

cd ./Services/IdentityAndAccessManagement
docker build -t contactbalasrinivas/bsk-identityandaccessmanagement .
cd ../../

cd ./Services/NewsfeedService
docker build -t contactbalasrinivas/bsk-newsfeedservice .
cd ../../

cd ./Services/NotificationService
docker build -t contactbalasrinivas/bsk-notificationservice .
cd ../../

cd ./Services/UserManagement
docker build -t contactbalasrinivas/bsk-usermanagement .
cd ../../




