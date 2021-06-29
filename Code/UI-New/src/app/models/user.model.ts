export class User {
    ProfileName: string;
    Gender: number;
    About: string;
    Location: string;
    Headline: string;
    ProfileImageUrl: string;
    Name: string;
    MailId: string;

    constructor(
        profileName?: string,
        gender?: number,
        about?: string,
        location?: string,
        headline?: string,
        profileImageUrl?: string,
        name?: string,
        mailId?: string
    ) {
        this.ProfileName = profileName;
        this.Gender = gender;
        this.About = about;
        this.Location = location;
        this.Headline = headline;
        this.ProfileImageUrl = profileImageUrl;
        this.Name = name;
        this.MailId = mailId;
    }
}