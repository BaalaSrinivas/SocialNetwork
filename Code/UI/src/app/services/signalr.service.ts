import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { NotificationAdapter } from '../models/adapters/notification.adapter';
import { Notification } from '../models/notification.model';

const notificationUrl = environment.apiUrl + 'notification/hub';

@Injectable({
  providedIn: 'root'
})

export class SignalrService {
  private hubConnection: HubConnection;

  constructor(private httpClient: HttpClient,
    private _notificationAdapter: NotificationAdapter) {

    var options: IHttpConnectionOptions = {
      accessTokenFactory: () => { return `${sessionStorage.getItem('accessToken')}` }
    };

    this.hubConnection = new HubConnectionBuilder().withUrl(notificationUrl, options).build();

    this.hubConnection.start().catch((err) => {
      console.log(err);
    });
  }

  addCallbackListener(callback: Function, methodName: string) {
    this.hubConnection.on(methodName, (data: Notification) => {
      callback(this._notificationAdapter.Adapt(data));
    });
  }
}
