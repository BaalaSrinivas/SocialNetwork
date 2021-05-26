CREATE DATABASE [SMFollow]
GO

USE [SMFollow]

CREATE TABLE [dbo].[FollowEntities]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Follower] NVARCHAR(200) NOT NULL, 
    [Following] NVARCHAR(200) NOT NULL
)
GO

CREATE TABLE [dbo].[FollowMetaData]
(
	[UserId] NVARCHAR(200) NOT NULL PRIMARY KEY, 
    [FollowersCount] INT NOT NULL, 
    [FriendsCount] INT NOT NULL
)
GO

CREATE TABLE [dbo].[FriendEntities]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [FromUser] NVARCHAR(200) NOT NULL, 
    [ToUser] NVARCHAR(200) NOT NULL, 
    [State] INT NOT NULL
)
GO