export class Friend {
    Id: string;
    FromUser: string;
    ToUser: string;
    State: number;

    constructor(id?: string, fromUser?: string, toUser?: string, state?: number) {
        this.Id = id;
        this.FromUser = fromUser;
        this.ToUser = toUser;
        this.State = state;
    }
}