export class Notification {
  Id: string
  UserId: string
  IsRead: boolean
  Content: string
  UserProfileUrl: string
  Timestamp: Date

  constructor(id?: string,
    userId?: string,
    isRead?: boolean,
    content?: string,
    userProfileUrl?: string,
    timestamp?: Date
  ) {
    this.Id = id;
    this.UserId = userId;
    this.IsRead = isRead;
    this.Content = content;
    this.UserProfileUrl = userProfileUrl;
    this.Timestamp = timestamp;
  }
}
