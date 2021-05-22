export class Comment {
    Id: string
    UserId: string
    CommentText: string
    PostId: string
    LikesCount: number
    Timestamp: Date

    constructor(id?: string, userId?: string, commentText?: string, postId?: string, likesCount?: number, timestamp?: Date) {
        this.Id = id;
        this.UserId = userId;
        this.CommentText = commentText;
        this.PostId = postId;
        this.LikesCount = likesCount;
        this.Timestamp = timestamp;
    }
}