CREATE TABLE [dbo].[FollowEntities]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserId] NVARCHAR(MAX) NOT NULL, 
    [TargetUserId] NVARCHAR(MAX) NOT NULL
)

CREATE TABLE [dbo].[FollowMetaData]
(
	[UserId] NVARCHAR(200) NOT NULL PRIMARY KEY, 
    [FollowersCount] INT NOT NULL, 
    [FriendsCount] INT NOT NULL
)

CREATE TABLE [dbo].[FriendEntities]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserId] NVARCHAR(200) NOT NULL, 
    [TargetUserId] NVARCHAR(200) NOT NULL, 
    [FriendRequestState] INT NOT NULL
)