import { Friend } from "../friend.model";

export class FriendAdapter implements IModelAdapter<Friend>
{
    Adapt(data: any): Friend {
        return new Friend(
            data.id,
            data.fromUser,
            data.toUser,
            data.state
        )
    }

}