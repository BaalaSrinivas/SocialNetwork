import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";

const newsfeedUrl = environment.apiUrl + 'v1/newsfeed/';

@Injectable()
export class NewsfeedService {

    constructor(private _httpClient: HttpClient) {

    }

    getNewsfeed(userId: string): Observable<string[]> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };

        return this._httpClient.get<string[]>(newsfeedUrl + 'getnewsfeed?userid=' + userId, httpOptions);
    }

}
