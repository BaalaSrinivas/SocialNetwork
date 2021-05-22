import { Comment } from "../comment.model";

export class CommentAdapter implements IModelAdapter<Comment> {
    Adapt(data: any): Comment {
        return new Comment(
            data.id,
            data.userId,
            data.commentText,
            data.postId,
            data.likesCount,
            data.timestamp
        );
    }

}