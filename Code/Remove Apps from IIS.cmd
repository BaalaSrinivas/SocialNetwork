cd %systemroot%\system32\inetsrv\

appcmd delete site "Apigateway"

appcmd delete site "BlobService"

appcmd delete site "ContentService"

appcmd delete site "FollowService"

appcmd delete site "IdentityAndAccessManagement"

appcmd delete site "NewsfeedService"

appcmd delete site "NotificationService"

appcmd delete site "UserManagement"

appcmd delete site "UI"

appcmd delete apppool /APPPOOL.name:ApigatewayAP

appcmd delete apppool /APPPOOL.name:BlobServiceyAP

appcmd delete apppool /APPPOOL.name:ContentServiceAP

appcmd delete apppool /APPPOOL.name:FollowServiceAP

appcmd delete apppool /APPPOOL.name:IdentityAndAccessManagementAP

appcmd delete apppool /APPPOOL.name:NewsfeedServiceAP

appcmd delete apppool /APPPOOL.name:NotificationServiceAP

appcmd delete apppool /APPPOOL.name:UserManagementAP

appcmd delete apppool /APPPOOL.name:UIAP

pause