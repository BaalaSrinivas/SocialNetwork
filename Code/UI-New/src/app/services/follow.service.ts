import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { FollowAdapter } from "../models/adapters/follow.adapter";
import { FriendAdapter } from "../models/adapters/friend.adapter";
import { Follow } from "../models/follow.model";
import { FollowMetaData } from "../models/followmetadata.model";
import { Friend } from "../models/friend.model";

const followApi = environment.apiUrl + 'v1/follow/';

@Injectable()
export class FollowService {
    constructor(private _httpClient: HttpClient, private _friendAdapter: FriendAdapter, private _followAdapter: FollowAdapter) {

    }
    

    getUserFollowInfo(): Observable<FollowMetaData> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.get<FollowMetaData>(followApi + 'getuserfollowinfo', httpOptions);
    }

    followUser(userId: string): Observable<boolean> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.post<boolean>(followApi + 'followuser', userId, httpOptions);
    }

    unFollowUser(followEntity: Follow): Observable<boolean> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.post<boolean>(followApi + 'unfollowuser', followEntity, httpOptions);
    }

    sendFriendRequest(userId: string): Observable<boolean> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };

        return this._httpClient.post<boolean>(followApi + 'sendfriendrequest', JSON.stringify(userId), httpOptions);
    }

    unFriend(friendEntity: Friend): Observable<boolean> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.post<boolean>(followApi + 'unfriend', friendEntity, httpOptions);
    }

    acceptFriendRequest(friendEntity: Friend): Observable<boolean> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.post<boolean>(followApi + 'acceptfriendrequest', friendEntity, httpOptions);
    }

    deleteFriendRequest(friendEntity: Friend): Observable<boolean> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.post<boolean>(followApi + 'deleteFriendRequest', friendEntity, httpOptions);
    }

    getFriendRequests(): Observable<Friend[]> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.get<Friend[]>(followApi + 'getfriendrequests', httpOptions)
            .pipe(map((data: any[]) =>
                data.map(item => this._friendAdapter.Adapt(item))
            ));
    }

    getFriends(): Observable<Friend[]> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.get<Friend[]>(followApi + 'getfriends', httpOptions).pipe(map((data: any[]) =>
            data.map(item => this._friendAdapter.Adapt(item))
        ));
    }
}