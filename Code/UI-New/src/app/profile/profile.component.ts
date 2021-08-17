import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Post } from '../models/post.model';
import { PostImage } from '../models/postimage.model';
import { User } from '../models/user.model';
import { ContentService } from '../services/content.service';
import { FollowService } from '../services/follow.service';
import { ToastService } from '../services/toast.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  mailId: string
  isHost: boolean;
  friendState: number = 2;
  isFollowing: boolean;

  constructor(private _contentService: ContentService,
    private _userService: UserService,
    private _followService: FollowService,
    private _activatedRoute: ActivatedRoute,
    private toastService: ToastService,
    private _modalService: NgbModal  ) {
  }

  user: User = new User();

  userImages: PostImage[];

  userPosts: Post[];

  ngOnInit() {
    this._activatedRoute.queryParams.subscribe(params => {
      this.mailId = params['mailid'] != undefined ? params['mailid'] : sessionStorage.getItem('mailId');
      this.isHost = this.mailId == sessionStorage.getItem('mailId');

      this._userService.getUser(this.mailId).subscribe(u => {
        this.user = u;
      });

      this.getUserPosts();

      this.getImages();

      this.getFriendFollowInfo();
    });
  }

  getFriendFollowInfo() {
    this._followService.getFriendFollowInfo(this.mailId).subscribe(data => {
      this.friendState = data.friendState;
      this.isFollowing = data.isFollowing;
    });
  }

  getUserPosts() {
    this._contentService.getUserPostIds(20, this.mailId).subscribe(ids => {
      this._contentService.getPosts(ids).subscribe(posts => {
        this.userPosts = posts;
        console.log(this.userPosts);
      });
    });
  }

  sendFriendRequest() {
    this._followService.sendFriendRequest(this.mailId).subscribe(data => {
      this.toastService.show({
        title: 'Success',
        content: 'Request sent successfully',
        class: 'text-success'
      });
    });
  }

  followUser() {
    this._followService.followUser(this.mailId).subscribe(data => {
      this.getFriendFollowInfo();
    });
  }

  unfollowUserConfirmation() {
    const model = this._modalService.open(NgbdModalConfirmUnfollow);
    model.componentInstance.userName = this.user.Name;
    model.componentInstance.clickevent.subscribe(($e) => {
      this.unfollowUser();
    });
  }

  unfollowUser() {
    this._followService.unFollowUser(this.mailId).subscribe(data => {
      this.getFriendFollowInfo();
    });
  }

  getImages() {
    this._contentService.getImages(6, this.mailId).subscribe(data => {
      this.userImages = data;
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
    <p><strong>Are you sure you want to unfollow <span class="text-primary">{{userName}}</span>?</strong></p>
  </div>
  <div class="modal-footer">
    <button type="button" class="btn btn-outline-secondary" (click)="modal.dismiss('cancel click')">Cancel</button>&nbsp;
    <button type="button" class="btn btn-primary" (click)="Ok()">Yes</button>
  </div>
  `
})
export class NgbdModalConfirmUnfollow {

  @Output() clickevent = new EventEmitter<boolean>();

  constructor(public modal: NgbActiveModal) { }
  userName: string;

  Ok() {
    this.clickevent.emit(true);
    this.modal.dismiss();
  }
}
