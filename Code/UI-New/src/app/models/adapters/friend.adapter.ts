import { Friend } from "../friend.model";
import { IModelAdapter } from "./interface/IModelAdapter.interface";

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
