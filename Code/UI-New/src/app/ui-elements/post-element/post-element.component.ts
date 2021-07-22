import { Component, Input, OnInit } from '@angular/core';
import { Post } from '../../models/post.model';
import { Comment } from '../../models/comment.model';
import { ContentService } from '../../services/content.service';

@Component({
  selector: 'app-post-element',
  templateUrl: './post-element.component.html',
  styleUrls: ['./post-element.component.css']
})
export class PostElementComponent implements OnInit {

  _post: Post = new Post();
  content: string;

  @Input() set post(value: any) {
    this._post = value;
    //TODO: This is poor handling, but fed up with css, will fix it after some days
    this.content = this._post.Content.replace(new RegExp("<img", "g"), "<img class=\"img-fluid\"");
  }
  get post() {
    return this._post;
  }

  profileUrl: string;

  comment: Comment = new Comment();

  postComments: Comment[];

  constructor(private _contentService: ContentService) { }
  commentToggle: boolean = false;


  ngOnInit(): void {
    this.profileUrl = sessionStorage.getItem('profileUrl');
  }

  change() {
    this.commentToggle = !this.commentToggle;
    if (this.commentToggle) {
      this.getComments();
    }
  }

  likePost() {
    this._contentService.likePost(this.post.Id).subscribe(l => {
      this._post.LikeCount = l;
      this._post.HasUserLiked = !this.post.HasUserLiked;
    });
  }

  addComment() {
    this.comment.PostId = this.post.Id
    this._contentService.addComment(this.comment).subscribe(r => {
      console.log(r);
      this._post.CommentCount = r;
      this.comment = new Comment();
      this.getComments();
    });
  }

  getComments() {
    this._contentService.getComments(this.post.Id).subscribe(comments => {
      this.postComments = comments;
    });
  }
}
