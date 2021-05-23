export class Post {
    Id: string
    UserId: string
    Content: string
    LikeCount: number
    CommentCount: number
    Timestamp: Date
    HasUserLiked: boolean

    constructor(id?: string, userId?: string, content?: string, likeCount?: number, commentCount?: number, timestamp?: Date, hasUserLiked?: boolean) {
        this.Id = id;
        this.UserId = userId;
        this.Content = content;
        this.LikeCount = likeCount;
        this.CommentCount = commentCount;
        this.Timestamp = timestamp;
        this.HasUserLiked = hasUserLiked;
    }
}