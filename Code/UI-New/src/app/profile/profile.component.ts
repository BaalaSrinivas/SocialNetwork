import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  isFriend: boolean;
  isFollowing: boolean;

  constructor(private _contentService: ContentService,
    private _userService: UserService,
    private _followService: FollowService,
    private _activatedRoute: ActivatedRoute,
    private toastService: ToastService) {
  }

  user: User = new User();
  userName: string;

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

      this._followService.getFriendFollowInfo(this.mailId).subscribe(data => {
        this.isFriend = data.isFriend;
        this.isFollowing = data.isFollowing;
      });
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

    });
  }

  followUser() {
    this._followService.followUser(this.mailId).subscribe(data => {

    });
  }

  getImages() {
    this._contentService.getImages(6, this.mailId).subscribe(data => {
      this.userImages = data;
    });
  }

}
