export class Notification {
  Id: string
  UserId: string
  IsRead: boolean
  Content: string
  UserProfileUrl: string
  Timestamp: Date
  Type: string
  PostId: string

  constructor(id?: string,
    userId?: string,
    isRead?: boolean,
    content?: string,
    userProfileUrl?: string,
    timestamp?: Date,
    type?: string,
    postId?: string
  ) {
    this.Id = id;
    this.UserId = userId;
    this.IsRead = isRead;
    this.Content = content;
    this.UserProfileUrl = userProfileUrl;
    this.Timestamp = timestamp;
    this.Type = type;
    this.PostId = postId;
  }
}
