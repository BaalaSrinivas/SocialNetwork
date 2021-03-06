import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { PostAdapter } from "../models/adapters/post.adapter";
import { Post } from "../models/post.model";
import { Comment } from '../models/comment.model';
import { CommentAdapter } from "../models/adapters/comment.adapter";
import { PostImageAdapter } from "../models/adapters/postimage.adapter";
import { PostImage } from "../models/postimage.model";


const contentApi = environment.apiUrl + 'v1/content/';

@Injectable()
export class ContentService {

  constructor(private _httpClient: HttpClient,
    private _postAdapter: PostAdapter,
    private _commentAdapter: CommentAdapter,
    private _postImageAdapter: PostImageAdapter) {

  }

  addComment(comment: Comment): Observable<number> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };
    return this._httpClient.post<number>(contentApi + 'addcomment', comment, httpOptions);
  }

  getComments(postId: string): Observable<Comment[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };
    return this._httpClient.get<Comment[]>(contentApi + 'getcomments?postid=' + postId, httpOptions)
      .pipe(map((data: any[]) =>
        data.map(item => this._commentAdapter.Adapt(item))
      )
      );
  }

  getPosts(guids: string[]): Observable<Post[]> {
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

  getImages(userId: string): Observable<PostImage[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };

    var data = {
      "userId": userId
    }

    return this._httpClient.post<PostImage[]>(contentApi + 'getuserimages', data, httpOptions)
      .pipe(map((data: any[]) =>
        data.map(item => this._postImageAdapter.Adapt(item))
      ));
  }

  createPost(post: Post): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };
    return this._httpClient.post<boolean>(contentApi + 'createpost', post, httpOptions);
  }

  deletePost(postId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };

    var data = {
      "Id": postId
    }
    return this._httpClient.post<boolean>(contentApi + 'deletepost', data, httpOptions);
  }

  likePost(postId: string): Observable<number> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };
    return this._httpClient.get<number>(contentApi + 'likepost?postId=' + postId, httpOptions);
  }

  getUserPostIds(userId: string): Observable<string[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('idToken')}`
      })
    };
    var data = {
      'userId': userId
    }
    return this._httpClient.post<string[]>(contentApi + 'getuserposts', data, httpOptions);
  }
}
