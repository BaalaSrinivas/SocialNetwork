import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { NotificationAdapter } from "../models/adapters/notification.adapter";
import { Notification } from "../models/notification.model";

const notifiactionApi = environment.apiUrl + 'v1/notification/';

@Injectable()
export class NotificationService {

  constructor(private _httpClient: HttpClient,
    private _notificationAdapter: NotificationAdapter) {

  }

  getNotifications(): Observable<Notification[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };

    return this._httpClient.get<Notification[]>(notifiactionApi + 'GetUnreadNotifications', httpOptions)
      .pipe(map((data: any[]) =>
        data.map(item => this._notificationAdapter.Adapt(item))
      ));
  }

  UpdateNotificationReadStatus(notificationId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })       
    };
    var data = {
      'Id': notificationId
    }
    return this._httpClient.post<boolean>(notifiactionApi + 'updatenotificationreadstatus', data, httpOptions)
  }

}
