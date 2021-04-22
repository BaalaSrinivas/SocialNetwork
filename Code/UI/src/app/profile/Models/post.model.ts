﻿export class Post {
    Id: string
    UserId: string
    Content: string
    LikeCount: number
    CommentCount: number

    constructor(id: string, userId: string, content: string, likeCount: number, commentCount: number) {
        this.Id = id;
        this.UserId = userId;
        this.Content = content;
        this.LikeCount = likeCount;
        this.CommentCount = commentCount;
    }
}