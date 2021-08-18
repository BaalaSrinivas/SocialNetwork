import { IModelAdapter } from "./interface/IModelAdapter.interface";
import { Notification } from "../notification.model";

export class NotificationAdapter implements IModelAdapter<Notification> {
  Adapt(data: any): Notification {
    return new Notification(
      data.id,
      data.userId,
      data.isRead,
      data.content,
      data.userProfileUrl,
      data.timestamp,
      data.type,
      data.postId
    );
    }
}
