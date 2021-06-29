export class Friend {
    Id: string;
    UserId: string;
    UserName: string
    ProfileImageUrl: string
    UserHeadline: string

    constructor(id?: string,
        userId?: string,
        userName?: string,
        profileImageUrl?: string,
        userHeadline?: string
    ) {
        this.Id = id;
        this.UserId = userId;
        this.UserName = userName;
        this.ProfileImageUrl = profileImageUrl;
        this.UserHeadline = userHeadline
    }
}