import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { PostAdapter } from "./Models/Adapters/post.adapter";
import { Post } from "./Models/post.model";



const contentApi = environment.apiUrl + 'v1/content/';

@Injectable()
export class ContentService {

    constructor(private _httpClient: HttpClient, private _postAdapter: PostAdapter) {
        
    }

    addComment(guid: any, commentText: string): Observable<boolean> {
        var data = {
            postId: guid,
            commentText: commentText
        };
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.post<boolean>(contentApi + 'addcomment', data, httpOptions);
    }

    getPosts(guids: any[]): Observable<any> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
            })
        };
        return this._httpClient.post<Post[]>(contentApi + 'getposts', guids, httpOptions)
            .pipe(map((data: any[]) =>
                data.map(item => this._postAdapter.Adapt(item))
            )
       );
    }
}