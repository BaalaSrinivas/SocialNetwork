import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Post } from '../../models/post.model';
import { Comment } from '../../models/comment.model';
import { ContentService } from '../../services/content.service';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-post-element',
  templateUrl: './post-element.component.html',
  styleUrls: ['./post-element.component.css']
})
export class PostElementComponent implements OnInit {

  _post: Post = new Post();
  content: string;

  @Output() onPostDelete = new EventEmitter<boolean>();
  @Input() showDelete: boolean = false;

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

  constructor(private _contentService: ContentService,
    private _modalService: NgbModal   ) { }
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

  DeleteConfirmation(post: Post) {
    const modal = this._modalService.open(NgbdModalConfirmDeletePost);
    modal.componentInstance.post = post;
    modal.componentInstance.clickevent.subscribe(($e) => {
      this.DeletePost($e);
    })
  }

  DeletePost(post: Post) {
    this._contentService.deletePost(post.Id).subscribe((data) => {
      this.onPostDelete.emit(true);
    });
  }
}

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
  <div class="modal-header">
    <h5 class="modal-title" id="modal-title">Delete Post</h5>
    <button type="button" class="close" aria-describedby="modal-title" (click)="modal.dismiss('Cross click')">
      <span aria-hidden="true">&times;</span>
    </button>
  </div>
  <div class="modal-body">
    <p><strong>Are you sure you want to delete this post ?</strong></p>
    <p><span class="text-danger">This operation can not be undone.</span></p>
  </div>
  <div class="modal-footer">
    <button type="button" class="btn btn-outline-secondary" (click)="modal.dismiss('cancel click')">Cancel</button>&nbsp;
    <button type="button" class="btn btn-primary" (click)="Ok(post)">Yes</button>
  </div>
  `
})
export class NgbdModalConfirmDeletePost {

  @Output() clickevent = new EventEmitter<Post>();

  constructor(public modal: NgbActiveModal) { }
  post: Post;

  Ok(post: Post) {
    this.clickevent.emit(post);
    this.modal.dismiss();
  }
}

