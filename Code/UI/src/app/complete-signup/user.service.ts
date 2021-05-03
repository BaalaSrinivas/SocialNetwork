import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { User } from "./Models/user.model";

const userManagementApi = environment.apiUrl + 'v1/user';

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}`
    })
};

@Injectable()
export class UserService {
    constructor(private _httpClient: HttpClient) {

    }

    createUser(user: User): Observable<boolean> {
        return this._httpClient.post<boolean>(userManagementApi , user, httpOptions);
    }
}