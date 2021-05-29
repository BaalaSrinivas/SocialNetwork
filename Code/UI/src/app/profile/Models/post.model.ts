export class Post {
    Id: string
    UserId: string
    Content: string
    LikeCount: number
    CommentCount: number
    Timestamp: Date
    HasUserLiked: boolean
    UserName: string
    ProfileImageUrl: string

    constructor(
        id?: string,
        userId?: string,
        content?: string,
        likeCount?: number,
        commentCount?: number,
        timestamp?: Date,
        hasUserLiked?: boolean,
        userName?: string,
        profileImageUrl?: string
    ) {
        this.Id = id;
        this.UserId = userId;
        this.Content = content;
        this.LikeCount = likeCount;
        this.CommentCount = commentCount;
        this.Timestamp = timestamp;
        this.HasUserLiked = hasUserLiked;
        this.UserName = userName;
        this.ProfileImageUrl = profileImageUrl;
    }
}