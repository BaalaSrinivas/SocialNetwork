import { User } from "../user.model";

export class UserAdapter implements IModelAdapter<User>
{
    Adapt(data: any): User {
        return new User(
            data.profileName,
            data.gender,
            data.profileImageId,
            data.about,
            data.location,
            data.headline
        );
    }
}