import { PostImage } from "../postimage.model";
import { IModelAdapter } from "./interface/IModelAdapter.interface";

export class PostImageAdapter implements IModelAdapter<PostImage>{
  Adapt(data: any): PostImage {
    return new PostImage(
      data.id,
      data.postId,
      data.userId,
      data.imageUrl
    );
  }
}
