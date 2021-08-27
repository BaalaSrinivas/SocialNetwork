cd %systemroot%\system32\inetsrv\
SET BasePath=C:\Work\Learning\Projects\SocialNetwork\Code\Deploy

appcmd stop site "Default Web Site"

appcmd add apppool /name:ApigatewayAP
appcmd add site /name:Apigateway /physicalPath:%BasePath%\Apigateway /bindings:https/*:5000:
appcmd set site /site.name:Apigateway /[path='/'].applicationPool:ApigatewayAP

appcmd add apppool /name:BlobServiceyAP
appcmd add site /name:BlobService /physicalPath:%BasePath%\BlobService /bindings:https/*:5001:
appcmd set site /site.name:BlobService /[path='/'].applicationPool:BlobServiceyAP

appcmd add apppool /name:ContentServiceAP
appcmd add site /name:ContentService /physicalPath:%BasePath%\ContentService /bindings:https/*:5002:
appcmd set site /site.name:ContentService /[path='/'].applicationPool:ContentServiceAP

appcmd add apppool /name:FollowServiceAP
appcmd add site /name:FollowService /physicalPath:%BasePath%\FollowService /bindings:https/*:5003:
appcmd set site /site.name:FollowService /[path='/'].applicationPool:FollowServiceAP

appcmd add apppool /name:IdentityAndAccessManagementAP
appcmd add site /name:IdentityAndAccessManagement /physicalPath:%BasePath%\IdentityAndAccessManagement /bindings:https/*:5004:
appcmd set site /site.name:IdentityAndAccessManagement /[path='/'].applicationPool:IdentityAndAccessManagementAP

appcmd add apppool /name:NewsfeedServiceAP
appcmd add site /name:NewsfeedService /physicalPath:%BasePath%\NewsfeedService /bindings:https/*:5005:
appcmd set site /site.name:NewsfeedService /[path='/'].applicationPool:NewsfeedServiceAP

appcmd add apppool /name:NotificationServiceAP
appcmd add site /name:NotificationService /physicalPath:%BasePath%\NotificationService /bindings:https/*:5006:
appcmd set site /site.name:NotificationService /[path='/'].applicationPool:NotificationServiceAP

appcmd add apppool /name:UserManagementAP
appcmd add site /name:UserManagement /physicalPath:%BasePath%\UserManagement /bindings:https/*:5007:
appcmd set site /site.name:UserManagement /[path='/'].applicationPool:UserManagementAP

appcmd add apppool /name:UIAP
appcmd add site /name:UI /physicalPath:%BasePath%\UI /bindings:http/*:80:
appcmd set site /site.name:UI /[path='/'].applicationPool:UIAP

pause