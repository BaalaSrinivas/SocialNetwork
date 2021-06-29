import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@microsoft/signalr';
import { environment } from '../../environments/environment';

const notificationUrl = environment.apiUrl + 'notification/hub';

@Injectable({
    providedIn: 'root'
})

export class SignalrService {
    private hubConnection: HubConnection;

    constructor(private httpClient: HttpClient) {

        var options: IHttpConnectionOptions = {
            accessTokenFactory: () => { return `${sessionStorage.getItem('idToken')}` }        
        };

        this.hubConnection = new HubConnectionBuilder().withUrl(notificationUrl, options).build();

        this.hubConnection.start().catch((err) => {
            console.log(err);
        });
    }

    addCallbackListener(callback: Function, methodName: string) {
        this.hubConnection.on(methodName, (data: string) => {            
            callback(data);
        });
    }
}
