import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { AuthenticationService } from "../shared/authentication.service";
import { PostAdapter } from "./Models/Adapters/post.adapter";
import { Post } from "./Models/post.model";

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}`
    })
};

@Injectable()
export class ContentService {


    constructor(private authService: AuthenticationService, private httpClient: HttpClient, private postAdapter: PostAdapter) {

    }


    addComment(guid: any, commentText: string): Observable<boolean> {
        var data = {
            postId: guid,
            commentText: commentText
        };
        return this.httpClient.post<boolean>(environment.apiUrl + 'addcomment', data, httpOptions);
    }

    getPosts(guids: any[]): Observable<any> {


        return this.httpClient.post<Post[]>(environment.apiUrl + 'getposts', guids, httpOptions)
            .pipe(map((data: any[]) =>
                data.map(item => this.postAdapter.Adapt(item))
            )
       );
    }
}