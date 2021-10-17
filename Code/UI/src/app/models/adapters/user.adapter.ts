import { User } from "../user.model";
import { IModelAdapter } from "./interface/IModelAdapter.interface";

export class UserAdapter implements IModelAdapter<User>
{
  Adapt(data: any): User {
    //TODO: Replace size in URL
    const index = data.profileImageUrl.lastIndexOf('.');
    data.profileImageUrl = data.profileImageUrl.substring(0, index) + "_1x1" + data.profileImageUrl.substring(index);
        return new User(
            data.profileName,
            data.gender,
            data.about,
            data.location,
            data.headline,
            data.profileImageUrl,
            data.name,
            data.mailId
        );
    }
}
