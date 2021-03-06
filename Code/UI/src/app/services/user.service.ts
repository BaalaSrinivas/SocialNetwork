import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { UserAdapter } from "../models/adapters/user.adapter";
import { User } from "../models/user.model";

const userManagementApi = environment.apiUrl + 'v1/user';



@Injectable()
export class UserService {
  constructor(private _httpClient: HttpClient, private _userAdapter: UserAdapter) {

  }

  createUser(user: User, profileImage: File): Observable<User> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };

    const form = new FormData();
    var props = Object.keys(user);
    props.forEach(function (key) {
      form.append(key, user[key]);
    });
    form.append('ProfileImage', profileImage);
    return this._httpClient.post<string>(userManagementApi, form, httpOptions).pipe(map((item) => {
      if (item) {
        return this._userAdapter.Adapt(item)
      }
      return null;
    }));
  }

  getUser(userId: string): Observable<User> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };
    return this._httpClient.get<User>(userManagementApi + '?userId=' + userId, httpOptions).pipe(map((item) => {
      if (item) {
        return this._userAdapter.Adapt(item)
      }
      return null;
    }));
  }

  searchUser(userKey: string): Observable<User[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };
    return this._httpClient.get<User[]>(userManagementApi + '/search?key=' + userKey, httpOptions).pipe(map((data: any) => data.map(item => this._userAdapter.Adapt(item))));
  }
}
