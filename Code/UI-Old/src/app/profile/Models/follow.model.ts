export class Follow {
    Id: string;
    Follower: string;
    Following: string;

    constructor(id?: string, follower?: string, following?: string) {
        this.Id = id;
        this.Follower = follower;
        this.Following = following;
    }
}