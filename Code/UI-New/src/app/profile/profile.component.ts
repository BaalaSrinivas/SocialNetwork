import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from '../models/post.model';
import { PostImage } from '../models/postimage.model';
import { User } from '../models/user.model';
import { ContentService } from '../services/content.service';
import { FollowService } from '../services/follow.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  mailId: string
  showAddPost: boolean;

  constructor(private _contentService: ContentService,
    private _userService: UserService,
    private _followService: FollowService,
    private _activatedRoute: ActivatedRoute) {

  }

  user: User = new User();
  userName: string;

  userImages: PostImage[];

  userPosts: Post[];

  ngOnInit() {
    this._activatedRoute.queryParams.subscribe(params => {
      this.mailId = params['mailid'] != undefined ? params['mailid'] : sessionStorage.getItem('mailId');
      this.showAddPost = this.mailId == sessionStorage.getItem('mailId');

      this._userService.getUser(this.mailId).subscribe(u => {
        this.user = u;
      });

      this._contentService.getUserPostIds(20, this.mailId).subscribe(ids => {

        this._contentService.getPosts(ids).subscribe(posts => {
          this.userPosts = posts;
          console.log(this.userPosts);
        });
      });

      this.getImages();
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
    this._contentService.getImages(6).subscribe(data => {
      this.userImages = data;
    });
  }

}
