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

  @Input()
  post: Post = new Post();
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
      this.post.LikeCount = l;
      this.post.HasUserLiked = !this.post.HasUserLiked;
    });
  }

  addComment() {
    this.comment.PostId = this.post.Id
    this._contentService.addComment(this.comment).subscribe(r => {
      console.log(r);
      this.post.CommentCount = r;
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
