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
    private _modalService: NgbModal) {
  }

  friendsCount: number = 0;
  postCount: number = 0;
  imageCount: number = 0;
  scrollIndex: number = 1;
  scrollLength: number = 5;
  lock: boolean = false;

  userpostIds: string[];

  user: User = new User();

  userImages: PostImage[];

  userPosts: Post[] = [];

  ngOnInit() {
    this._activatedRoute.queryParams.subscribe(params => {
      this.userPosts = undefined;
      this.mailId = params['mailid'] != undefined ? params['mailid'] : sessionStorage.getItem('mailId');
      this.isHost = this.mailId == sessionStorage.getItem('mailId');

      this._userService.getUser(this.mailId).subscribe(u => {
        this.user = u;
      });

      this.getFriendsCount();

      this.getUserPosts();

      this.getImages();

      this.getFriendFollowInfo();
    });
  }

  onScroll() {
    if (this.userpostIds !== undefined && this.userpostIds.length == 0) {
      this.userPosts = [];
    }
    if (this.userpostIds !== undefined && this.userpostIds.length >= (this.scrollIndex - 1) * this.scrollLength && !this.lock) {
      this.lock = true;
      this.fetchUserPost((this.scrollIndex - 1) * this.scrollLength, (this.scrollIndex) * this.scrollLength);
    }
  }

  private fetchUserPost(start: number, end: number) {
    this._contentService.getPosts(this.userpostIds.slice(start, end)).subscribe(posts => {
      if (this.userPosts === undefined) {
        this.userPosts = [];
      }
      this.userPosts.push(...posts);
      this.scrollIndex++;
      this.lock = false;
    });
  }

  RefreshPosts() {
    this.getUserPosts();
    this.getImages();
  }

  getFriendsCount() {
    this._followService.getUserFriendsCount(this.mailId).subscribe((data) => {
      this.friendsCount = data;
    });
  }

  getFriendFollowInfo() {
    this._followService.getFriendFollowInfo(this.mailId).subscribe(data => {
      this.friendState = data.friendState;
      this.isFollowing = data.isFollowing;
    });
  }

  getUserPosts() {
    this._contentService.getUserPostIds(this.mailId).subscribe(ids => {
      this.postCount = ids.length;
      this.userpostIds = ids;
      this.userPosts = [];
      this.scrollIndex = 1;
      this.onScroll();
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
      this.toastService.show({
        title: 'Success',
        content: 'Following ' + this.user.Name,
        class: 'text-success'
      });
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
    this._contentService.getImages(this.mailId).subscribe(data => {
      this.imageCount = data.length;
      this.userImages = data.slice(0, 9);

      this.userImages.forEach((c, i) => {
        //TODO: Replace size in URL
        var index = this.userImages[i].ImageUrl.lastIndexOf('.')
        this.userImages[i].ImageUrl = this.userImages[i].ImageUrl.substring(0, index) + "_1x1" + this.userImages[i].ImageUrl.substring(index);
      })
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
