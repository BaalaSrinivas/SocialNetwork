import { Post } from "../post.model";
import { IModelAdapter } from "./interface/IModelAdapter.interface";

export class PostAdapter implements IModelAdapter<Post> {
    Adapt(data: any): Post {
        return new Post(
            data.id,
            data.userId,
            data.content,
            data.likeCount,
            data.commentCount,
            data.timestamp,
            data.hasUserLiked,
            data.userName,
            data.profileImageUrl
        );
    }
}
