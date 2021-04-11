import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { AuthenticationService } from "../shared/authentication.service";
import { PostAdapter } from "./Models/Adapters/post.adapter";
import { Post } from "./Models/post.model";

@Injectable()
export class ContentService {
    headers: any;

    constructor(private authService: AuthenticationService, private httpClient: HttpClient, private postAdapter: PostAdapter) {
        this.authService.loginChanged.subscribe(x => {
            this.setAccessToken();
        });
    }

    setAccessToken() {
        this.authService.getAccessToken().then(token => {
            this.headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        });
    }

    addComment(guid: any, commentText: string): Observable<boolean> {
        var data = {
            postId: guid,
            commentText: commentText
        };
        return this.httpClient.post<boolean>(environment.apiUrl + 'addcomment', data, { headers: this.headers });
    }

    getPosts(guids: any[]): Observable<any> {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
        })

        return this.httpClient.post<Post[]>(environment.apiUrl + 'getposts', guids, { headers: headers })
            .pipe(map((data: any[]) =>
                data.map(item => this.postAdapter.Adapt(item))
            )
       );
    }
}