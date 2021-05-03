export class User {
    ProfileName: string;
    Gender: number;
    ProfileImageId: string;
    About: string;
    Location: string;
    Headline: string;

    constructor(
        profileName?: string,
        gender?: number,
        profileImageId?: string,
        about?: string,
        location?: string,
        headline?: string
    ) {
        this.ProfileName = profileName;
        this.Gender = gender;
        this.ProfileImageId = profileImageId;
        this.About = about;
        this.Location = location;
        this.Headline = headline;
    }
}