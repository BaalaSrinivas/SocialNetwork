import { Friend } from "../friend.model";

export class FriendAdapter implements IModelAdapter<Friend>
{
    Adapt(data: any): Friend {
        return new Friend(
            data.id,
            data.userId,
            data.userName,
            data.profileImageUrl,
            data.userHeadline
        )
    }

}