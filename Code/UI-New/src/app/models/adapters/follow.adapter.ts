import { Follow } from "../follow.model";
import { IModelAdapter } from "./interface/IModelAdapter.interface";

export class FollowAdapter implements IModelAdapter<Follow>
{
    Adapt(data: any): Follow {
        return new Follow(
            data.Id,
            data.follower,
            data.following
        );
    }
}
