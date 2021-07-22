export class PostImage {
  Id: string
  PostId: string
  UserId: string
  ImageUrl: string

  constructor(
    id?: string,
    postId?: string,
    userId?: string,
    imageUrl?: string
  ) {
    this.Id = id;
    this.PostId = postId;
    this.UserId = userId;
    this.ImageUrl = imageUrl;
  }
}
